using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.GridModels;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using OtpNet;
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
    /// Box &amp; label management UI:
    /// - Lists Handling Units (boxes), product labels, scrap labels
    /// - Prints labels (preview/designer/direct)
    /// - Creates new labels manually (with optional OTP verification)
    /// - Shows box → product detail relation
    /// </summary>
    public partial class ucBoxLabel : XtraUserControl
    {
        // ------- OTP (session-local cache for current form) -------
        private string currentOtpCode;
        private DateTime otpExpirationTime;

        // ------- Selection caches for grids -------
        private List<int> handlingUnitSelectedRows = new List<int>();
        private List<int> productionDetailSelectedRows = new List<int>();
        private List<int> scrapProductionDetailSelectedRows = new List<int>();

        // ------- UI state -------
        public decimal manualInput;
        private readonly List<vw_ShopOrderGridModel> selectedOrders = new List<vw_ShopOrderGridModel>();
        public vw_ShopOrderGridModel selectedModel = new vw_ShopOrderGridModel();

        // ------- Bindings -------
        private BindingList<HandlingUnitGridModel> handlingUnitGridModels;
        private BindingList<ProductionDetailGridModel> productionGridModels;
        private BindingList<HandlingProductionDetailGridModel> gridModels;

        private string ucLabelselectedPage = "Box";
        private string handlingUnitM2Description = "";
        private string kilogramUnitPartNoDescription = "";
        private string adetUnitPartNoDescription = "";
        private GridLookUpEditBestPopupFormSizeHelper _gluePopupSizer;

        private UserModel userModel { get; set; }

        /// <summary>Localization helper for this control.</summary>
        private static string L(string id, string @default, string desc)
            => MessageTextHelper.GetMessageText("UCBL", id, @default, desc);

        public ucBoxLabel(UserModel model)
        {
            InitializeComponent();
            LanguageHelper.InitializeLanguage(this);

            userModel = model;

            // Better popup sizing for Glue
            _gluePopupSizer = new GridLookUpEditBestPopupFormSizeHelper(glueShopOrderOperation, initialHeight: 200, width: 450);

            InitData();

            // Only admins/designers see the designer checkbox
            chkReportDesignerOpen.Visible = ToolsMdiManager.frmOperatorActive.Users.Any(u => u.Role == 4);

            // Mutually exclusive UX for print/preview options
            chkPrintView.CheckedChanged += (s, e) =>
            {
                if (chkPrintView.Checked)
                {
                    chkReportDesignerOpen.Checked = false;
                    chkReportDesignerOpen.Enabled = false;
                }
                else chkReportDesignerOpen.Enabled = true;
            };
            chkReportDesignerOpen.CheckedChanged += (s, e) =>
            {
                if (chkReportDesignerOpen.Checked)
                {
                    chkPrintView.Checked = false;
                    chkPrintView.Enabled = false;
                }
                else chkPrintView.Enabled = true;
            };
        }

        /// <summary>Programmatically selects a work order.</summary>
        internal void SelectWorkOrder(Guid row)
        {
            glueShopOrderOperation.EditValue = row;
        }

        #region INIT DATA

        /// <summary>Initializes glue data binding and selects the first order when present.</summary>
        private void InitData()
        {
            var list = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
            glueShopOrderOperation.Properties.DataSource = list;

            var first = list.FirstOrDefault();
            if (first != null)
                glueShopOrderOperation.EditValue = first.Id;
        }

        /// <summary>Builds Handling Unit (box) list for selected order with O(n) grouping.</summary>
        private void InitDataBox()
        {
            var act = ToolsMdiManager.frmOperatorActive;

            handlingUnitGridModels = new BindingList<HandlingUnitGridModel>();

            var huForOrder = act.handlingUnits
                .Where(h => h.ShopOrderID == selectedModel.Id)
                .ToList();

            // Precompute quantities per BoxID only once
            var qtyByBox = act.productionDetails
                .Where(x => x.ShopOrderOperationID == selectedModel.Id &&
                            x.ProductionStateID == StaticValues.specialCodeOk.Id &&
                            x.BoxID != Guid.Empty)
                .GroupBy(x => x.BoxID)
                .ToDictionary(g => g.Key, g => (double)g.Sum(x => x.Quantity));

            foreach (var item in huForOrder)
            {
                qtyByBox.TryGetValue(item.Id, out var qty);
                handlingUnitGridModels.Add(new HandlingUnitGridModel
                {
                    Id = item.Id,
                    ShopOrderOperationID = item.ShopOrderID,
                    ShopOrderProductionID = item.ShopOrderProductionID,
                    ProductID = selectedModel.ProductID,
                    OrderNo = selectedModel.orderNo,
                    OperationNo = selectedModel.operationNo.ToString(),
                    Barcode = item.Barcode,
                    Serial = item.Serial.ToString(),
                    CreateAt = item.CreatedAt,
                    Quantity = qty,
                    Description = item.Description,
                    unitMeas = selectedModel.unitMeas,
                });
            }

            gcHandlingUnitMain.DataSource = new RealTimeSource { DataSource = handlingUnitGridModels };
        }

        /// <summary>Builds OK production details for the selected order.</summary>
        private void InitDataProductionDetails()
        {
            var act = ToolsMdiManager.frmOperatorActive;
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var details = act.productionDetails
                .Where(p => p.ShopOrderOperationID == selectedModel.Id &&
                            !p.ByProduct &&
                            p.ProductionStateID == StaticValues.specialCodeOk.Id)
                .ToList();

            if (chkHideOtherShifts.Checked)
                details = details.Where(x => x.ShiftId == StaticValues.shift.Id).ToList();

            // Map HU Id → Barcode (O(1) lookup)
            var huById = act.handlingUnits.ToDictionary(h => h.Id, h => h.Barcode);

            foreach (var d in details)
            {
                var barcode = (d.BoxID == Guid.Empty) ? "" : (huById.TryGetValue(d.BoxID, out var b) ? b : "");
                productionGridModels.Add(new ProductionDetailGridModel
                {
                    Id = d.Id,
                    ShopOrderOperationID = d.ShopOrderOperationID,
                    ShopOrderProductionID = d.ShopOrderProductionID,
                    ProductID = d.ProductID,
                    OrderNo = selectedModel.orderNo,
                    OperationNo = selectedModel.operationNo.ToString(),
                    Barcode = d.Barcode,
                    Serial = d.serial.ToString(),
                    CreateAt = d.CreatedAt,
                    ResourceName = act.resource.resourceName,
                    Quantity = d.Quantity,
                    ManualInput = d.ManualInput,
                    PartHandlingBoxBarcode = barcode
                });
            }

            gridControlProductionDetail.DataSource = productionGridModels;
        }

        /// <summary>Builds SCRAP production details for the selected order.</summary>
        private void InitDataScrapProductionDetails()
        {
            var act = ToolsMdiManager.frmOperatorActive;
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var details = act.productionDetails
                .Where(p => p.ShopOrderOperationID == selectedModel.Id &&
                            !p.ByProduct &&
                            p.ProductionStateID == StaticValues.specialCodeScrap.Id)
                .ToList();

            var huById = act.handlingUnits.ToDictionary(h => h.Id, h => h.Barcode);

            foreach (var d in details)
            {
                var barcode = (d.BoxID == Guid.Empty) ? "" : (huById.TryGetValue(d.BoxID, out var b) ? b : "");
                productionGridModels.Add(new ProductionDetailGridModel
                {
                    Id = d.Id,
                    ShopOrderOperationID = d.ShopOrderOperationID,
                    ShopOrderProductionID = d.ShopOrderProductionID,
                    ProductID = d.ProductID,
                    OrderNo = selectedModel.orderNo,
                    OperationNo = selectedModel.operationNo.ToString(),
                    Barcode = d.Barcode,
                    Serial = d.serial.ToString(),
                    CreateAt = d.CreatedAt,
                    ResourceName = act.resource.resourceName,
                    Quantity = d.Quantity,
                    ManualInput = d.ManualInput,
                    PartHandlingBoxBarcode = barcode
                });
            }

            gcScrapProducts.DataSource = productionGridModels;
        }

        /// <summary>Builds “Box → Product detail” master-detail using pre-grouped data for O(n) binding.</summary>
        private void InitDataBoxProductDetails()
        {
            gridModels = new BindingList<HandlingProductionDetailGridModel>();

            // Group once by PartHandlingBoxBarcode
            var byBox = (gridControlProductionDetail.DataSource as BindingList<ProductionDetailGridModel>)
                ?.GroupBy(x => x.PartHandlingBoxBarcode ?? "")
                .ToDictionary(g => g.Key, g => g.ToList())
                ?? new Dictionary<string, List<ProductionDetailGridModel>>();

            foreach (var hu in handlingUnitGridModels)
            {
                byBox.TryGetValue(hu.Barcode ?? "", out var items);
                gridModels.Add(new HandlingProductionDetailGridModel
                {
                    Id = hu.Id,
                    ShopOrderOperationID = hu.ShopOrderOperationID,
                    ShopOrderProductionID = hu.ShopOrderProductionID,
                    ProductID = hu.ProductID,
                    OrderNo = hu.OrderNo,
                    OperationNo = hu.OperationNo,
                    Barcode = hu.Barcode,
                    Serial = hu.Serial,
                    Quantity = hu.Quantity,
                    CreateAt = hu.CreateAt,
                    ProductionDetailGridModels = items ?? new List<ProductionDetailGridModel>()
                });
            }

            gcProductionHandlingUnits.DataSource = new RealTimeSource { DataSource = gridModels };
        }

        #endregion

        #region BAR BUTTON CLICK EVENT

        /// <summary>Print command: supports Box / Product / Scrap with preview, designer, or direct print.</summary>
        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked)
                {
                    if (!GetManuelUserModel())
                        return;
                }

                if (userModel == null)
                    GetUserModel();

                if (ucLabelselectedPage == "Box")
                {
                    if (handlingUnitSelectedRows.Count > 0)
                    {
                        foreach (var rowHandle in handlingUnitSelectedRows)
                        {
                            if (!gvHandlingUnitMain.IsDataRow(rowHandle)) continue;

                            var id = (Guid)gvHandlingUnitMain.GetRowCellValue(rowHandle, nameof(HandlingUnitGridModel.Id));
                            var h = ToolsMdiManager.frmOperatorActive.handlingUnits.First(x => x.Id == id);
                            var vwShopOrderGridModel = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(v => v.Id == h.ShopOrderID);

                            var report = ReportHandlingUnitAdetHelper.CreatePartHandlingUnit(vwShopOrderGridModel, userModel, h);

                            if (chkPrintView.Checked) report.PrintBarcodeView();
                            else if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                            else if (chkDesignCheck.Checked)
                            {
                                using (var ofd = new OpenFileDialog
                                {
                                    Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message"),
                                    RestoreDirectory = true
                                })
                                {
                                    if (ofd.ShowDialog() == DialogResult.OK)
                                    {
                                        var myReport = ReportHandlingUnitAdetHelper.CreatePartHandlingUnit(vwShopOrderGridModel, userModel, h, false);
                                        myReport.PrintBarcodeSelectFileDesigner(ofd.FileName);
                                    }
                                }
                            }
                            else report.PrintLabel();
                        }
                    }
                    else
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                    }
                }
                else if (ucLabelselectedPage == "ProductionDetails")
                {
                    if (productionDetailSelectedRows.Count > 0)
                    {
                        foreach (var rowHandle in productionDetailSelectedRows)
                        {
                            if (!gridViewProductionDetail.IsDataRow(rowHandle)) continue;

                            var id = (Guid)gridViewProductionDetail.GetRowCellValue(rowHandle, nameof(ProductionDetailGridModel.Id));
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == id);

                            if (chkDesignCheck.Checked)
                            {
                                using (var ofd = new OpenFileDialog
                                {
                                    Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message"),
                                    RestoreDirectory = true
                                })
                                {
                                    if (ofd.ShowDialog() == DialogResult.OK)
                                    {
                                        var myReport = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd, false);
                                        myReport.PrintBarcodeDesigner(ofd.FileName);
                                    }
                                }
                            }
                            else
                            {
                                var report = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd);
                                if (chkPrintView.Checked) report.PrintBarcodeView();
                                if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                                else report.PrintLabel();
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
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "958", "Bu ekranda etiketi cıktısı alamazsınız", "Message"));
                }
                else if (ucLabelselectedPage == "ScrapProductionDetails")
                {
                    if (scrapProductionDetailSelectedRows.Count > 0)
                    {
                        foreach (var rowHandle in scrapProductionDetailSelectedRows)
                        {
                            if (!gvScrapProducts.IsDataRow(rowHandle)) continue;

                            var id = (Guid)gvScrapProducts.GetRowCellValue(rowHandle, nameof(ProductionDetailGridModel.Id));
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == id);
                            var product = ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == pd.ProductID);

                            var report = new ReportProductHelper
                            {
                                product = product,
                                shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction,
                                shopOrderProductionDetail = pd,
                                machine = ToolsMdiManager.frmOperatorActive.machine,
                                resource = ToolsMdiManager.frmOperatorActive.resource,
                                userModel = userModel,
                                shopOrderOperation = selectedModel,
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
                                using (var ofd = new OpenFileDialog
                                {
                                    Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message"),
                                    RestoreDirectory = true
                                })
                                {
                                    if (ofd.ShowDialog() == DialogResult.OK)
                                        report.PrintBarcodeDesigner(ofd.FileName);
                                }
                            }
                            else
                            {
                                if (chkPrintView.Checked) report.PrintBarcodeView();
                                if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                                else report.PrintLabel(true);
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
                if (ex.Message.Contains("File not found"))
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "959", "Etiket Dosya yolu bulunamadı", "Message"));
                else
                    ToolsMessageBox.Error(this, ex);
            }
        }

        /// <summary>Creates a new label (manual path). Supports AD / m2 / kg flows.</summary>
        private void batBtnNewLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (userModel == null)
                    GetUserModel();

                if (ucLabelselectedPage == "Box")
                {
                    if (!OperatorPanelConfigurationHelper.CanManuallyCreateShopOrderProductionDetail(selectedModel))
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "962", "Bu iş emrinde manuel olarak yeni kasa etiketi oluşturulamaz", "Message"));
                        return;
                    }

                    if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked && !GetManuelUserModel())
                        return;

                    var frm = new FrmNumericKeyboard(selectedModel);
                    if (frm.ShowDialog() != DialogResult.OK) return;

                    if (frm.value <= 0)
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "961", "Girdiğiniz değer sıfır'dan yüksek olmalı", "Message"));
                        return;
                    }

                    if (selectedModel.unitMeas == Units.ad.ToText())
                    {
                        if (userModel == null) GetUserModel();

                        if (ToolsMdiManager.frmOperatorActive.panelDetail.PartiNoControl == true)
                            adetUnitPartNoDescription = CreateInputDialogBox();

                        HandlingUnit halfHandlingUnit = null;

                        // Half-case assist (optional)
                        if (ToolsMdiManager.frmOperatorActive.panelDetail.HalfCaseStatus && handlingUnitSelectedRows.Count > 0)
                        {
                            foreach (var rowHandle in handlingUnitSelectedRows)
                            {
                                if (!gvHandlingUnitMain.IsDataRow(rowHandle)) continue;

                                try
                                {
                                    var id = (Guid)gvHandlingUnitMain.GetRowCellValue(rowHandle, nameof(HandlingUnitGridModel.Id));
                                    var h = ToolsMdiManager.frmOperatorActive.handlingUnits.First(x => x.Id == id);
                                    if (h.Quantity < frm.value)
                                    {
                                        halfHandlingUnit = h;
                                        break;
                                    }
                                }
                                catch { /* ignore */ }
                            }
                        }

                        var report = ReportHandlingUnitAdetHelper.CreateReportHandlingUnit(selectedModel, frm.value, userModel, frm.selectedPartHandlingUnit, halfHandlingUnit, false, adetUnitPartNoDescription);

                        if (chkDesignCheck.Checked)
                        {
                            using (var ofd = new OpenFileDialog
                            {
                                Filter = MessageTextHelper.GetMessageText("000", "101", "Label Design File |*.repx", "Message"),
                                RestoreDirectory = true
                            })
                            {
                                if (ofd.ShowDialog() == DialogResult.OK)
                                    report.PrintBarcodeDesigner(ofd.FileName);
                            }
                        }
                        else
                        {
                            HandlingUnitManager.Current.UpdateCompanyPersonId(report.handlingUnit.Id, userModel.CompanyPersonId);

                            foreach (var sopd in report.shopOrderProductionDetails)
                            {
                                if (ToolsMdiManager.frmOperatorActive.productionDetails.Any(p => p.Id == sopd.Id))
                                    ToolsMdiManager.frmOperatorActive.productionDetails.RemoveAll(pd => pd.Id == sopd.Id);

                                sopd.BoxID = report.handlingUnit.Id;
                                sopd.IfsReported = true;
                                ShopOrderProductionDetailManager.Current.Update(sopd);
                                ToolsMdiManager.frmOperatorActive.productionDetails.Add(sopd);
                            }

                            report.handlingUnit.SendIfs = true;
                            report.handlingUnit.SendDate = DateTime.Now;
                            HandlingUnitManager.Current.Update(report.handlingUnit);

                            ToolsMdiManager.frmOperatorActive.handlingUnits.RemoveAll(h => h.Id == report.handlingUnit.Id);
                            ToolsMdiManager.frmOperatorActive.handlingUnits.Add(report.handlingUnit);

                            if (report.handlingUnit.Quantity < (decimal)frm.selectedPartHandlingUnit.MaxQuantityCapacity)
                            {
                                if (ToolsMdiManager.frmOperatorActive.halfHandlingUnit.ContainsKey(selectedModel.Id))
                                    ToolsMdiManager.frmOperatorActive.halfHandlingUnit.Remove(selectedModel.Id);

                                var handlingUnitModel = new ProcessHandlingUnitModel
                                {
                                    BoxBarcode = report.handlingUnit.Barcode,
                                    Quantity = report.handlingUnit.Quantity,
                                    LotCount = 1,
                                    CreatedAt = report.handlingUnit.CreatedAt
                                };
                                ToolsMdiManager.frmOperatorActive.halfHandlingUnit.Add(selectedModel.Id, handlingUnitModel);

                                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "960", "Kasa dolu olmadığı için otomatik kasa bildirimlerinde kullanılacak", "Message"));
                            }

                            InitDataBox();
                            InitDataProductionDetails();
                            InitDataBoxProductDetails();

                            if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                            else if (chkPrintView.Checked) report.PrintBarcodeView();
                            else if (ToolsMdiManager.frmOperatorActive.panelDetail.BoxFillsUp) report.PrintLabel();
                        }
                    }
                    else if (selectedModel.unitMeas == Units.m2.ToText())
                    {
                        handlingUnitM2Description = CreateInputDialogBox();
                        manualInput = frm.value;

                        ToolsMdiManager.frmOperatorActive._manualInputBySquareMeters = manualInput;
                        ToolsMdiManager.frmOperatorActive._SquareMetersDescription = handlingUnitM2Description;
                        ToolsMdiManager.frmOperatorActive.squareMeterPlcControl = true;
                        ToolsMdiManager.frmOperatorActive.InsertShopOrderProductionDetailM2(DateTime.Now);
                        StaticValues.opcClient.ResetCounter(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdCounterReset);

                        ToolsMdiManager.frmOperatorActive.container.Visible = false;
                    }
                    else if (selectedModel.unitMeas == Units.kg.ToText())
                    {
                        if (ToolsMdiManager.frmOperatorActive.panelDetail.PartiNoControl == true)
                            kilogramUnitPartNoDescription = CreateInputDialogBox();

                        manualInput = frm.value;
                        ToolsMdiManager.frmOperatorActive._manuelInputByKilogram = manualInput;
                        ToolsMdiManager.frmOperatorActive._PartiNoKilogramDescription = kilogramUnitPartNoDescription;
                        ToolsMdiManager.frmOperatorActive.kilogramPlcControl = true;
                        ToolsMdiManager.frmOperatorActive.counter++;
                    }
                    else
                    {
                        if (userModel == null) GetUserModel();

                        var report = ReportHandlingUnitAdetHelper.CreateReportHandlingUnit(selectedModel, frm.value, userModel, frm.selectedPartHandlingUnit, null);
                        HandlingUnitManager.Current.UpdateCompanyPersonId(report.handlingUnit.Id, userModel.CompanyPersonId);

                        if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                        else if (chkPrintView.Checked) report.PrintBarcodeView();
                        else report.PrintLabel();

                        InitDataBox();
                        InitDataProductionDetails();
                        InitDataBoxProductDetails();
                    }
                }
                else if (ucLabelselectedPage == "ProductionDetails")
                {
                    if (!OperatorPanelConfigurationHelper.CanManuallyCreateShopOrderProductionDetail(selectedModel))
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "963", "Bu iş emrinde manuel olarak yeni ürün etiketi oluşturulamaz", "Message"));
                        return;
                    }

                    if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked && !GetManuelUserModel())
                        return;

                    if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowMultipleProductAdding)
                    {
                        var frm = new FrmNumericKeyboard(L("105", "Etiket adeti", "Message"));
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (frm.value <= 0)
                            {
                                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "961", "Girdiğiniz değer sıfır'dan yüksek olmalı", "Message"));
                                return;
                            }

                            if (selectedModel.unitMeas == Units.ad.ToText())
                            {
                                for (int i = 0; i < frm.value; i++)
                                {
                                    var detail = InsertProductionDetail();
                                    PrintProductionDetail(detail);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectedModel.unitMeas == Units.ad.ToText())
                        {
                            var detail = InsertProductionDetail();
                            PrintProductionDetail(detail);
                        }
                    }
                }
                else if (ucLabelselectedPage == "BoxProductDetails")
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "964", "Bu ekranda yeni ürün etiketi oluşturulamaz", "Message"));
                }
                else if (ucLabelselectedPage == "ScrapProductionDetails")
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "965", "Şüpheli ürün girişi için Şüpheli Ürün Bildirimi ekranını kullanınız", "Message"));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("File not found"))
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "959", "Etiket Dosya yolu bulunamadı", "Message"));
                else
                    ToolsMessageBox.Error(this, ex);
            }
        }

        #endregion

        #region OTP / Auth / Helpers

        /// <summary>
        /// Shows manual-label authorization dialog and (if required) performs OTP verification by e-mail.
        /// Returns true on success, false if user cancelled or verification failed.
        /// </summary>
        private bool GetManuelUserModel()
        {
            var frmLogin = new FrmUserLogin(UserLoginAuthorization.manuelLabelAuthorization);
            if (frmLogin.ShowDialog() != DialogResult.OK)
                return false;

            if (frmLogin.userModel.TwoFactorActive && frmLogin.userModel.Email != null)
            {
                // Create fresh OTP if none or expired
                if (ToolsMdiManager.frmOperatorActive.currentOtpCode == null ||
                    DateTime.Now >= ToolsMdiManager.frmOperatorActive.otpExpirationTime)
                {
                    string machineName;
                    ToolsMdiManager.frmOperatorActive.currentOtpCode = GenerateOtp();
                    ToolsMdiManager.frmOperatorActive.otpExpirationTime = DateTime.Now.AddMinutes(5);

                    currentOtpCode = ToolsMdiManager.frmOperatorActive.currentOtpCode;
                    otpExpirationTime = ToolsMdiManager.frmOperatorActive.otpExpirationTime;

                    if (ToolsMdiManager.frmOperatorActive.resource.Definition == ToolsMdiManager.frmOperatorActive.resource.resourceName)
                        machineName = ToolsMdiManager.frmOperatorActive.machine.Definition;
                    else
                        machineName = $"{ToolsMdiManager.frmOperatorActive.resource.Definition} - {ToolsMdiManager.frmOperatorActive.resource.resourceName}";

                    OTPMailHelper.SendOtpEmail(frmLogin.userModel.Email, currentOtpCode, machineName);
                }

                // Prompt until expires or user succeeds/cancels
                while (DateTime.Now < ToolsMdiManager.frmOperatorActive.otpExpirationTime)
                {
                    var title = $"{ToolsMdiManager.frmOperatorActive.resource.resourceName}-" +
                                L("100", "Doğrulama İşlemi", "Message");

                    var prompt = string.Format(
                        L("101", "Sayın {0},\r\nMail adresinize gelen doğrulama kodunu giriniz:", "Message"),
                        frmLogin.userModel.Name);

                    string enteredOtp = OTPMailHelper.ShowInputDialog(title, prompt, ToolsMdiManager.frmOperatorActive.otpExpirationTime);

                    if (string.IsNullOrEmpty(enteredOtp))
                    {
                        ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, L("102", "İşlem iptal edildi.", "Message"));
                        return false;
                    }

                    if (enteredOtp == ToolsMdiManager.frmOperatorActive.currentOtpCode)
                    {
                        ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, L("103", "Doğrulama başarılı!", "Message"));
                        ToolsMdiManager.frmOperatorActive.manuelLabelUser = frmLogin.userModel;
                        return true;
                    }

                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, L("104", "Doğrulama kodu yanlış. Tekrar deneyin.", "Message"));
                }

                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, L("106", "Zaman aşımına uğradınız.", "Message"));
                ToolsMdiManager.frmOperatorActive.currentOtpCode = null; // reset when expired
                currentOtpCode = null;
                return false;
            }

            ToolsMdiManager.frmOperatorActive.manuelLabelUser = frmLogin.userModel;
            return true;
        }

        /// <summary>Generates a 5-minute TOTP (hard-coded secret here; consider moving to config).</summary>
        private string GenerateOtp()
        {
            var secretKey = Base32Encoding.ToBytes("JBSWY3DPEHPK3PXP"); // Consider moving to config + encryption
            var totp = new Totp(secretKey, step: 300);  // 5 minutes
            return totp.ComputeTotp();
        }

        /// <summary>Prompts for a Part/Batch No. and returns value (throws if user cancels).</summary>
        private string CreateInputDialogBox()
        {
            var args = new XtraInputBoxArgs
            {
                Caption = MessageTextHelper.GetMessageText("000", "861", "Parti Numarası", "Message"),
                Prompt = MessageTextHelper.GetMessageText("000", "862", "Lütfen parti numarasını giriniz", "Message"),
                DefaultButtonIndex = 0
            };
            args.Showing += Args_Showing;

            var editor = new TextEdit
            {
                Properties =
                {
                    Appearance =
                    {
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                        ForeColor = Color.Black,
                        Options = { UseFont = true, UseForeColor = true }
                    },
                    AppearanceFocused =
                    {
                        BackColor = Color.PeachPuff,
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                        ForeColor = Color.Black,
                        Options = { UseBackColor = true, UseFont = true, UseForeColor = true }
                    }
                },
                Size = new Size(338, 28)
            };

            args.Editor = editor;
            args.DefaultResponse = "";

            var result = XtraInputBox.Show(args);
            if (result == null)
                throw new Exception(MessageTextHelper.GetMessageText("000", "630", "Operasyon numarası girmeden ürün listesine ulaşamazsınız", "Message"));

            return result.ToString();
        }

        private void Args_Showing(object sender, XtraMessageShowingArgs e)
        {
            // Optionally set dialog icon here
        }

        /// <summary>Opens user login for “print product barcode/process barcode” path; throws if user cancels.</summary>
        private void GetUserModel()
        {
            var frm = new FrmUserLogin(false);
            if (frm.ShowDialog() == DialogResult.OK)
                userModel = frm.userModel;
            else
                throw new Exception(MessageTextHelper.GetMessageText("000", "629", "Yetkili kullanıcı girişi yapmadan etiket alamazsınız", "Message"));
        }

        /// <summary>Creates and inserts a production detail (default qty=1) and refreshes the grid.</summary>
        private ShopOrderProductionDetail InsertProductionDetail(decimal quantity = 1, decimal handlingunitQuantity = 0)
        {
            var detail = ShopOrderProductionDetailHelper.CreateAndInsertProductionDetail(
                null,
                ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == selectedModel.ProductID),
                true,
                userModel,
                ToolsMdiManager.frmOperatorActive.Users.Count,
                quantity,
                handlingunitQuantity);

            ToolsMdiManager.frmOperatorActive.productionDetails.Add(detail);
            InitDataProductionDetails();
            return detail;
        }

        /// <summary>Prints a single production detail according to panel configuration and UI flags.</summary>
        private void PrintProductionDetail(ShopOrderProductionDetail detail)
        {
            if (ToolsMdiManager.frmOperatorActive.panelDetail.PrintProductBarcode ||
                ToolsMdiManager.frmOperatorActive.panelDetail.ProcessBarcode)
            {
                var report = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, detail);

                if (chkPrintView.Checked) report.PrintBarcodeView();
                if (chkReportDesignerOpen.Checked) report.PrintBarcodeDesigner();
                else report.PrintLabel();
            }
        }

        #endregion

        #region UI handlers

        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void glueShopOrderOperation_EditValueChanged(object sender, EventArgs e)
        {
            if (glueShopOrderOperation.EditValue == null) return;

            var id = (Guid)glueShopOrderOperation.EditValue;
            selectedModel = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == id);

            InitDataBox();
            InitDataProductionDetails();
            InitDataBoxProductDetails();
            InitDataScrapProductionDetails();
        }

        private void gvHandlingUnitMain_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            handlingUnitSelectedRows = gvHandlingUnitMain.GetSelectedRows().ToList();
        }

        private void gvProductionDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productionDetailSelectedRows = gridViewProductionDetail.GetSelectedRows().ToList();
        }

        private void gvScrapProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            scrapProductionDetailSelectedRows = gvScrapProducts.GetSelectedRows().ToList();
        }

        private void gvProductionHandlingUnits_MasterRowGetRelationDisplayCaption(object sender, MasterRowGetRelationNameEventArgs e)
        {
            var code = (string)gvProductionHandlingUnits.GetRowCellValue(e.RowHandle, "Barcode");
            if (e.RelationIndex == 0)
                e.RelationName = $"{code}";
        }

        private void xtcLabelBox_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == null) return;

            var idx = xtcLabelBox.TabPages.IndexOf(e.Page);
            if (idx == 0)
                ucLabelselectedPage = "Box";
            else if (idx == 1)
                ucLabelselectedPage = "ProductionDetails";
            else if (idx == 2)
                ucLabelselectedPage = "BoxProductDetails";
            else if (idx == 3)
                ucLabelselectedPage = "ScrapProductionDetails";
        }

        private void chkHideOtherShifts_CheckedChanged(object sender, EventArgs e)
        {
            InitDataProductionDetails();
            InitDataBoxProductDetails();
        }

        private void gvHandlingUnitMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (!ToolsMdiManager.frmOperatorActive.panelDetail.HalfCaseStatus) return;
            if (e.RowHandle < 0) return;

            var qtyObj = gvHandlingUnitMain.GetRowCellValue(e.RowHandle, nameof(HandlingUnitGridModel.Quantity));
            if (qtyObj is double qty && qty < selectedModel.MaxQuantityCapacity)
                e.Appearance.BackColor = Color.LightBlue;
        }

        #endregion
    }
}

