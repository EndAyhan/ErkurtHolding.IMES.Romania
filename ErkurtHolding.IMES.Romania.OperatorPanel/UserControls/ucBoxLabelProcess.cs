using DevExpress.Data;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
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
    public partial class ucBoxLabelProcess : DevExpress.XtraEditors.XtraUserControl
    {
        private List<int> handlingUnitSelectedRows = new List<int>();
        private List<int> productionDetailSelectedRows = new List<int>();
        private List<int> scrapProductionDetailSelectedRows = new List<int>();
        private List<HandlingUnit> handlingUnits = null;
        private List<ProcessHandlingUnitModel> processHandlingUnitModels = new List<ProcessHandlingUnitModel>();
        private BindingList<ProductionDetailGridModel> productionGridModels;
        private UserModel userModel { get; set; }
        private GridLookUpEditBestPopupFormSizeHelper _gluePopupSizer;

        string ucLabelselectedPage = "Box";
        public ucBoxLabelProcess(UserModel model)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            handlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.Select(x => x.Id).ToList());
            if (handlingUnits == null)
                handlingUnits = new List<HandlingUnit>();

            userModel = model;

            _gluePopupSizer = new GridLookUpEditBestPopupFormSizeHelper(glueShopOrderOperation, initialHeight: 200, width: 450);

            InitData();
            InitDataBox();
            InitDataProductionDetails();
            InitDataScrapProductionDetails();
            chkReportDesignerOpen.Visible = ToolsMdiManager.frmOperatorActive.Users.Any(u => u.Role == 4);
        }

        #region INIT DATA
        private void InitData()
        {
            glueShopOrderOperation.Properties.DataSource = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
            glueShopOrderOperation.EditValue = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.OrderByDescending(x => x.opStartDate).First().Id;
        }

        private void InitDataBox()
        {

            //handlingUnits.Clear();
            handlingUnitSelectedRows.Clear();
            processHandlingUnitModels = new List<ProcessHandlingUnitModel>();

            List<HandlingUnit> filteredHandlingUnits;
            if (chkHideOtherShifts.Checked)
            {
                filteredHandlingUnits = handlingUnits.Where(x => x.ShopOrderProductionID == ToolsMdiManager.frmOperatorActive.shopOrderProduction.Id).ToList();
                filteredHandlingUnits = handlingUnits.Where(x => filteredHandlingUnits.Select(y => y.Barcode).Contains(x.Barcode)).ToList();
            }
            else
                filteredHandlingUnits = handlingUnits;

            if (!filteredHandlingUnits.HasEntries())
            {
                gcHandlingUnitMain.DataSource = processHandlingUnitModels;
                return;
            }

            var queryResult = from h in filteredHandlingUnits
                              group h by new { h.Barcode/*, h.ShopOrderProductionID*/ } into grp
                              select new
                              {
                                  barcode = grp.Key.Barcode,
                                  //productionId = grp.Key.ShopOrderProductionID,
                                  quantity = grp.Sum(q => q.Quantity),
                                  lotCount = grp.Count()
                              };

            foreach (var query in queryResult)
            {
                ProcessHandlingUnitModel handlingUnitModel = new ProcessHandlingUnitModel();
                handlingUnitModel.BoxBarcode = query.barcode;
                //handlingUnitModel.shopOrderProductionId = query.productionId;
                handlingUnitModel.Quantity = query.quantity;
                handlingUnitModel.LotCount = query.lotCount;
                handlingUnitModel.CreatedAt = filteredHandlingUnits.Where(x => x.Barcode == query.barcode).OrderBy(o => o.CreatedAt).First().CreatedAt;
                processHandlingUnitModels.Add(handlingUnitModel);
            }

            gcHandlingUnitMain.DataSource = processHandlingUnitModels;
        }

        private void InitDataProductionDetails()
        {
            productionGridModels = new BindingList<ProductionDetailGridModel>();

            var shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p => p.ByProduct == false && p.ProductionStateID == StaticValues.specialCodeOk.Id).ToList();
            if (chkHideOtherShifts.Checked)
                shopOrderProductionDetails = shopOrderProductionDetails.Where(x => x.ShiftId == StaticValues.shift.Id).ToList();

            foreach (var item in shopOrderProductionDetails)
            {
                try
                {
                    var order = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(o => o.Id == item.ShopOrderOperationID);

                    ProductionDetailGridModel productionDetail = new ProductionDetailGridModel();
                    productionDetail.Id = item.Id;
                    productionDetail.ShopOrderOperationID = item.ShopOrderOperationID;
                    productionDetail.ShopOrderProductionID = item.ShopOrderProductionID;
                    productionDetail.ProductID = item.ProductID;
                    productionDetail.OrderNo = order.orderNo;
                    productionDetail.OperationNo = order.operationNo.ToString();
                    productionDetail.Barcode = item.Barcode;
                    productionDetail.Serial = item.serial.ToString();
                    productionDetail.CreateAt = item.CreatedAt;
                    productionDetail.ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName;
                    productionDetail.Quantity = item.Quantity;
                    productionDetail.ManualInput = item.ManualInput;
                    productionDetail.PartHandlingBoxBarcode = item.BoxID == Guid.Empty ? "" : handlingUnits.First(p => p.Id == item.BoxID).Barcode;

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

            var shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p => p.ByProduct == false && p.ProductionStateID == StaticValues.specialCodeScrap.Id).ToList();

            foreach (var item in shopOrderProductionDetails)
            {
                try
                {
                    var order = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(o => o.Id == item.ShopOrderOperationID);

                    ProductionDetailGridModel productionDetail = new ProductionDetailGridModel();
                    productionDetail.Id = item.Id;
                    productionDetail.ShopOrderOperationID = item.ShopOrderOperationID;
                    productionDetail.ShopOrderProductionID = item.ShopOrderProductionID;
                    productionDetail.ProductID = item.ProductID;
                    productionDetail.OrderNo = order.orderNo;
                    productionDetail.OperationNo = order.operationNo.ToString();
                    productionDetail.Barcode = item.Barcode;
                    productionDetail.Serial = item.serial.ToString();
                    productionDetail.CreateAt = item.CreatedAt;
                    productionDetail.ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName;
                    productionDetail.Quantity = item.Quantity;
                    productionDetail.ManualInput = item.ManualInput;
                    productionDetail.PartHandlingBoxBarcode = item.BoxID == Guid.Empty ? "" : handlingUnits.First(p => p.Id == item.BoxID).Barcode;

                    productionGridModels.Add(productionDetail);
                }
                catch (Exception)
                {
                }
            }
            gcScrapProducts.DataSource = productionGridModels;
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
        #endregion

        #region CHKHIDE OTHER SHIFTS CHECKED CHANGED
        private void chkHideOtherShifts_CheckedChanged(object sender, EventArgs e)
        {
            InitDataBox();
            InitDataProductionDetails();
        }

        #endregion

        #region MENU BUTTON CLICK EVENT
        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

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
                        foreach (var selectedRow in handlingUnitSelectedRows)
                        {
                            var handling = (ProcessHandlingUnitModel)gvHandlingUnitMain.GetRow(selectedRow);

                            ReportProcessHandlingUnitHelper report = new ReportProcessHandlingUnitHelper();

                            var handlings = handlingUnits.Where(x => x.Barcode == handling.BoxBarcode).ToList();

                            foreach (var item in handlings)
                            {
                                report.shopOrderOperations.Add(ShopOrderOperationManager.Current.GetShopOrderOperationById(item.ShopOrderID));
                                report.vwShopOrderGridModels.Add(ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderID));
                                report.handlingUnits.Add(item);

                                report.shopOrderProductionDetails.AddRange(ToolsMdiManager.frmOperatorActive.productionDetails.Where(x => x.BoxID == item.Id).ToList());
                            }
                            report.product = ToolsMdiManager.frmOperatorActive.products[0];
                            report.machine = ToolsMdiManager.frmOperatorActive.machine;
                            report.resource = ToolsMdiManager.frmOperatorActive.resource;
                            report.shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction;
                            report.user = userModel;
                            report.printLabelModel = ToolsMdiManager.frmOperatorActive.printLabelModels.First(p => p.ProductId == report.product.Id && p.productionLabelType == ProductionLabelType.Box);

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

                                    report.PrintBarcodeSelectFileDesigner(DosyaYolu);

                                }
                            }
                            else
                                report.PrintLabel();
                        }
                    }
                    else
                        ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "957", "Yazdırmak istediğiniz etiket secili değil", "Message"));
                }
                else if (ucLabelselectedPage == "ProductionDetails")
                {
                    if (productionDetailSelectedRows.Count > 0)
                    {
                        foreach (var selectedRow in productionDetailSelectedRows)
                        {
                            var product = (ProductionDetailGridModel)gridViewProductionDetail.GetRow(selectedRow);
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == product.Id);
                            var order = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(o => o.Id == pd.ShopOrderOperationID);

                            if (order.alan5 == "TRUE")
                            {
                                if (chkDesignCheck.Checked)//Yeni bir dizayn yapılmak isteniyor ise
                                {
                                    OpenFileDialog openFileDialog = new OpenFileDialog();
                                    openFileDialog.Filter = "Etiket Dizayn Dosyası |*.repx";
                                    openFileDialog.RestoreDirectory = true;
                                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        string DosyaYolu = openFileDialog.FileName;
                                        var myReport = ReportHelper.CreateProductionDetail(userModel, order, ToolsMdiManager.frmOperatorActive.shopOrderProduction, pd, false);
                                        myReport.PrintBarcodeDesigner(DosyaYolu);

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
                                var prm = product.Serial.CreateParameters("@Serial");
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
                        foreach (var selectedRow in scrapProductionDetailSelectedRows)
                        {
                            var productGM = (ProductionDetailGridModel)gvScrapProducts.GetRow(selectedRow);
                            var pd = ToolsMdiManager.frmOperatorActive.productionDetails.First(x => x.Id == productGM.Id);
                            var product = ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == pd.ProductID);
                            var order = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(o => o.Id == pd.ShopOrderOperationID);

                            ReportProductHelper report = new ReportProductHelper();
                            report.product = product;
                            report.shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction;
                            report.shopOrderProductionDetail = pd;
                            report.machine = ToolsMdiManager.frmOperatorActive.machine;
                            report.resource = ToolsMdiManager.frmOperatorActive.resource;
                            report.userModel = userModel;
                            report.shopOrderOperation = order;
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
        #endregion

        #region GET USER MODEL
        private void GetUserModel()
        {
            FrmUserLogin frm = new FrmUserLogin(false);
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

        private void gvHandlingUnitMain_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                var handling = (ProcessHandlingUnitModel)gvHandlingUnitMain.GetRow(e.RowHandle);
                if (handling.Quantity < (decimal)ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive.MaxQuantityCapacity)
                    e.Appearance.BackColor = Color.LightBlue;
            }
        }
    }
}
