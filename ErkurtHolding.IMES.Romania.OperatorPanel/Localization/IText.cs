namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Resolves localized strings by key with optional named parameters.
    /// </summary>
    public interface IText
    {
        /// <summary>
        /// Gets a localized string for <paramref name="key"/>. 
        /// Use named parameters: _t["error.not_found", ("id", id)].
        /// </summary>
        string this[string key, params (string name, object value)[] args] { get; }

        /// <summary>
        /// Creates a culture override tied to this instance (without mutating globals).
        /// </summary>
        IText For(string languageCode);
    }
}
