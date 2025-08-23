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
    /// Provides stable localization keys for <see cref="CallStateType"/>.
    /// </summary>
    public static class CallStateTypeTextKey
    {
        /// <summary>
        /// Returns the localization key for the given <see cref="CallStateType"/>.
        /// </summary>
        public static string Key(CallStateType state)
        {
            switch (state)
            {
                case CallStateType.high: return "enums.call_state.high";
                case CallStateType.medium: return "enums.call_state.medium";
                case CallStateType.low: return "enums.call_state.low";
                default: return "enums.call_state.unknown";
            }
        }
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
            return StaticValues.T[CallStateTypeTextKey.Key(state)];
        }
    }
}
