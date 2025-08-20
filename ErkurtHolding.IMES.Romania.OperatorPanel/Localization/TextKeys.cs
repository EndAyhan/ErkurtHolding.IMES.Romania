namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>Stable keys (use dotted paths in JSON).</summary>
    public static class TextKeys
    {
        // Errors
        public const string Error_InvalidInput = "error.invalid_input";
        public const string Error_NotFound = "error.not_found";               // "Item not found: {id}"
        public const string Error_Unhandled = "error.unhandled";               // "Unexpected error"

        // Info / labels
        public const string Info_Saved = "info.saved";                    // "Saved successfully"
        public const string Label_TotalCount = "label.total_count";             // "{count} item(s)"

        // Domain-specific samples
        public const string Order_Status_Pending = "order.status.pending";
        public const string Order_Status_Approved = "order.status.approved";
        public const string Order_Status_Rejected = "order.status.rejected";
    }
}
