using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents the current working status of a resource.
    /// </summary>
    public enum ResourceWorkingStatus
    {
        Working = 1,
        Waiting = 2,
        Setup = 3,
        Interruption = 4,
        Fault = 5,
        Maintenance = 6
    }

    /// <summary>
    /// Helper to render <see cref="ResourceWorkingStatus"/> using an <c>IText</c> provider.
    /// </summary>
    public static class ResourceWorkingStatusTextExtensions
    {
        public static string ToText(this ResourceWorkingStatus status)
        {
            switch (status)
            {
                case ResourceWorkingStatus.Working: return MessageTextHelper.GetMessageText("ENUM", "283", "Working", "Enum");
                case ResourceWorkingStatus.Waiting: return MessageTextHelper.GetMessageText("ENUM", "284", "Waiting", "Enum");
                case ResourceWorkingStatus.Setup: return MessageTextHelper.GetMessageText("ENUM", "285", "Setup", "Enum");
                case ResourceWorkingStatus.Interruption: return MessageTextHelper.GetMessageText("ENUM", "286", "Interruption", "Enum");
                case ResourceWorkingStatus.Fault: return MessageTextHelper.GetMessageText("ENUM", "287", "Fault", "Enum");
                case ResourceWorkingStatus.Maintenance: return MessageTextHelper.GetMessageText("ENUM", "288", "Maintenance", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "289", "Unknown Status", "Enum");
            }
        }
    }
}
