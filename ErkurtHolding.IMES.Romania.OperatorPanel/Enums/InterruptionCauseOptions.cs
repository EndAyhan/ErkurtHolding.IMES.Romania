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
    /// Helper to render <see cref="InterruptionCauseOptions"/> using an <c>IText</c> provider.
    /// </summary>
    public static class InterruptionCauseOptionsTextExtensions
    {
        public static string ToText(this InterruptionCauseOptions option)
        {
            switch (option)
            {
                case InterruptionCauseOptions.Start: return MessageTextHelper.GetMessageText("ENUM", "147", "Start Interruption", "Enum");
                case InterruptionCauseOptions.End: return MessageTextHelper.GetMessageText("ENUM", "148", "End Interruption", "Enum");
                case InterruptionCauseOptions.Default: return MessageTextHelper.GetMessageText("ENUM", "149", "Default", "Enum");
                case InterruptionCauseOptions.Waiting: return MessageTextHelper.GetMessageText("ENUM", "150", "Waiting", "Enum");
                case InterruptionCauseOptions.AutoMaintenance: return MessageTextHelper.GetMessageText("ENUM", "151", "Autonomous Maintenance", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "152", "Unknown Option", "Enum");
            }
        }
    }
}
