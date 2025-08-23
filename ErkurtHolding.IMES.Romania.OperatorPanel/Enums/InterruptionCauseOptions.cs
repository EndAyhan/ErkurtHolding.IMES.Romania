using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents interruption cause operations in the operator panel.
    /// </summary>
    public enum InterruptionCauseOptions
    {
        Start = 0,
        End = 1,
        Default = 2,
        Waiting = 3,
        AutoMaintenance = 4
    }

    /// <summary>
    /// Provides stable localization keys for <see cref="InterruptionCauseOptions"/>.
    /// </summary>
    public static class InterruptionCauseOptionsTextKey
    {
        public static string Key(InterruptionCauseOptions option)
        {
            switch (option)
            {
                case InterruptionCauseOptions.Start: return "enums.interruption_cause_options.start";
                case InterruptionCauseOptions.End: return "enums.interruption_cause_options.end";
                case InterruptionCauseOptions.Default: return "enums.interruption_cause_options.default";
                case InterruptionCauseOptions.Waiting: return "enums.interruption_cause_options.waiting";
                case InterruptionCauseOptions.AutoMaintenance: return "enums.interruption_cause_options.auto_maintenance";
                default: return "enums.interruption_cause_options.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="InterruptionCauseOptions"/> using an <c>IText</c> provider.
    /// </summary>
    public static class InterruptionCauseOptionsTextExtensions
    {
        public static string ToText(this InterruptionCauseOptions option)
        {
            return StaticValues.T[InterruptionCauseOptionsTextKey.Key(option)];
        }
    }
}
