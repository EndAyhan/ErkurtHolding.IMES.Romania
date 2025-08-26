using System.ComponentModel;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Display name attribute that resolves the caption via MessageTextHelper at runtime.
    /// </summary>
    public sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _formId;
        private readonly string _textId;
        private readonly string _description;

        /// <param name="formId">MessageText.FormId (scope), e.g. "GRD".</param>
        /// <param name="textId">MessageText.TextId (unique per key), e.g. "HPD.OrderNo".</param>
        /// <param name="defaultText">Fallback text if not found.</param>
        /// <param name="description">Optional description stored in MessageText.Description.</param>
        public LocalizedDisplayNameAttribute(string formId, string textId, string defaultText, string description = null)
            : base(defaultText)
        {
            _formId = formId;
            _textId = textId;
            _description = string.IsNullOrWhiteSpace(description) ? textId : description;
        }

        public override string DisplayName =>
            MessageTextHelper.GetMessageText(_formId, _textId, base.DisplayName, _description);
    }
}
