using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents the priority of a call request.
    /// </summary>
    public enum CallStateType
    {
        /// <summary>Critical priority call.</summary>
        high = 1,

        /// <summary>Important priority call.</summary>
        medium = 2,

        /// <summary>Low priority call.</summary>
        low = 3
    }

    /// <summary>
    /// Convenience helpers to render <see cref="CallStateType"/>.
    /// </summary>
    public static class CallStateTypeTextExtensions
    {
        /// <summary>
        /// Gets the localized text for the given <see cref="CallStateType"/>.
        /// </summary>
        public static string ToText(this CallStateType state)
        {
            switch (state)
            {
                case CallStateType.high: return MessageTextHelper.GetMessageText("ENUM", "102", "High", "Enum");
                case CallStateType.medium: return MessageTextHelper.GetMessageText("ENUM", "103", "Medium", "Enum");
                case CallStateType.low: return MessageTextHelper.GetMessageText("ENUM", "104", "Low", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "105", "Unknown", "Enum");
            }
        }
    }
}
