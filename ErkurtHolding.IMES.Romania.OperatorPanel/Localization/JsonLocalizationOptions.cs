using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Options for JsonText provider. Values default from AppSettings.
    /// </summary>
    public sealed class JsonLocalizationOptions
    {
        public string BasePath { get; }
        public string DefaultLanguage { get; }
        public bool ReloadOnChange { get; }

        public JsonLocalizationOptions()
        {
            BasePath = ConfigurationManager.AppSettings["Localization:BasePath"] ?? "Locales";
            DefaultLanguage = ConfigurationManager.AppSettings["Localization:Default"] ?? "en_EN";
            ReloadOnChange = string.Equals(
                ConfigurationManager.AppSettings["Localization:ReloadOnChange"], "true",
                StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// JSON-backed localization that reads "xx_XX.json" from a folder.
    /// Thread-safe, cached per language, with fallback to default and neutral language ("en").
    /// </summary>
    public sealed class JsonText : IText, IDisposable
    {
        private static readonly Lazy<JsonLocalizationOptions> _opts = new Lazy<JsonLocalizationOptions>(() => new JsonLocalizationOptions(), LazyThreadSafetyMode.ExecutionAndPublication);

        // Cache: language -> dictionary
        private static readonly ConcurrentDictionary<string, Lazy<Dictionary<string, string>>> _cache
            = new ConcurrentDictionary<string, Lazy<Dictionary<string, string>>>(StringComparer.OrdinalIgnoreCase);

        private static FileSystemWatcher _watcher;
        private static int _watcherInit;

        private readonly string _language;

        /// <summary>
        /// Creates a provider using AppSettings["Language"] or "en_EN".
        /// </summary>
        public JsonText()
            : this(ConfigurationManager.AppSettings["Localization:Default"] ?? "en_EN")
        {
        }

        /// <summary>
        /// Creates a provider for a specific language code like "en_EN" or "tr_TR".
        /// </summary>
        public JsonText(string language)
        {
            _language = Normalize(language);
            EnsureWatcher();
        }

        public IText For(string languageCode) => new JsonText(languageCode);

        public string this[string key, params (string name, object value)[] args]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                    return string.Empty;

                // 1) exact language (e.g., en_EN)
                // 2) neutral (e.g., en)
                // 3) default (e.g., en_EN from options)
                // 4) key (as last resort)
                var candidates = EnumerateLanguageFallbacks(_language).ToList();
                foreach (var lang in candidates)
                {
                    var dict = GetOrLoad(lang);
                    if (dict != null && dict.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value))
                        return Format(value, args);
                }

                // As a last resort, return the key—safe for debugging and won’t crash UI.
                return Format(key, args);
            }
        }

        public void Dispose()
        {
            // Nothing per-instance. Watcher is static; keep it alive across instances.
        }

        // --- Internals ---

        private static string Normalize(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                return _opts.Value.DefaultLanguage;

            // Accept forms like "en-EN", "en_EN", "en"; canonicalize to underscore with region uppercase
            var sanitized = lang.Replace('-', '_');
            if (sanitized.Length == 2) // "en"
                return sanitized.ToLowerInvariant();

            var parts = sanitized.Split('_');
            if (parts.Length == 2)
                return $"{parts[0].ToLowerInvariant()}_{parts[1].ToUpperInvariant()}";

            return sanitized;
        }

        private static IEnumerable<string> EnumerateLanguageFallbacks(string lang)
        {
            // exact
            yield return lang;

            // neutral
            var idx = lang.IndexOf('_');
            if (idx > 0)
                yield return lang.Substring(0, idx);

            // default from options
            yield return Normalize(_opts.Value.DefaultLanguage);
        }

        private static Dictionary<string, string> GetOrLoad(string lang)
        {
            var lazy = _cache.GetOrAdd(lang, l => new Lazy<Dictionary<string, string>>(
                () => LoadLanguageFile(l), LazyThreadSafetyMode.ExecutionAndPublication));

            try { return lazy.Value; }
            catch
            {
                // If load failed, drop the bad entry so a later retry can succeed.
                _cache.TryRemove(lang, out _);
                return null;
            }
        }

        private static Dictionary<string, string> LoadLanguageFile(string lang)
        {
            var basePath = Path.GetFullPath(_opts.Value.BasePath);
            var candidates = new[]
            {
                Path.Combine(basePath, $"{lang}.json"),
                Path.Combine(basePath, $"{lang.ToLowerInvariant()}.json"),
                Path.Combine(basePath, $"{lang.ToUpperInvariant()}.json")
            };

            string chosen = candidates.FirstOrDefault(File.Exists);
            if (chosen == null)
            {
                // Return empty to allow fallback to next language
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            var json = File.ReadAllText(chosen);
            // Allow both flat { "a.b": "..." } and nested { "a": { "b": "..." } }
            var raw = JsonConvert.DeserializeObject<object>(json);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Flatten(raw, "", dict);
            return dict;
        }

        private static void Flatten(object node, string prefix, IDictionary<string, string> dict)
        {
            if (node == null) return;

            if (node is Newtonsoft.Json.Linq.JValue v)
            {
                dict[prefix.Trim('.')] = v.ToString();
                return;
            }
            if (node is Newtonsoft.Json.Linq.JObject o)
            {
                foreach (var prop in o.Properties())
                {
                    var key = string.IsNullOrEmpty(prefix) ? prop.Name : prefix + "." + prop.Name;
                    Flatten(prop.Value, key, dict);
                }
                return;
            }
            if (node is Newtonsoft.Json.Linq.JArray a)
            {
                for (int i = 0; i < a.Count; i++)
                {
                    var key = string.IsNullOrEmpty(prefix) ? i.ToString() : prefix + "." + i;
                    Flatten(a[i], key, dict);
                }
                return;
            }
        }

        private static string Format(string template, params (string name, object value)[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(template))
                return template ?? string.Empty;

            // Replace {name} (case-insensitive). Avoid string.Format; we want named tokens.
            // If you later want ICU MessageFormat, you can plug it here.
            var result = template;
            foreach (var (name, value) in args)
            {
                if (string.IsNullOrEmpty(name)) continue;
                var pattern = @"\{" + Regex.Escape(name) + @"\}";
                result = Regex.Replace(result, pattern,
                    value?.ToString() ?? string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
            return result;
        }

        private static void EnsureWatcher()
        {
            if (!_opts.Value.ReloadOnChange) return;
            if (Interlocked.CompareExchange(ref _watcherInit, 1, 0) != 0) return;

            var basePath = Path.GetFullPath(_opts.Value.BasePath);
            Directory.CreateDirectory(basePath);

            _watcher = new FileSystemWatcher(basePath, "*.json")
            {
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime
            };
            _watcher.Changed += (_, __) => InvalidateAll();
            _watcher.Created += (_, __) => InvalidateAll();
            _watcher.Deleted += (_, __) => InvalidateAll();
            _watcher.Renamed += (_, __) => InvalidateAll();
            _watcher.EnableRaisingEvents = true;
        }

        private static void InvalidateAll()
        {
            foreach (var key in _cache.Keys.ToArray())
                _cache.TryRemove(key, out _);
        }
    }
}
