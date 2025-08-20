using System;
using System.Linq;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Tools
{
    public static class ToolsRibbonManager
    {
        public static RibbonControl ribbonControl { get; set; }

        // Page & group names
        private const string Page_General = "rbPageGeneral";
        private const string Page_Interruptions = "rbPageInterruptions";
        private const string Group_ShopOrderSettings = "rpgShopOrderSettings";
        private const string Group_OperatorEnterExit = "rpgOperatorEnterExit";
        private const string Group_MachineDown = "rpgMachineDown";
        private const string Group_MachineDownTime = "rpgMachineDownTime";
        private const string Group_PrMaintenance = "rpgPrMaintenance";
        private const string Group_Scale = "rpgScale";
        private const string Group_QrCode = "rpgqrCode";

        public static void RibbonPageStatus(RibbonPage selectedPage)
        {
            if (ribbonControl == null || selectedPage == null) return;

            foreach (RibbonPage page in ribbonControl.Pages)
                page.Visible = string.Equals(selectedPage.Name, page.Name, StringComparison.Ordinal);
        }

        public static void RibbonButtonStatus(
            ShopOrderStatus shopOrderStatus,
            InterruptionCauseOptions interruptionCause,
            MachineDownTimeButtonStatus machineDownTimeButtonStatus,
            PrMaintenanceButtonStatus prMaintenanceButtonStatus)
        {
            RibbonButtonStatus(shopOrderStatus);
            RibbonButtonStatus(interruptionCause);
            RibbonButtonStatus(machineDownTimeButtonStatus);
            RibbonButtonStatus(prMaintenanceButtonStatus);
        }

        public static void RibbonButtonStatus(ShopOrderStatus shopOrderStatus)
        {
            if (ribbonControl == null) return;
            var t = new JsonText();

            var generalPage = ribbonControl.Pages.Cast<RibbonPage>()
                                .FirstOrDefault(p => p.Name == Page_General);
            if (generalPage == null) return;

            bool startVisible = false;

            // Shop order settings
            foreach (var group in generalPage.Groups.Where(g => g.Name == Group_ShopOrderSettings))
            {
                // Defensive: hide known indices if they exist
                SetItemVisible(group, 0, false);
                SetItemVisible(group, 2, false);
                SetItemVisible(group, 3, false);
                SetItemVisible(group, 4, false);
                SetItemVisible(group, 5, false);

                // Show the one that matches current status text
                var text = shopOrderStatus.ToText(t);
                var link = FindByDescription(group, text);
                if (link != null) link.Visible = true;

                // Mirror visibilities (guard indices)
                SetMirror(group, 1, 0); // [1].Visible = [0].Visible
                SetMirror(group, 6, 2); // [6].Visible = [2].Visible

                // Additional condition for [3]
                bool visible2 = GetVisible(group, 2);
                bool allow3 = visible2 && ToolsMdiManager.frmOperatorActive != null
                                        && !ToolsMdiManager.frmOperatorActive.processNewActive;
                SetItemVisible(group, 3, allow3);

                // Start group visible if "start" item was not visible
                startVisible = !GetVisible(group, 0);
            }

            // Operator enter/exit group visibility toggled by "start" state
            foreach (var group in generalPage.Groups.Where(g => g.Name == Group_OperatorEnterExit))
                group.Visible = startVisible;
        }

        public static void RibbonButtonStatus(InterruptionCauseOptions interruptionCause)
        {
            if (ribbonControl == null) return;
            var t = new JsonText();

            foreach (var group in ribbonControl.Pages.Cast<RibbonPage>()
                         .Where(p => p.Name == Page_Interruptions)
                         .SelectMany(p => p.Groups.Where(g => g.Name == Group_MachineDown)))
            {
                SetItemVisible(group, 0, false);
                SetItemVisible(group, 1, false);

                var text = interruptionCause.ToText(t);
                var link = FindByDescription(group, text);
                if (link != null) link.Visible = true;
            }
        }

        public static void RibbonButtonStatus(MachineDownTimeButtonStatus machineDownTimeButtonStatus)
        {
            if (ribbonControl == null) return;
            var t = new JsonText();

            foreach (var group in ribbonControl.Pages.Cast<RibbonPage>()
                         .Where(p => p.Name == Page_Interruptions)
                         .SelectMany(p => p.Groups.Where(g => g.Name == Group_MachineDownTime)))
            {
                // Hide known indices if they exist
                SetItemVisible(group, 0, false);
                SetItemVisible(group, 1, false);
                SetItemVisible(group, 2, false);
                SetItemVisible(group, 3, false);
                SetItemVisible(group, 4, false);

                var text = machineDownTimeButtonStatus.ToText(t);
                var link = FindByDescription(group, text);
                if (link != null) link.Visible = true;

                // [5] visible if neither [0] nor [4] is visible
                bool any = GetVisible(group, 0) || GetVisible(group, 4);
                SetItemVisible(group, 5, !any);
            }
        }

        public static void RibbonButtonStatus(PrMaintenanceButtonStatus prMaintenanceButtonStatus)
        {
            if (ribbonControl == null) return;
            var t = new JsonText();

            foreach (var group in ribbonControl.Pages.Cast<RibbonPage>()
                         .Where(p => p.Name == Page_Interruptions)
                         .SelectMany(p => p.Groups.Where(g => g.Name == Group_PrMaintenance)))
            {
                SetItemVisible(group, 0, false);
                SetItemVisible(group, 1, false);
                SetItemVisible(group, 2, false);

                var text = prMaintenanceButtonStatus.ToText(t);
                var link = FindByDescription(group, text);
                if (link != null) link.Visible = true;
            }
        }

        public static void RibbonButtonScale(bool visible)
        {
            if (ribbonControl == null) return;
            foreach (var group in ribbonControl.Pages.Cast<RibbonPage>()
                         .Where(p => p.Name == Page_General)
                         .SelectMany(p => p.Groups.Where(g => g.Name == Group_Scale)))
            {
                group.Visible = visible;
            }
        }

        public static void RibbonButtonQrCode(bool visible)
        {
            if (ribbonControl == null) return;
            foreach (var group in ribbonControl.Pages.Cast<RibbonPage>()
                         .Where(p => p.Name == Page_General)
                         .SelectMany(p => p.Groups.Where(g => g.Name == Group_QrCode)))
            {
                group.Visible = visible;
            }
        }

        // ---------- helpers ----------

        private static BarItemLink FindByDescription(RibbonPageGroup group, string description)
        {
            if (group == null || group.ItemLinks == null || string.IsNullOrEmpty(description))
                return null;

            foreach (BarItemLink link in group.ItemLinks)
            {
                if (link.Item != null &&
                    string.Equals(link.Item.Description, description, StringComparison.Ordinal))
                {
                    return link;
                }
            }
            return null;
        }

        private static void SetItemVisible(RibbonPageGroup group, int index, bool visible)
        {
            if (group == null || group.ItemLinks == null) return;
            if (index < 0 || index >= group.ItemLinks.Count) return;
            group.ItemLinks[index].Visible = visible;
        }

        private static bool GetVisible(RibbonPageGroup group, int index)
        {
            if (group == null || group.ItemLinks == null) return false;
            if (index < 0 || index >= group.ItemLinks.Count) return false;
            return group.ItemLinks[index].Visible;
        }

        private static void SetMirror(RibbonPageGroup group, int targetIndex, int sourceIndex)
        {
            if (group == null || group.ItemLinks == null) return;
            if (targetIndex < 0 || targetIndex >= group.ItemLinks.Count) return;
            if (sourceIndex < 0 || sourceIndex >= group.ItemLinks.Count) return;
            group.ItemLinks[targetIndex].Visible = group.ItemLinks[sourceIndex].Visible;
        }
    }
}
