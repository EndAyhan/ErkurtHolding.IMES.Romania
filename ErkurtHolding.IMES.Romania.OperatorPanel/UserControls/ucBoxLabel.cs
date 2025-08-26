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
    public partial class ucBoxLabel : DevExpress.XtraEditors.XtraUserControl
    {
        private string currentOtpCode;
        private DateTime otpExpirationTime;
        List<int> handlingUnitSelectedRows = new List<int>();
        List<int> productionDetailSelectedRows = new List<int>();
        List<int> scrapProductionDetailSelectedRows = new List<int>();
        public decimal manualInput;
        List<vw_ShopOrderGridModel> selectedOrders = new List<vw_ShopOrderGridModel>();
        public vw_ShopOrderGridModel selectedModel = new vw_ShopOrderGridModel();
        BindingList<HandlingUnitGridModel> handlingUnitGridModels;
        BindingList<ProductionDetailGridModel> productionGridModels;
        BindingList<HandlingProductionDetailGridModel> gridModels;
        string ucLabelselectedPage = "Box";
        string handlingUnitM2Description = "";
        string kilogramUnitPartNoDescription = "";
        string adetUnitPartNoDescription = "";
        private GridLookUpEditBestPopupFormSizeHelper _gluePopupSizer;

        UserModel userModel { get; set; }
        public ucBoxLabel(UserModel model)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            userModel = model;

            _gluePopupSizer = new GridLookUpEditBestPopupFormSizeHelper(glueShopOrderOperation, initialHeight: 200, width: 450);

            InitData();
            chkReportDesignerOpen.Visible = ToolsMdiManager.frmOperatorActive.Users.Any(u => u.Role == 4);
        }

        internal void SelectWorkOrder(Guid row)
        {
            glueShopOrderOperation.EditValue = row;
        }

        #region INIT DATA
        private void InitData()
        {
            glueShopOrderOperation.Properties.DataSource = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
            glueShopOrderOperation.EditValue = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.FirstOrDefault().Id;
        }

        private void InitDataBox()
        {
            handlingUnitGridModels = new BindingList<HandlingUnitGridModel>();

            var handlingUnitresult = ToolsMdiManager.frmOperatorActive.handlingUnits.Where(h => h.ShopOrderID == selectedModel.Id).ToList();
            foreach (var item in handlingUnitresult)
            {
                handlingUnitGridModels.Add(new HandlingUnitGridModel()
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
                    Quantity = (double)ToolsMdiManager.frmOperatorActive.productionDetails.Where(x => x.ShopOrderOperationID == item.ShopOrderID && x.BoxID == item.Id && x.ProductionStateID == StaticValues.specialCodeOk.Id).Sum(x => x.Quantity),
                    Description = item.Description,
                    unitMeas = selectedModel.unitMeas,
                });
            }
            RealTimeSource rts = new RealTimeSource()
            {
                DataSource = handlingUnitGridModels
            };
            gcHandlingUnitMain.DataSource = rts;
        }

        private void InitDataProductionDetails()
        {
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p => p.ShopOrderOperationID == selectedModel.Id && p.ByProduct == false && p.ProductionStateID == StaticValues.specialCodeOk.Id).ToList();
            if (chkHideOtherShifts.Checked)
                shopOrderProductionDetails = shopOrderProductionDetails.Where(x => x.ShiftId == StaticValues.shift.Id).ToList();

            foreach (var item in shopOrderProductionDetails)
            {
                try
                {
                    ProductionDetailGridModel productionDetail = new ProductionDetailGridModel();
                    productionDetail.Id = item.Id;
                    productionDetail.ShopOrderOperationID = item.ShopOrderOperationID;
                    productionDetail.ShopOrderProductionID = item.ShopOrderProductionID;
                    productionDetail.ProductID = item.ProductID;
                    productionDetail.OrderNo = selectedModel.orderNo;
                    productionDetail.OperationNo = selectedModel.operationNo.ToString();
                    productionDetail.Barcode = item.Barcode;
                    productionDetail.Serial = item.serial.ToString();
                    productionDetail.CreateAt = item.CreatedAt;
                    productionDetail.ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName;
                    productionDetail.Quantity = item.Quantity;
                    productionDetail.ManualInput = item.ManualInput;
                    productionDetail.PartHandlingBoxBarcode = item.BoxID == Guid.Empty ? "" : ToolsMdiManager.frmOperatorActive.handlingUnits.First(p => p.Id == item.BoxID).Barcode;

                    productionGridModels.Add(productionDetail);
                }
                catch (Exception)
                {
                }
            }
            gridControlProductionDetail.DataSource = productionGridModels;
        }

        private void InitDataScrapProductionDetails()
        {
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p => p.ShopOrderOperationID == selectedModel.Id && p.ByProduct == false && p.ProductionStateID == StaticValues.specialCodeScrap.Id).ToList();

            foreach (var item in shopOrderProductionDetails)
            {
                try
                {
                    ProductionDetailGridModel productionDetail = new ProductionDetailGridModel();
                    productionDetail.Id = item.Id;
                    productionDetail.ShopOrderOperationID = item.ShopOrderOperationID;
                    productionDetail.ShopOrderProductionID = item.ShopOrderProductionID;
                    productionDetail.ProductID = item.ProductID;
                    productionDetail.OrderNo = selectedModel.orderNo;
                    productionDetail.OperationNo = selectedModel.operationNo.ToString();
                    productionDetail.Barcode = item.Barcode;
                    productionDetail.Serial = item.serial.ToString();
                    productionDetail.CreateAt = item.CreatedAt;
                    productionDetail.ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName;
                    productionDetail.Quantity = item.Quantity;
                    productionDetail.ManualInput = item.ManualInput;
                    productionDetail.PartHandlingBoxBarcode = item.BoxID == Guid.Empty ? "" : ToolsMdiManager.frmOperatorActive.handlingUnits.First(p => p.Id == item.BoxID).Barcode;

                    productionGridModels.Add(productionDetail);
                }
                catch (Exception)
                {
                }
            }
            gcScrapProducts.DataSource = productionGridModels;
        }

        private void InitDataBoxProductDetails()
        {

            gridModels = new BindingList<HandlingProductionDetailGridModel>();

            foreach (var handlingUnit in handlingUnitGridModels)
            {
                gridModels.Add(new HandlingProductionDetailGridModel()
                {
                    Id = handlingUnit.Id,
                    ShopOrderOperationID = handlingUnit.ShopOrderOperationID,
                    ShopOrderProductionID = handlingUnit.ShopOrderProductionID,
                    ProductID = handlingUnit.ProductID,
                    OrderNo = handlingUnit.OrderNo,
                    OperationNo = handlingUnit.OperationNo,
                    Barcode = handlingUnit.Barcode,
                    Serial = handlingUnit.Serial,
                    Quantity = handlingUnit.Quantity,
                    CreateAt = handlingUnit.CreateAt,
                    ProductionDetailGridModels = productionGridModels.Where(x => x.PartHandlingBoxBarcode == handlingUnit.Barcode).ToList()
                });
            }
            RealTimeSource rts = new RealTimeSource()
            {
                DataSource = gridModels
            };
            gcProductionHandlingUnits.DataSource = rts;
        }

        #endregion

        #region BAR BUTTON CLICK EVENT
        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked)
                {
                    if (!GetManuelUserModel())
                    {
                        return;
                    }
                }
                if (userModel == null)
                    GetUserModel();
                if (ucLabelselectedPage == "Box")
                {
                    if (handlingUnitSelectedRows.Count > 0)
                    {
                        foreach (var selectedRow in handlingUnitSelectedRows)
                        {
                            var proxy = (RealTimeProxyForObject)gvHandlingUnitMain.GetRow(selectedRow);
                            var row = (Guid)proxy.Content.Where(x => x.Key.DisplayName == "Id").FirstOrDefault().Value;
                            var h = ToolsMdiManager.frmOperatorActive.handlingUnits.First(x => x.Id == row);
                            vw_ShopOrderGridModel vwShopOrderGridModel = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(v => v.Id == h.ShopOrderID);

                            var report = ReportHandlingUnitAdetHelper.CreatePartHandlingUnit(vwShopOrderGridModel, userModel, h);

                            if (chkPrintView.Checked)
                                report.PrintBarcodeView();
                            else if (chkReportDesignerOpen.Checked)
                                report.PrintBarcodeDesigner();
                            else if (chkDesignCheck.Checked)//Yeni bir dizayn yapılmak isteniyor ise
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "Etiket Dizayn Dosyası |*.repx";
                                openFileDialog.RestoreDirectory = true;
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string DosyaYolu = openFileDialog.FileName;
                                    var myReport = ReportHandlingUnitAdetHelper.CreatePartHandlingUnit(vwShopOrderGridModel, userModel, h, false);
                                    myReport.PrintBarcodeSelectFileDesigner(DosyaYolu);

                                }
                            }
                            else
                                report.PrintLabel();
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
                        foreach (var selectedRow in productionDetailSelectedRows)
                        {
                            var product = (ProductionDetailGridModel)gridViewProductionDetail.GetRow(selectedRow);
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == product.Id);
                            if (chkDesignCheck.Checked)//Yeni bir dizayn yapılmak isteniyor ise
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "Etiket Dizayn Dosyası |*.repx";
                                openFileDialog.RestoreDirectory = true;
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string DosyaYolu = openFileDialog.FileName;
                                    var myReport = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd, false);
                                    myReport.PrintBarcodeDesigner(DosyaYolu);
                                }
                            }
                            else
                            {
                                var report = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd);

                                if (chkPrintView.Checked)
                                    report.PrintBarcodeView();
                                if (chkReportDesignerOpen.Checked)
                                    report.PrintBarcodeDesigner();
                                else
                                    report.PrintLabel();
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
                        foreach (var selectedRow in scrapProductionDetailSelectedRows)
                        {
                            var productGM = (ProductionDetailGridModel)gvScrapProducts.GetRow(selectedRow);
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == productGM.Id);
                            var product = ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == pd.ProductID);

                            ReportProductHelper report = new ReportProductHelper();
                            report.product = product;
                            report.shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction;
                            report.shopOrderProductionDetail = pd;
                            report.machine = ToolsMdiManager.frmOperatorActive.machine;
                            report.resource = ToolsMdiManager.frmOperatorActive.resource;
                            report.userModel = userModel;
                            report.shopOrderOperation = selectedModel;
                            report.printLabelModel = new PrintLabelModel()
                            {
                                LabelDesingFilePath = StaticValues.ScrapProductDesignPath,
                                PrintCopyCount = 1,
                                printerName = StaticValues.ScrapPrinterName,
                                productionLabelType = ProductionLabelType.Product

                            };

                            if (chkDesignCheck.Checked)//Yeni bir dizayn yapılmak isteniyor ise
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "Etiket Dizayn Dosyası |*.repx";
                                openFileDialog.RestoreDirectory = true;
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string DosyaYolu = openFileDialog.FileName;
                                    report.PrintBarcodeDesigner(DosyaYolu);
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
                if (ex.Message.Contains("File not found"))
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "959", "Etiket Dosya yolu bulunamadı", "Message"));
                else
                    ToolsMessageBox.Error(this, ex);
            }
        }
        private void batBtnNewLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (userModel == null)
                    GetUserModel();
                if (ucLabelselectedPage == "Box")
                {
                    if (OperatorPanelConfigurationHelper.CanManuallyCreateShopOrderProductionDetail(selectedModel))
                    {
                        if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked)
                        {
                            if (!GetManuelUserModel())
                            {
                                return;
                            }
                        }
                        FrmNumericKeyboard frm = new FrmNumericKeyboard(selectedModel);

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (frm.value > 0)
                            {
                                if (selectedModel.unitMeas == Units.ad.ToText())
                                {
                                    //ReportHelper.CreatePartHandlingUnitAd(selectedModel, frm.value);
                                    if (userModel == null)
                                        GetUserModel();

                                    if (ToolsMdiManager.frmOperatorActive.panelDetail.PartiNoControl == true)
                                    {
                                        adetUnitPartNoDescription = CreateInputDialogBox();
                                    }

                                    HandlingUnit halfHandlingUnit = null;
                                    // Check Panel Parameter
                                    if (ToolsMdiManager.frmOperatorActive.panelDetail.HalfCaseStatus)
                                    {
                                        if (handlingUnitSelectedRows.Count > 0)
                                        {
                                            foreach (var selectedRow in handlingUnitSelectedRows)
                                            {
                                                try
                                                {
                                                    var proxy = (RealTimeProxyForObject)gvHandlingUnitMain.GetRow(selectedRow);
                                                    var row = (Guid)proxy.Content.Where(x => x.Key.DisplayName == "Id").FirstOrDefault().Value;
                                                    var h = ToolsMdiManager.frmOperatorActive.handlingUnits.First(x => x.Id == row);
                                                    //var phu = ToolsMdiManager.frmOperatorActive.partHandlingUnits.First(x => x.Id == h.PartHandlingUnitID);
                                                    if (h.Quantity < frm.value)
                                                    {
                                                        halfHandlingUnit = h;
                                                        break;
                                                    }
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }

                                    var report = ReportHandlingUnitAdetHelper.CreateReportHandlingUnit(selectedModel, frm.value, userModel, frm.selectedPartHandlingUnit, halfHandlingUnit, false, adetUnitPartNoDescription);

                                    if (chkDesignCheck.Checked)
                                    {
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Filter = "Etiket Dizayn Dosyası |*.repx";
                                        openFileDialog.RestoreDirectory = true;
                                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            string DosyaYolu = openFileDialog.FileName;
                                            report.PrintBarcodeDesigner(DosyaYolu);

                                        }
                                    }
                                    else
                                    {
                                        HandlingUnitManager.Current.UpdateCompanyPersonId(report.handlingUnit.Id, userModel.CompanyPersonId);

                                        foreach (var shopOrderProductionDetail in report.shopOrderProductionDetails)
                                        {
                                            if (ToolsMdiManager.frmOperatorActive.productionDetails.Any(p => p.Id == shopOrderProductionDetail.Id))
                                            {
                                                ToolsMdiManager.frmOperatorActive.productionDetails.RemoveAll(pd => pd.Id == shopOrderProductionDetail.Id);
                                            }

                                            shopOrderProductionDetail.BoxID = report.handlingUnit.Id;
                                            shopOrderProductionDetail.IfsReported = true;
                                            ShopOrderProductionDetailManager.Current.Update(shopOrderProductionDetail);
                                            ToolsMdiManager.frmOperatorActive.productionDetails.Add(shopOrderProductionDetail);
                                        }


                                        report.handlingUnit.SendIfs = true;
                                        report.handlingUnit.SendDate = DateTime.Now;
                                        HandlingUnitManager.Current.Update(report.handlingUnit);
                                        if (ToolsMdiManager.frmOperatorActive.handlingUnits.Any(x => x.Id == report.handlingUnit.Id))
                                        {
                                            ToolsMdiManager.frmOperatorActive.handlingUnits.RemoveAll(h => h.Id == report.handlingUnit.Id);

                                        }

                                        ToolsMdiManager.frmOperatorActive.handlingUnits.Add(report.handlingUnit);

                                        if (report.handlingUnit.Quantity < (decimal)frm.selectedPartHandlingUnit.MaxQuantityCapacity)
                                        {
                                            if (ToolsMdiManager.frmOperatorActive.halfHandlingUnit.ContainsKey(selectedModel.Id))
                                                ToolsMdiManager.frmOperatorActive.halfHandlingUnit.Remove(selectedModel.Id);

                                            ProcessHandlingUnitModel handlingUnitModel = new ProcessHandlingUnitModel();
                                            handlingUnitModel.BoxBarcode = report.handlingUnit.Barcode;
                                            handlingUnitModel.Quantity = report.handlingUnit.Quantity;
                                            handlingUnitModel.LotCount = 1;
                                            handlingUnitModel.CreatedAt = report.handlingUnit.CreatedAt;
                                            ToolsMdiManager.frmOperatorActive.halfHandlingUnit.Add(selectedModel.Id, handlingUnitModel);

                                            ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "960", "Kasa dolu olmadığı için otomatik kasa bildirimlerinde kullanılacak", "Message"));
                                        }


                                        InitDataBox();
                                        InitDataProductionDetails();
                                        InitDataBoxProductDetails();

                                        if (chkReportDesignerOpen.Checked)
                                            report.PrintBarcodeDesigner();
                                        else if (chkPrintView.Checked)
                                            report.PrintBarcodeView();
                                        else if (ToolsMdiManager.frmOperatorActive.panelDetail.BoxFillsUp)
                                            report.PrintLabel();
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
                                    {
                                        kilogramUnitPartNoDescription = CreateInputDialogBox();
                                    }
                                    manualInput = frm.value;
                                    ToolsMdiManager.frmOperatorActive._manuelInputByKilogram = manualInput;
                                    ToolsMdiManager.frmOperatorActive._PartiNoKilogramDescription = kilogramUnitPartNoDescription;
                                    ToolsMdiManager.frmOperatorActive.kilogramPlcControl = true;
                                    ToolsMdiManager.frmOperatorActive.counter++;
                                }
                                else
                                {
                                    if (userModel == null)
                                        GetUserModel();

                                    var report = ReportHandlingUnitAdetHelper.CreateReportHandlingUnit(selectedModel, frm.value, userModel, frm.selectedPartHandlingUnit, null);

                                    HandlingUnitManager.Current.UpdateCompanyPersonId(report.handlingUnit.Id, userModel.CompanyPersonId);

                                    if (chkReportDesignerOpen.Checked)
                                        report.PrintBarcodeDesigner();
                                    else if (chkPrintView.Checked)
                                        report.PrintBarcodeView();
                                    else
                                        report.PrintLabel();
                                }

                                InitDataBox();
                                InitDataProductionDetails();
                                InitDataBoxProductDetails();
                            }
                            else
                                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "961", "Girdiğiniz değer sıfır'dan yüksek olmalı", "Message"));
                        }
                    }
                    else
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "962", "Bu iş emrinde manuel olarak yeni kasa etiketi oluşturulamaz", "Message"));
                }
                else if (ucLabelselectedPage == "ProductionDetails")
                {
                    if (OperatorPanelConfigurationHelper.CanManuallyCreateShopOrderProductionDetail(selectedModel))
                    {
                        if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualLabelChecked)
                        {
                            if (!GetManuelUserModel())
                            {
                                return;
                            }
                        }
                        if (ToolsMdiManager.frmOperatorActive.panelDetail.AllowMultipleProductAdding)
                        {
                            FrmNumericKeyboard frm = new FrmNumericKeyboard("Etiket adeti");

                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                if (frm.value > 0)
                                {
                                    if (selectedModel.unitMeas == Units.ad.ToText())
                                    {
                                        for (int i = 0; i < frm.value; i++)
                                        {
                                            var detail = InsertProductionDetail();
                                            PrintProductionDetail(detail);
                                        }
                                    }
                                }
                                else
                                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "961", "Girdiğiniz değer sıfır'dan yüksek olmalı", "Message"));
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

                    else
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "963", "Bu iş emrinde manuel olarak yeni ürün etiketi oluşturulamaz", "Message"));
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
        private bool GetManuelUserModel()
        {
            FrmUserLogin frmLogin = new FrmUserLogin(UserLoginAuthorization.manuelLabelAuthorization);
            if (frmLogin.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            if (frmLogin.userModel.TwoFactorActive && frmLogin.userModel.Email != null)
            {
                if (ToolsMdiManager.frmOperatorActive.currentOtpCode == null || DateTime.Now >= ToolsMdiManager.frmOperatorActive.otpExpirationTime)
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

                while (DateTime.Now < ToolsMdiManager.frmOperatorActive.otpExpirationTime)
                {
                    string enteredOtp = OTPMailHelper.ShowInputDialog($"{ToolsMdiManager.frmOperatorActive.resource.resourceName}-Doğrulama İşlemi", $"Sayın {frmLogin.userModel.Name},\r\nMail adresinize gelen doğrulama kodunu giriniz:", ToolsMdiManager.frmOperatorActive.otpExpirationTime);

                    if (string.IsNullOrEmpty(enteredOtp))
                    {
                        ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "İşlem iptal edildi.");
                        return false;
                    }

                    if (enteredOtp == ToolsMdiManager.frmOperatorActive.currentOtpCode)
                    {
                        ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Doğrulama başarılı!");
                        ToolsMdiManager.frmOperatorActive.manuelLabelUser = frmLogin.userModel;
                        return true;
                    }

                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Doğrulama kodu yanlış. Tekrar deneyin.");
                }

                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Zaman aşımına uğradınız.");
                ToolsMdiManager.frmOperatorActive.currentOtpCode = null; // Süre dolduğunda kodu sıfırla
                currentOtpCode = null;
                return false;
            }

            ToolsMdiManager.frmOperatorActive.manuelLabelUser = frmLogin.userModel;
            return true;
        }

        private string GenerateOtp()
        {
            var secretKey = Base32Encoding.ToBytes("JBSWY3DPEHPK3PXP"); // Gizli anahtar
            var totp = new Totp(secretKey, step: 300);  // 5 dakikalık geçerlilik
            return totp.ComputeTotp();
        }


        private void GetUserModel()
        {
            FrmUserLogin frm = new FrmUserLogin(false);
            if (frm.ShowDialog() == DialogResult.OK)
                userModel = frm.userModel;
            else
                throw new Exception(MessageTextHelper.GetMessageText("000", "629", "Yetkili kullanıcı girişi yapmadan etiket alamazsınız", "Message"));
        }

        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }
        #endregion

        private ShopOrderProductionDetail InsertProductionDetail(decimal quantity = 1, decimal handlingunitQuantity = 0)
        {
            var detail = ShopOrderProductionDetailHelper.CreateAndInsertProductionDetail(null, ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == selectedModel.ProductID), true, userModel, ToolsMdiManager.frmOperatorActive.Users.Count, quantity, handlingunitQuantity);
            ToolsMdiManager.frmOperatorActive.productionDetails.Add(detail);

            InitDataProductionDetails();

            return detail;
        }

        private void PrintProductionDetail(ShopOrderProductionDetail detail)
        {
            if (ToolsMdiManager.frmOperatorActive.panelDetail.PrintProductBarcode || ToolsMdiManager.frmOperatorActive.panelDetail.ProcessBarcode)
            {
                var report = ReportHelper.CreateProductionDetail(userModel, selectedModel, ToolsMdiManager.frmOperatorActive.shopOrderProduction, detail);

                if (chkPrintView.Checked)
                    report.PrintBarcodeView();
                if (chkReportDesignerOpen.Checked)
                    report.PrintBarcodeDesigner();
                else
                    report.PrintLabel();
            }
        }

        private void glueShopOrderOperation_EditValueChanged(object sender, System.EventArgs e)
        {
            var shopOrderOperationSelectedId = (Guid)glueShopOrderOperation.EditValue;
            selectedModel = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == shopOrderOperationSelectedId);
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

        private void gvProductionHandlingUnits_MasterRowGetRelationDisplayCaption(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            string companyName = (string)gvProductionHandlingUnits.GetRowCellValue(e.RowHandle, "Barcode");
            if (e.RelationIndex == 0)
                e.RelationName = $"{companyName}";
        }

        private void xtcLabelBox_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtcLabelBox.TabPages.IndexOf(e.Page) == 0)//Kasa listesi
            {
                ucLabelselectedPage = "Box";
            }
            else if (xtcLabelBox.TabPages.IndexOf(e.Page) == 1)//Ürün Listesi Listesi
            {
                ucLabelselectedPage = "ProductionDetails";
            }
            else if (xtcLabelBox.TabPages.IndexOf(e.Page) == 2)//Kasa Ürün Detay Listesi
            {
                ucLabelselectedPage = "BoxProductDetails";
            }
            else if (xtcLabelBox.TabPages.IndexOf(e.Page) == 3)//Şüpheli Ürün Detay Listesi
            {
                ucLabelselectedPage = "ScrapProductionDetails";
            }
            else//SÜRELER
            {

            }
        }

        private void chkPrintView_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrintView.Checked)
                chkReportDesignerOpen.Checked = false;
        }

        private void chkReportDesignerOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReportDesignerOpen.Checked)
                chkPrintView.Checked = false;
        }


        #region CreateInputDialogBox
        private string CreateInputDialogBox()
        {

            XtraInputBoxArgs args = new XtraInputBoxArgs();

            args.Caption = MessageTextHelper.GetMessageText("000", "861", "Parti Numarası", "Message");
            args.Prompt = MessageTextHelper.GetMessageText("000", "862", "Lütfen parti numarasını giriniz", "Message");
            args.DefaultButtonIndex = 0;
            args.Showing += Args_Showing;

            TextEdit editor = new TextEdit();


            editor.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            editor.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            editor.Properties.Appearance.Options.UseFont = true;
            editor.Properties.Appearance.Options.UseForeColor = true;
            editor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.PeachPuff;
            editor.Properties.AppearanceFocused.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            editor.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Black;
            editor.Properties.AppearanceFocused.Options.UseBackColor = true;
            editor.Properties.AppearanceFocused.Options.UseFont = true;
            editor.Properties.AppearanceFocused.Options.UseForeColor = true;
            editor.Size = new System.Drawing.Size(338, 28);


            args.Editor = editor;

            args.DefaultResponse = "";
            var result = XtraInputBox.Show(args);
            if (result == null)
            {
                throw new Exception(MessageTextHelper.GetMessageText("000", "630", "Operasyon numarası girmeden ürün listesine ulaşamazsınız", "Message"));
            }
            else
                return result.ToString();
        }
        private void Args_Showing(object sender, XtraMessageShowingArgs e)
        {
            //e.Form.Icon = this.Icon;
        }
        #endregion

        private void barBtnBoxComplete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var denemeSil = ToolsMdiManager.frmOperatorActive.productionDetails.Where(x => x.BoxID == Guid.Empty).ToList();
        }

        private void chkHideOtherShifts_CheckedChanged(object sender, EventArgs e)
        {
            InitDataProductionDetails();
            InitDataBoxProductDetails();
        }

        private void gvHandlingUnitMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            // Check Panel Parameter
            if (ToolsMdiManager.frmOperatorActive.panelDetail.HalfCaseStatus)
            {
                if (e.RowHandle >= 0)
                {
                    var proxy = (RealTimeProxyForObject)gvHandlingUnitMain.GetRow(e.RowHandle);
                    var quantity = (double)proxy.Content.Where(x => x.Key.DisplayName == "Quantity").FirstOrDefault().Value;
                    if (quantity < selectedModel.MaxQuantityCapacity)
                        e.Appearance.BackColor = Color.LightBlue;
                }
            }
        }
    }

}
