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
    /// Helpers to render <see cref="CallStatusType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class CallStatusTypeTextExtensions
    {
        /// <summary>Gets the localized text for the given <see cref="CallStatusType"/>.</summary>
        public static string ToText(this CallStatusType status)
        {
            switch (status)
            {
                case CallStatusType.waiting: return MessageTextHelper.GetMessageText("ENUM", "108", "Waiting", "Enum");
                case CallStatusType.inprogress: return MessageTextHelper.GetMessageText("ENUM", "109", "In progress", "Enum");
                case CallStatusType.delivered: return MessageTextHelper.GetMessageText("ENUM", "110", "Delivered", "Enum");
                case CallStatusType.completed: return MessageTextHelper.GetMessageText("ENUM", "111", "Completed", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "112", "Unknown", "Enum");
            }
        }
    }
}
