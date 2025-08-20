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
    /// Provides stable localization keys for <see cref="ResourceWorkingStatus"/>.
    /// </summary>
    public static class ResourceWorkingStatusTextKey
    {
        public static string Key(ResourceWorkingStatus status)
        {
            switch (status)
            {
                case ResourceWorkingStatus.Working: return "enums.resource_working_status.working";
                case ResourceWorkingStatus.Waiting: return "enums.resource_working_status.waiting";
                case ResourceWorkingStatus.Setup: return "enums.resource_working_status.setup";
                case ResourceWorkingStatus.Interruption: return "enums.resource_working_status.interruption";
                case ResourceWorkingStatus.Fault: return "enums.resource_working_status.fault";
                case ResourceWorkingStatus.Maintenance: return "enums.resource_working_status.maintenance";
                default: return "enums.resource_working_status.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="ResourceWorkingStatus"/> using an <c>IText</c> provider.
    /// </summary>
    public static class ResourceWorkingStatusTextExtensions
    {
        public static string ToText(this ResourceWorkingStatus status, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[ResourceWorkingStatusTextKey.Key(status)];
        }
    }
}
