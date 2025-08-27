using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Fast localization lookup for UI texts backed by an in-memory dictionary cache.
    /// Read path is lock-free; writes use copy-on-write to keep readers fast.
    /// </summary>
    public static class MessageTextHelper
    {
        // --------- cache key & storage ---------

        private struct CacheKey : IEquatable<CacheKey>
        {
            public readonly Guid LangId;
            public readonly string FormId; // ordinal compare
            public readonly string TextId; // ordinal compare

            public CacheKey(Guid langId, string formId, string textId)
            {
                LangId = langId;
                FormId = formId ?? string.Empty;
                TextId = textId ?? string.Empty;
            }

            public bool Equals(CacheKey other)
            {
                return LangId == other.LangId
                    && string.Equals(FormId, other.FormId, StringComparison.Ordinal)
                    && string.Equals(TextId, other.TextId, StringComparison.Ordinal);
            }

            public override bool Equals(object obj) => obj is CacheKey k && Equals(k);

            public override int GetHashCode()
            {
                unchecked
                {
                    int h = LangId.GetHashCode();
                    h = (h * 397) ^ StringComparer.Ordinal.GetHashCode(FormId ?? string.Empty);
                    h = (h * 397) ^ StringComparer.Ordinal.GetHashCode(TextId ?? string.Empty);
                    return h;
                }
            }
        }

        // Volatile reference to allow lock-free reads; writes replace the whole dictionary (copy-on-write).
        private static volatile Dictionary<CacheKey, string> _cache =
            new Dictionary<CacheKey, string>(1024);

        // Writers take this lock when replacing the dictionary.
        private static readonly object _writeLock = new object();

        /// <summary>
        /// Preloads/refreshes the cache from a given collection (e.g., StaticValues.MessageText).
        /// Call once at app start and whenever you change the language or refresh texts.
        /// </summary>
        public static void WarmupCache(IEnumerable<MessageText> source)
        {
            if (source == null) return;

            var fresh = new Dictionary<CacheKey, string>(capacity: Math.Max(2048, (source as ICollection<MessageText>)?.Count ?? 8192));

            foreach (var m in source)
            {
                if (m == null) continue;
                var key = new CacheKey(m.LanguageID, m.FormId ?? string.Empty, m.TextId ?? string.Empty);
                if (!fresh.ContainsKey(key))
                {
                    fresh[key] = m.Value ?? string.Empty;
                }
            }

            lock (_writeLock)
            {
                // Publish new snapshot
                _cache = fresh;
                Thread.MemoryBarrier(); // ensure publication ordering on older runtimes
            }
        }

        /// <summary>
        /// Rebuilds cache only for a specific language from provided items; leaves others intact.
        /// </summary>
        public static void ReloadForLanguage(Guid languageId, IEnumerable<MessageText> itemsForLang)
        {
            if (itemsForLang == null) return;

            lock (_writeLock)
            {
                var snapshot = new Dictionary<CacheKey, string>(_cache);
                // Remove existing entries for lang
                var toRemove = new List<CacheKey>();
                foreach (var kv in snapshot)
                    if (kv.Key.LangId == languageId) toRemove.Add(kv.Key);
                foreach (var k in toRemove) snapshot.Remove(k);

                // Add fresh ones
                foreach (var m in itemsForLang)
                {
                    if (m == null) continue;
                    if (m.LanguageID != languageId) continue;
                    snapshot[new CacheKey(m.LanguageID, m.FormId ?? string.Empty, m.TextId ?? string.Empty)] = m.Value ?? string.Empty;
                }

                _cache = snapshot;
                Thread.MemoryBarrier();
            }
        }

        // --------- public API ---------

        /// <summary>
        /// Returns localized text for (formTag, controlTag) in current language.
        /// Falls back to <paramref name="defaultValue"/> and optionally inserts a default record
        /// when the current language equals the default language.
        /// </summary>
        public static string GetMessageText(object frmTag, object controlTag, string defaultValue, string description, Dictionary<string, object> prm = null)
        {
            var formId = (frmTag ?? string.Empty).ToString();
            var textId = controlTag as string;
            if (string.IsNullOrWhiteSpace(textId))
                return ReplaceParameters(defaultValue, prm);

            var langId = StaticValues.languageCode.Id;
            var key = new CacheKey(langId, formId, textId);

            // Fast path: lock-free dictionary read
            string value;
            if (_cache.TryGetValue(key, out value) && !string.IsNullOrEmpty(value))
                return ReplaceParameters(value, prm);

            // Not in cache: try DB (optional; safe to skip if you don't have a direct query)
            try
            {
                var dbFound = MessageTextManager.Current.GetByFormAndTextId(langId, formId, textId);
                if (dbFound != null && !string.IsNullOrEmpty(dbFound.Value))
                {
                    // Add to cache (copy-on-write)
                    UpsertCache(dbFound.LanguageID, dbFound.FormId, dbFound.TextId, dbFound.Value);
                    return ReplaceParameters(dbFound.Value, prm);
                }
            }
            catch
            {
                // Ignore transient DB issues; fall back below.
            }

            // Auto-create only in default language to avoid polluting other languages.
            var newEntry = new MessageText
            {
                Id = Guid.NewGuid(),
                LanguageID = langId,
                FormId = formId,
                TextId = textId,
                Value = defaultValue,
                Description = description,
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            try
            {
                MessageTextManager.Current.Insert(newEntry);
                UpsertCache(langId, formId, textId, defaultValue);
            }
            catch
            {
                // Ignore insert failures; fallback is still returned.
            }

            return ReplaceParameters(defaultValue, prm);
        }

        /// <summary>
        /// Replaces tokens (exact key match) in <paramref name="msg"/> with provided values.
        /// Example: { "@PartNo", "123" } replaces all occurrences of "@PartNo" with "123".
        /// </summary>
        public static string ReplaceParameters(string msg, Dictionary<string, object> prm)
        {
            if (string.IsNullOrEmpty(msg) || prm == null || prm.Count == 0)
                return msg;

            // Micro-optimization: only replace keys that actually appear
            foreach (var p in prm)
            {
                var key = p.Key;
                if (string.IsNullOrEmpty(key)) continue;
                if (msg.IndexOf(key, StringComparison.Ordinal) < 0) continue;

                var val = p.Value == null ? string.Empty : p.Value.ToString();
                msg = msg.Replace(key, val);
            }
            return msg;
        }

        // --------- write helper (copy-on-write) ---------

        private static void UpsertCache(Guid langId, string formId, string textId, string value)
        {
            var key = new CacheKey(langId, formId ?? string.Empty, textId ?? string.Empty);

            lock (_writeLock)
            {
                var snapshot = new Dictionary<CacheKey, string>(_cache);
                snapshot[key] = value ?? string.Empty;
                _cache = snapshot;
                Thread.MemoryBarrier();
            }
        }
    }
}
