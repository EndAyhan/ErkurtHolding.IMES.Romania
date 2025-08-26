using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ReportManagers;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Reports;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helper methods for creating and stopping interruption causes for an operator form.
    /// Encapsulates DB writes (InterruptionCauseDetail, ResourceStatus) and minimal PLC interactions.
    /// </summary>
    public static class InterruptionCauseHelper
    {
        /// <summary>
        /// Creates an interruption on the given <see cref="FrmOperator"/> and appends a row
        /// to its interruption grid model. Also writes the corresponding detail(s) to DB and
        /// marks the resource status as <see cref="ResourceWorkingStatus.Interruption"/>.
        /// </summary>
        /// <param name="frmOperator">The active operator form (target resource/work center context).</param>
        /// <param name="interruption">Selected interruption cause.</param>
        /// <param name="opInterruptionCause">Selected operation-level interruption cause metadata.</param>
        /// <remarks>
        /// Behavior:
        /// - If the shop order status is <see cref="ShopOrderStatus.Start"/>, we add a single
        ///   <see cref="InterruptionCauseDetail"/> without tying it to a production.
        /// - Otherwise, we create a detail row per selected shop order operation and link it to the
        ///   current <see cref="ShopOrderProduction"/>.
        /// - The resource status is set to "Interruption".
        /// </remarks>
        public static void CreateInterruption(FrmOperator frmOperator, InterruptionCause interruption, OpInterruptionCause opInterruptionCause)
        {
            if (frmOperator == null) throw new ArgumentNullException(nameof(frmOperator));
            if (interruption == null) throw new ArgumentNullException(nameof(interruption));
            if (opInterruptionCause == null) throw new ArgumentNullException(nameof(opInterruptionCause));
            if (frmOperator.resource == null) throw new InvalidOperationException("Operator resource is not initialized.");

            // Keep references on the form (UI state)
            frmOperator.interruptionCause = interruption;
            frmOperator.opInterruptionCause = opInterruptionCause;

            // Build grid model for UI
            vw_InterruptionCauseGridModel causeGridModel = interruption.Casting<vw_InterruptionCauseGridModel>();
            causeGridModel.ResourceID = frmOperator.resource.Id;
            causeGridModel.CauseDescription = opInterruptionCause.description;
            causeGridModel.operation_cause_alan1 = opInterruptionCause.alan1;
            causeGridModel.operation_cause_alan2 = opInterruptionCause.alan2;
            causeGridModel.operation_cause_alan3 = opInterruptionCause.alan3;
            causeGridModel.operation_cause_alan4 = opInterruptionCause.alan4;
            causeGridModel.operation_cause_alan5 = opInterruptionCause.alan5;

            // Persist detail(s)
            if (frmOperator.shopOrderStatus == ShopOrderStatus.Start)
            {
                // Interruption not tied to production yet
                var detail = new InterruptionCauseDetail
                {
                    InterruptionCauseID = interruption.Id,
                    ResourceID = frmOperator.resource.Id
                };
                InterruptionCauseDetailManager.Current.Insert(detail);
            }
            else
            {
                if (frmOperator.shopOrderProduction == null)
                    throw new InvalidOperationException("Shop order production is not initialized.");

                // Create a detail per selected order operation
                foreach (var shopOrderOperation in frmOperator.shopOrderOperations)
                {
                    var detail = new InterruptionCauseDetail
                    {
                        InterruptionCauseID = interruption.Id,
                        ResourceID = frmOperator.resource.Id,
                        ShopOrderOperationID = shopOrderOperation.Id,
                        ShopOrderProductionID = frmOperator.shopOrderProduction.Id
                    };
                    InterruptionCauseDetailManager.Current.Insert(detail);
                }

                // Tie the grid model to the active production (for UI display)
                causeGridModel.ShopOrderProductionID = frmOperator.shopOrderProduction.Id;
            }

            // Reflect immediately in UI grid
            frmOperator.interruptionGridModel.interruptionCauseGridModels.Add(causeGridModel);
            frmOperator.interruptionGridModel.changeData = true;

            // Resource status -> Interruption (best-effort)
            try
            {
                var resourceStatus = new ResourceStatus
                {
                    MachineId = frmOperator.resource.Id,
                    ShopOrderProductionId = frmOperator.shopOrderStatus == ShopOrderStatus.Start
                        ? Guid.Empty
                        : frmOperator.shopOrderProduction.Id,
                    Status = (int)ResourceWorkingStatus.Interruption
                };
                _ = ResourceStatusManager.Current.Insert(resourceStatus);
            }
            catch
            {
                // Intentionally ignored – resource status is non-blocking for UI flow.
            }
        }

        /// <summary>
        /// Stops the current interruption on the form: closes interruption cause, updates
        /// resource status, refreshes the grid, and briefly toggles the PLC "interruption" line if present.
        /// </summary>
        /// <param name="frmOperator">The active operator form.</param>
        /// <param name="userModel">User performing the stop action (currently not used, but kept for future audit extensions).</param>
        public static void StopInterruption(FrmOperator frmOperator, UserModel userModel)
        {
            if (frmOperator == null) throw new ArgumentNullException(nameof(frmOperator));
            if (frmOperator.interruptionCause == null) return; // nothing to stop

            // Close interruption (DB)
            InterruptionCauseManager.Current.UpdateInterruptionCauseEndDate(frmOperator.interruptionCause.Id);

            // Update UI state
            frmOperator.interruptionCauseOptions = InterruptionCauseOptions.End;

            // PLC: machine lock is TRUE only when status == Start (i.e., not producing)
            StaticValues.opcClient.MachineLock(
                frmOperator.panelDetail.OPCNodeIdMachineControl,
                frmOperator.shopOrderStatus == ShopOrderStatus.Start);

            // PLC: gently pulse "interruption" node if configured
            if (!string.IsNullOrEmpty(frmOperator.panelDetail.OPCNodeIdInterruption))
            {
                try { StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdInterruption, true); }
                catch { /* ignore PLC errors */ }
            }

            // Refresh grid and clear resource status
            frmOperator.RefreshInterruptionCauseGrid();

            try
            {
                var productionId = frmOperator.shopOrderStatus == ShopOrderStatus.Start
                    ? Guid.Empty
                    : frmOperator.shopOrderProduction?.Id ?? Guid.Empty;

                ResourceStatusManager.Current.StopResourceStatus(
                    frmOperator.resource.Id, productionId, (int)ResourceWorkingStatus.Interruption);
            }
            catch
            {
                // Non-blocking – UI already reflects the end of interruption.
            }
        }
    }
}
