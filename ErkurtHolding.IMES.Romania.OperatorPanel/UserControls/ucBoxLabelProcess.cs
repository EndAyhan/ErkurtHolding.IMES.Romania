using DevExpress.Data;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.GridModels;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    /// <summary>
    /// Process labeling UI:
    /// - Lists handling units (grouped by barcode for process flow)
    /// - Lists production details and scrap details for the active orders
    /// - Allows printing labels (designer/preview/print)
    ///
    /// Perf notes:
    /// - Prebuild dictionaries/lists to avoid repeated LINQ scans in hot paths
    /// - Guard against null/empty collections
    /// - Keep LINQ allocations out of loops where possible
    /// </summary>
    public partial class ucBoxLabelProcess : DevExpress.XtraEditors.XtraUserControl
    {
        // --- selection state ---
        private List<int> handlingUnitSelectedRows = new List<int>();
        private List<int> productionDetailSelectedRows = new List<int>();
        private List<int> scrapProductionDetailSelectedRows = new List<int>();

        // --- data caches (populated once and reused) ---
        private List<HandlingUnit> handlingUnits = null;
        private Dictionary<Guid, HandlingUnit> _handlingById = new Dictionary<Guid, HandlingUnit>();
        private Dictionary<Guid, vw_ShopOrderGridModel> _ordersById = new Dictionary<Guid, vw_ShopOrderGridModel>();

        private List<ProcessHandlingUnitModel> processHandlingUnitModels = new List<ProcessHandlingUnitModel>();
        private BindingList<ProductionDetailGridModel> productionGridModels;

        private UserModel userModel { get; set; }
        private GridLookUpEditBestPopupFormSizeHelper _gluePopupSizer;

        private string ucLabelselectedPage = "Box";

        public ucBoxLabelProcess(UserModel model)
        {
            InitializeComponent();

            // Localize UI
            LanguageHelper.InitializeLanguage(this);

            // Load previously created HUs for all active WorkOrders
            var orderIds = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.Select(x => x.Id).ToList();
            handlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(orderIds);
            if (handlingUnits == null) handlingUnits = new List<HandlingUnit>();

            // Build a fast lookup by Id once
            _handlingById = handlingUnits.GroupBy(h => h.Id).ToDictionary(g => g.Key, g => g.First());

            // Cache orders by Id for quick access
            _ordersById = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels
                .GroupBy(x => x.Id).ToDictionary(g => g.Key, g => g.First());

            userModel = model;

            // Improve popup size UX for the order selector
            _gluePopupSizer = new GridLookUpEditBestPopupFormSizeHelper(glueShopOrderOperation, 200, 450);

            // Initial loads
            InitData();
            InitDataBox();
            InitDataProductionDetails();
            InitDataScrapProductionDetails();

            // Show "Open Designer" only for role 4 users
            chkReportDesignerOpen.Visible = ToolsMdiManager.frmOperatorActive.Users.Any(u => u.Role == 4);
        }

        #region INIT DATA

        /// <summary>Bind the order selector; default to most recently started.</summary>
        private void InitData()
        {
            var orders = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
            glueShopOrderOperation.Properties.DataSource = orders;

            var last = orders.OrderByDescending(x => x.opStartDate).FirstOrDefault();
            if (last != null)
                glueShopOrderOperation.EditValue = last.Id;
        }

        /// <summary>
        /// Build the grouped HandlingUnit view for process mode:
        /// - Group by barcode
        /// - Sum quantities, count lots, pick earliest CreatedAt
        /// </summary>
        private void InitDataBox()
        {
            handlingUnitSelectedRows.Clear();
            processHandlingUnitModels = new List<ProcessHandlingUnitModel>();

            List<HandlingUnit> filteredHandlingUnits;

            if (chkHideOtherShifts.Checked)
            {
                // Limit to the active production id, then re-include by common barcode
                var currentProdId = ToolsMdiManager.frmOperatorActive.shopOrderProduction != null
                    ? ToolsMdiManager.frmOperatorActive.shopOrderProduction.Id
                    : Guid.Empty;

                var sameProductionHUs = handlingUnits.Where(x => x.ShopOrderProductionID == currentProdId).ToList();
                var sharedBarcodes = new HashSet<string>(sameProductionHUs.Select(y => y.Barcode));
                filteredHandlingUnits = handlingUnits.Where(x => sharedBarcodes.Contains(x.Barcode)).ToList();
            }
            else
            {
                filteredHandlingUnits = handlingUnits;
            }

            if (!filteredHandlingUnits.HasEntries())
            {
                gcHandlingUnitMain.DataSource = processHandlingUnitModels;
                return;
            }

            // group by Barcode and aggregate
            var grouped = filteredHandlingUnits
                .GroupBy(h => h.Barcode)
                .Select(grp =>
                {
                    var first = grp.OrderBy(o => o.CreatedAt).First();
                    var model = new ProcessHandlingUnitModel
                    {
                        BoxBarcode = grp.Key,
                        Quantity = grp.Sum(q => q.Quantity),
                        LotCount = grp.Count(),
                        CreatedAt = first.CreatedAt
                    };
                    return model;
                })
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            processHandlingUnitModels.AddRange(grouped);
            gcHandlingUnitMain.DataSource = processHandlingUnitModels;
        }

        /// <summary>
        /// Build the "good" production list. We avoid repeated First()/Where() scans by:
        /// - caching orders in _ordersById
        /// - using _handlingById to map BoxID→Barcode quickly
        /// </summary>
        private void InitDataProductionDetails()
        {
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var all = ToolsMdiManager.frmOperatorActive.productionDetails;
            if (all == null || all.Count == 0)
            {
                gridControlProductionDetail.DataSource = productionGridModels;
                return;
            }

            var list = all.Where(p => !p.ByProduct && p.ProductionStateID == StaticValues.specialCodeOk.Id).ToList();
            if (chkHideOtherShifts.Checked && StaticValues.shift != null)
                list = list.Where(x => x.ShiftId == StaticValues.shift.Id).ToList();

            foreach (var item in list)
            {
                try
                {
                    vw_ShopOrderGridModel order;
                    if (!_ordersById.TryGetValue(item.ShopOrderOperationID, out order))
                        continue;

                    var grid = new ProductionDetailGridModel
                    {
                        Id = item.Id,
                        ShopOrderOperationID = item.ShopOrderOperationID,
                        ShopOrderProductionID = item.ShopOrderProductionID,
                        ProductID = item.ProductID,
                        OrderNo = order.orderNo,
                        OperationNo = order.operationNo.ToString(),
                        Barcode = item.Barcode,
                        Serial = item.serial.ToString(),
                        CreateAt = item.CreatedAt,
                        ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName,
                        Quantity = item.Quantity,
                        ManualInput = item.ManualInput,
                        PartHandlingBoxBarcode = GetBoxBarcodeOrEmpty(item.BoxID)
                    };

                    productionGridModels.Add(grid);
                }
                catch
                {
                    // Non-fatal row error; skip
                }
            }

            gridControlProductionDetail.DataSource = productionGridModels;
        }

        /// <summary>Build the "scrap" production list (same approach as good list).</summary>
        private void InitDataScrapProductionDetails()
        {
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var all = ToolsMdiManager.frmOperatorActive.productionDetails;
            if (all == null || all.Count == 0)
            {
                gcScrapProducts.DataSource = productionGridModels;
                return;
            }

            var list = all.Where(p => !p.ByProduct && p.ProductionStateID == StaticValues.specialCodeScrap.Id).ToList();

            foreach (var item in list)
            {
                try
                {
                    vw_ShopOrderGridModel order;
                    if (!_ordersById.TryGetValue(item.ShopOrderOperationID, out order))
                        continue;

                    var grid = new ProductionDetailGridModel
                    {
                        Id = item.Id,
                        ShopOrderOperationID = item.ShopOrderOperationID,
                        ShopOrderProductionID = item.ShopOrderProductionID,
                        ProductID = item.ProductID,
                        OrderNo = order.orderNo,
                        OperationNo = order.operationNo.ToString(),
                        Barcode = item.Barcode,
                        Serial = item.serial.ToString(),
                        CreateAt = item.CreatedAt,
                        ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName,
                        Quantity = item.Quantity,
                        ManualInput = item.ManualInput,
                        PartHandlingBoxBarcode = GetBoxBarcodeOrEmpty(item.BoxID)
                    };

                    productionGridModels.Add(grid);
                }
                catch
                {
                    // Non-fatal row error; skip
                }
            }

            gcScrapProducts.DataSource = productionGridModels;
        }

        /// <summary>Get HU barcode fast, or empty string if BoxID = Guid.Empty or not found.</summary>
        private string GetBoxBarcodeOrEmpty(Guid boxId)
        {
            if (boxId == Guid.Empty) return string.Empty;
            HandlingUnit hu;
            if (_handlingById.TryGetValue(boxId, out hu) && hu != null)
                return hu.Barcode ?? string.Empty;
            return string.Empty;
        }

        #endregion

        #region BOX GRID VIEW SELECTION CHANGED

        private void gvHandlingUnitMain_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            handlingUnitSelectedRows = gvHandlingUnitMain.GetSelectedRows().ToList();
        }

        #endregion

        #region XTRA TAB CONTROL SELECTED CHANGED

        private void xtcLabelBox_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int idx = xtcLabelBox.TabPages.IndexOf(e.Page);
            if (idx == 0) ucLabelselectedPage = "Box";
            else if (idx == 1) ucLabelselectedPage = "ProductionDetails";
            else if (idx == 2) ucLabelselectedPage = "BoxProductDetails";
            else if (idx == 3) ucLabelselectedPage = "ScrapProductionDetails";
        }

        #endregion

        #region CHKHIDE OTHER SHIFTS CHECKED CHANGED

        private void chkHideOtherShifts_CheckedChanged(object sender, EventArgs e)
        {
            InitDataBox();
            InitDataProductionDetails();
        }

        #endregion

        #region MENU BUTTON CLICK EVENT

        /// <summary>Close the container.</summary>
        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        /// <summary>
        /// Print handler for all tabs.
        /// Honors:
        /// - chkPrintView (preview)
        /// - chkReportDesignerOpen (open designer)
        /// - chkDesignCheck (select external .repx)
        /// </summary>
        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (userModel == null)
                    GetUserModel();

                if (ucLabelselectedPage == "Box")
                {
                    if (handlingUnitSelectedRows.Count > 0)
                    {
                        // Print each selected HU group
                        for (int i = 0; i < handlingUnitSelectedRows.Count; i++)
                        {
                            var handling = (ProcessHandlingUnitModel)gvHandlingUnitMain.GetRow(handlingUnitSelectedRows[i]);
                            if (handling == null) continue;

                            var handlings = handlingUnits.Where(x => x.Barcode == handling.BoxBarcode).ToList();
                            if (handlings.Count == 0) continue;

                            // Build the report helper with all pieces for this barcode group
                            var report = new ReportProcessHandlingUnitHelper
                            {
                                product = ToolsMdiManager.frmOperatorActive.products.Count > 0
                                    ? ToolsMdiManager.frmOperatorActive.products[0]
                                    : null,
                                machine = ToolsMdiManager.frmOperatorActive.machine,
                                resource = ToolsMdiManager.frmOperatorActive.resource,
                                shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction,
                                user = userModel
                            };

                            // Fill collections (avoid duplicate scans)
                            foreach (var item in handlings)
                            {
                                var so = ShopOrderOperationManager.Current.GetShopOrderOperationById(item.ShopOrderID);
                                if (so != null) report.shopOrderOperations.Add(so);

                                vw_ShopOrderGridModel view;
                                if (_ordersById.TryGetValue(item.ShopOrderID, out view) && view != null)
                                    report.vwShopOrderGridModels.Add(view);

                                report.handlingUnits.Add(item);

                                // All production details linked to this HU
                                var details = ToolsMdiManager.frmOperatorActive.productionDetails
                                    .Where(x => x.BoxID == item.Id).ToList();
                                if (details.Count > 0) report.shopOrderProductionDetails.AddRange(details);
                            }

                            // Choose label model for ProductId
                            if (report.product == null)
                                throw new Exception(MessageTextHelper.GetMessageText("UCBL", "107", "Ürün Bulunamadı", "Message")); // fallback; no product configured

                            report.printLabelModel = ToolsMdiManager.frmOperatorActive.printLabelModels
                                .First(p => p.ProductId == report.product.Id && p.productionLabelType == ProductionLabelType.Box);

                            // Execute chosen action
                            if (chkPrintView.Checked)
                                report.PrintBarcodeView();
                            else if (chkReportDesignerOpen.Checked)
                                report.PrintBarcodeDesigner();
                            else if (chkDesignCheck.Checked)
                            {
                                using (var openFileDialog = new OpenFileDialog())
                                {
                                    openFileDialog.Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message");
                                    openFileDialog.RestoreDirectory = true;
                                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        report.PrintBarcodeSelectFileDesigner(openFileDialog.FileName);
                                    }
                                }
                            }
                            else
                            {
                                report.PrintLabel();
                            }
                        }
                    }
                    else
                    {
                        // "No selection" info
                        ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                    }
                }
                else if (ucLabelselectedPage == "ProductionDetails")
                {
                    if (productionDetailSelectedRows.Count > 0)
                    {
                        for (int i = 0; i < productionDetailSelectedRows.Count; i++)
                        {
                            var productRow = (ProductionDetailGridModel)gridViewProductionDetail.GetRow(productionDetailSelectedRows[i]);
                            if (productRow == null) continue;

                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.FirstOrDefault(x => x.Id == productRow.Id);
                            if (pd == null) continue;

                            vw_ShopOrderGridModel order;
                            if (!_ordersById.TryGetValue(pd.ShopOrderOperationID, out order) || order == null)
                                continue;

                            if (order.alan5 == "TRUE")
                            {
                                if (chkDesignCheck.Checked)
                                {
                                    using (var openFileDialog = new OpenFileDialog())
                                    {
                                        openFileDialog.Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message");
                                        openFileDialog.RestoreDirectory = true;
                                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            var myReport = ReportHelper.CreateProductionDetail(userModel, order, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd, false);
                                            myReport.PrintBarcodeDesigner(openFileDialog.FileName);
                                        }
                                    }
                                }
                                else
                                {
                                    var report = ReportHelper.CreateProductionDetail(userModel, order, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd);
                                    if (chkPrintView.Checked)
                                        report.PrintBarcodeView();
                                    if (chkReportDesignerOpen.Checked)
                                        report.PrintBarcodeDesigner();
                                    else
                                        report.PrintLabel();
                                }
                            }
                            else
                            {
                                var prm = productRow.Serial.CreateParameters("@Serial");
                                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "967", "Yazdırmak istediğiniz etiketin (@Serial) iş emri, ürün etiketi vermiyor", "Message"), prm);
                            }
                        }
                    }
                    else
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                    }
                }
                else if (ucLabelselectedPage == "BoxProductDetails")
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                }
                else if (ucLabelselectedPage == "ScrapProductionDetails")
                {
                    if (scrapProductionDetailSelectedRows.Count > 0)
                    {
                        for (int i = 0; i < scrapProductionDetailSelectedRows.Count; i++)
                        {
                            var gm = (ProductionDetailGridModel)gvScrapProducts.GetRow(scrapProductionDetailSelectedRows[i]);
                            if (gm == null) continue;

                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.FirstOrDefault(x => x.Id == gm.Id);
                            if (pd == null) continue;

                            var product = ToolsMdiManager.frmOperatorActive.products.SingleOrDefault(x => x.Id == pd.ProductID);
                            vw_ShopOrderGridModel order;
                            if (product == null || !_ordersById.TryGetValue(pd.ShopOrderOperationID, out order) || order == null)
                                continue;

                            var report = new ReportProductHelper
                            {
                                product = product,
                                shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction,
                                shopOrderProductionDetail = pd,
                                machine = ToolsMdiManager.frmOperatorActive.machine,
                                resource = ToolsMdiManager.frmOperatorActive.resource,
                                userModel = userModel,
                                shopOrderOperation = order,
                                printLabelModel = new PrintLabelModel
                                {
                                    LabelDesingFilePath = StaticValues.ScrapProductDesignPath,
                                    PrintCopyCount = 1,
                                    printerName = StaticValues.ScrapPrinterName,
                                    productionLabelType = ProductionLabelType.Product
                                }
                            };

                            if (chkDesignCheck.Checked)
                            {
                                using (var openFileDialog = new OpenFileDialog())
                                {
                                    openFileDialog.Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message");
                                    openFileDialog.RestoreDirectory = true;
                                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        report.PrintBarcodeDesigner(openFileDialog.FileName);
                                    }
                                }
                            }
                            else
                            {
                                if (chkPrintView.Checked)
                                    report.PrintBarcodeView();
                                if (chkReportDesignerOpen.Checked)
                                    report.PrintBarcodeDesigner();
                                else
                                    report.PrintLabel(true);
                            }
                        }
                    }
                    else
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != null && ex.Message.IndexOf("File not found", StringComparison.OrdinalIgnoreCase) >= 0)
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "959", "Etiket Dosya yolu bulunamadı", "Message"));
                else
                    ToolsMessageBox.Error(this, ex);
            }
        }

        #endregion

        #region GET USER MODEL

        /// <summary>Ensures a privileged user is logged in before printing.</summary>
        private void GetUserModel()
        {
            var frm = new FrmUserLogin(false);
            if (frm.ShowDialog() == DialogResult.OK)
                userModel = frm.userModel;
            else
                throw new Exception(MessageTextHelper.GetMessageText("000", "629", "Yetkili kullanıcı girişi yapmadan etiket alamazsınız", "Message"));
        }

        #endregion

        private void gridViewProductionDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productionDetailSelectedRows = gridViewProductionDetail.GetSelectedRows().ToList();
        }

        private void gvScrapProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            scrapProductionDetailSelectedRows = gvScrapProducts.GetSelectedRows().ToList();
        }

        /// <summary>Row highlighting for half-filled cases.</summary>
        private void gvHandlingUnitMain_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                var handling = (ProcessHandlingUnitModel)gvHandlingUnitMain.GetRow(e.RowHandle);
                if (handling != null)
                {
                    var active = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive;
                    if (active != null && handling.Quantity < (decimal)active.MaxQuantityCapacity)
                        e.Appearance.BackColor = Color.LightBlue;
                }
            }
        }
    }
}