using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a call/ticket.
    /// </summary>
    public enum CallStatusType
    {
        /// <summary>Newly created and waiting to be picked up.</summary>
        waiting = 1,

        /// <summary>Assigned and being worked on.</summary>
        inprogress = 2,

        /// <summary>Delivered/handed over to the requester or next stage.</summary>
        delivered = 3,

        /// <summary>Completed/resolved.</summary>
        completed = 4
    }

    /// <summary>
    /// Provides stable localization keys for <see cref="CallStatusType"/>.
    /// </summary>
    public static class CallStatusTypeTextKey
    {
        /// <summary>Returns the localization key for the given <see cref="CallStatusType"/>.</summary>
        public static string Key(CallStatusType status)
        {
            switch (status)
            {
                case CallStatusType.waiting: return "enums.call_status.waiting";
                case CallStatusType.inprogress: return "enums.call_status.inprogress";
                case CallStatusType.delivered: return "enums.call_status.delivered";
                case CallStatusType.completed: return "enums.call_status.completed";
                default: return "enums.call_status.unknown";
            }
        }
    }

    /// <summary>
    /// Helpers to render <see cref="CallStatusType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class CallStatusTypeTextExtensions
    {
        /// <summary>Gets the localized text for the given <see cref="CallStatusType"/>.</summary>
        public static string ToText(this CallStatusType status)
        {
            return StaticValues.T[CallStatusTypeTextKey.Key(status)];
        }
    }
}
