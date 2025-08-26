using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.GridModels;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucQuestionableProduct : DevExpress.XtraEditors.XtraUserControl
    {
        ObservableCollection<int> selectedRows = new ObservableCollection<int>();
        List<QuestionableProductGridModel> IfsSendproductionDetails = new List<QuestionableProductGridModel>();
        ScrapReasons scrapReasons;
        UserModel userModel;
        private List<QuestionableProductGridModel> questionableProductGridModels = new List<QuestionableProductGridModel>();

        #region CONSTRUCTOR
        public ucQuestionableProduct(UserModel _userModel)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            btnManualAddScrapProduct.Visible = ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualScrapInjection;

            userModel = _userModel;
            selectedRows.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    {
                        spnQuantity.Value = selectedRows.Count;
                        spnQuantity.Enabled = false;
                        spnQuantity.ReadOnly = true;

                    }
                    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        spnQuantity.Value = selectedRows.Count;

                        if (selectedRows.Count == 0)
                        {
                            spnQuantity.Enabled = true;
                            spnQuantity.ReadOnly = false;
                        }
                    }
                });


            scrapReasons = ScrapReasonsManager.Current.GetScrapReasonsByRejectMessage(
                StaticValues.panel.BranchId,
                ToolsMdiManager.frmOperatorActive.machine.Id,
                StaticValues.scrapReasonsUnclearProductRejectMessage);

            if (scrapReasons == null)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "983", "IFS üzerinde iş merkezi için tanımlı ŞÜPHELİ ÜRÜN bilgisi olmadığı için devam edilemiyor. Lütfen sistem yöneticisine başvurunuz", "Message"));
                ToolsMdiManager.frmOperatorActive.container.Visible = false;

            }

            questionableProductGridModels = questionableProductGridModels
            .OrderByDescending(x => x.Barcode)
            .ToList();


            gridControl1.DataSource = questionableProductGridModels;
            var view = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SortInfo.ClearSorting(); // Mevcut sıralamayı temizle
            view.SortInfo.Add(view.Columns["Barcode"], DevExpress.Data.ColumnSortOrder.Descending);

            InitProduct();
            InitShopOrder();
        }
        #endregion

        #region INITDATA GRID LOOK UP EDIT
        private void InitShopOrder()
        {
            try
            {
                var shoporders = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
                glueShopOrder.Properties.DataSource = shoporders;
                glueShopOrder.EditValue = shoporders.FirstOrDefault().Id;
            }
            catch (Exception)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "639", "İş Emri Olmadan Şüpheliye Ayırmak İçin Tamam' a Basın Sadece Barcod Okutarak İşlem Yapabilirsiniz\n", "Message"));
            }
        }

        private void InitProduct()
        {
            var products = ToolsMdiManager.frmOperatorActive.products;
            glueProduct.Properties.DataSource = products;
        }

        #endregion

        #region txtBarcode_KeyDown
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtBarcode.Text == string.Empty)
                return;

            if (Keys.Enter == e.KeyCode)
            {
                int rowHandle = gridView1.LocateByValue("Serial", txtBarcode.Text);
                if (rowHandle < 0)
                {
                    rowHandle = gridView1.LocateByValue("Barcode", txtBarcode.Text);
                    if (rowHandle < 0)
                    {
                        var pd = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetailByBarcodeOrSerial(txtBarcode.Text);
                        if (pd == null)
                        {
                            var prm = txtBarcode.Text.CreateParameters("@Barcode");
                            ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "984", "@Barcode Barkoduna ait Ürün bulunamadı", "Message"), prm);
                        }
                        else
                        {
                            if (pd.ProductionStateID == StaticValues.specialCodeScrap.Id)
                            {
                                var prm = txtBarcode.Text.CreateParameters("@Barcode");
                                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "985", "@Barcode Barkodu daha önce şüpheliye ayrılmıştır", "Message"), prm);
                            }

                            else if (pd.ProductionStateID == StaticValues.specialCodeNotOk.Id)
                            {
                                var prm = txtBarcode.Text.CreateParameters("@Barcode");
                                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "986", "@Barcode Barkodu daha önce hurdaya ayrılmıştır", "Message"), prm);
                            }

                            else
                            {
                                AddItemToGridControl(
                                                        pd,
                                                        ProductManager.Current.GetProductById(pd.ProductID),
                                                        vw_ShopOrderGridModelManager.Current.GetShopOrderOperationModelById(pd.ShopOrderOperationID),
                                                        ShopOrderProductionManager.Current.GetShopOrderProductionById(pd.ShopOrderProductionID),
                                                        MachineManager.Current.GetMachineById(pd.WorkCenterID),
                                                        MachineManager.Current.GetMachineById(pd.ResourceID));

                                gridControl1.RefreshDataSource();
                                gridControl1.Refresh();
                                gridView1.RefreshData();
                                gridView1.RefreshEditor(true);
                                gridView1.RefreshRow(0);
                                int newRowHandle = gridView1.LocateByValue("Serial", pd.serial.ToString());
                                if (newRowHandle >= 0)
                                {
                                    gridView1.FocusedRowHandle = newRowHandle;
                                    gridView1.SelectRow(newRowHandle);
                                }

                            }
                        }
                    }
                    else
                        gridView1.SelectRow(rowHandle);

                }
                else
                    gridView1.SelectRow(rowHandle);

                txtBarcode.SelectAll();
            }
        }
        #endregion

        #region GRID LOOK UP EDIT VALUE CHANGED EVENT
        bool editValueChanged = false;
        private void glueProduct_EditValueChanged(object sender, EventArgs e)
        {
            if (editValueChanged)
                return;

            if (selectedRows.Count > 0)
            {
                if (ToolsMessageBox.Question(this, MessageTextHelper.GetMessageText("000", "987", "Seçimleri kaydetmek istiyor musunuz?", "Message")))
                {
                    FrmNote frm = new FrmNote();
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    SetProductionDetailScrap(frm.GetNote());
                    ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == (Guid)glueShopOrder.EditValue);
                    ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
                    ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)glueShopOrder.EditValue);
                }
                selectedRows.Clear();
                spnQuantity.Value = 0;
            }

            editValueChanged = true;
            var productId = (Guid)glueProduct.EditValue;
            glueShopOrder.EditValue = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(p => p.ProductID == productId).Id;
            editValueChanged = false;
            InitShopOrderProductionDetails();
        }

        private void glueShopOrder_EditValueChanged(object sender, EventArgs e)
        {
            if (editValueChanged)
                return;

            if (selectedRows.Count > 0)
            {
                if (ToolsMessageBox.Question(this, MessageTextHelper.GetMessageText("000", "987", "Seçimleri kaydetmek istiyor musunuz?", "Message")))
                {
                    FrmNote frm = new FrmNote();
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    SetProductionDetailScrap(frm.GetNote());
                    ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == (Guid)glueShopOrder.EditValue);
                    ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
                    ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)glueShopOrder.EditValue);
                }
                selectedRows.Clear();
                spnQuantity.Value = 0;
            }
            editValueChanged = true;
            var shopOrdertId = (Guid)glueShopOrder.EditValue;
            glueProduct.EditValue = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(s => s.Id == shopOrdertId).ProductID;
            editValueChanged = false;
            InitShopOrderProductionDetails();
        }
        #endregion

        #region INITDATA GRID VIEW 
        private void InitShopOrderProductionDetails()
        {
            if (glueShopOrder.EditValue != null && glueShopOrder.Text != String.Empty)
            {
                var Id = (Guid)glueProduct.EditValue;
                var result = ToolsMdiManager.frmOperatorActive.productionDetails
                    .Where(p =>
                        p.ShopOrderOperationID == (Guid)glueShopOrder.EditValue &&
                        p.ProductID == (Guid)glueProduct.EditValue &&
                        p.ProductionStateID == StaticValues.specialCodeOk.Id)
                    .OrderBy(p => p.serial)
                    .ToList();

                questionableProductGridModels.Clear();
                selectedRows.Clear();
                spnQuantity.Value = 0;
                gridView1.ClearSelection();
                spnQuantity.Enabled = true;
                spnQuantity.ReadOnly = false;

                foreach (var item in result)
                {
                    AddItemToGridControl(
                        item,
                        ToolsMdiManager.frmOperatorActive.products.First(x => x.Id == item.ProductID),
                        ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID),
                        ToolsMdiManager.frmOperatorActive.shopOrderProduction,
                        ToolsMdiManager.frmOperatorActive.machine,
                        ToolsMdiManager.frmOperatorActive.resource);
                }

                gridControl1.RefreshDataSource();
                gridControl1.Refresh();
                gridView1.RefreshData();
                gridView1.RefreshEditor(true);
                gridView1.RefreshRow(0);
            }
        }

        private void AddItemToGridControl(ShopOrderProductionDetail item, Product product, vw_ShopOrderGridModel sogm, ShopOrderProduction sop, Machine machine, Machine resource)
        {
            var questionableProductGridModel = new QuestionableProductGridModel();
            questionableProductGridModel.ShopOrderProductionDetail = item;
            questionableProductGridModel.Product = product;
            questionableProductGridModel.ShopOrderProduction = sop;
            questionableProductGridModel.ShopOrderGridModel = sogm;
            questionableProductGridModel.Machine = machine;
            questionableProductGridModel.Resource = resource;

            questionableProductGridModels.Add(questionableProductGridModel);
        }
        #endregion

        #region GRID VIEW EVENTS

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {

            if (e.Action == CollectionChangeAction.Add)
            {
                spnQuantity.Value += 1;
                selectedRows.Add(e.ControllerRow);
            }
            else if (e.Action == CollectionChangeAction.Remove)
            {
                spnQuantity.Value -= 1;
                selectedRows.Remove(e.ControllerRow);
            }
            else if (e.Action == CollectionChangeAction.Refresh)
            {
                var rows = gridView1.GetSelectedRows();
                selectedRows.Clear();
                if (rows.Length != 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        selectedRows.Add(rows[i]);
                    }
                }
            }
        }
        #endregion

        #region QUANTITY SPIN EDIT EVENT
        private void spnQuantity_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            var value = Convert.ToDecimal(e.NewValue);
            if (value < 0 /*|| glueShopOrder.Text == ""*/)
                e.Cancel = true;
            if (value > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }
        #endregion        

        #region BUTTONS  CLICK EVENT
        private void btnSave_Click(object sender, EventArgs e)
        {
            selectedRows.Clear();
            var rowsIndex = gridView1.GetSelectedRows();
            foreach (var item in rowsIndex)
            {
                selectedRows.Add(item);
            }
            if (spnQuantity.Value > 0)
            {
                if (ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.Count() == 0 && (QuestionableProductGridModel)gridView1.GetRow(0) == null)
                {
                    var rowObject = (QuestionableProductGridModel)gridView1.GetRow(0);
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "638", "Herhangi Bir İş Emri Seçili Olmadığı İçin Miktar Girerek İşlem Yapamazsınız.\n", "Message"));
                    return;
                }
                FrmNote frm = new FrmNote();
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                SetProductionDetailScrap(frm.GetNote());
                if (glueShopOrder.EditValue != null)
                {
                    ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(x => x.Id == (Guid)glueShopOrder.EditValue);
                    ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
                    ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)glueShopOrder.EditValue);
                }
            }
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void btnManualAddScrapProduct_Click(object sender, EventArgs e)
        {
            if (glueShopOrder.Text == "")
            {
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "988", "Şüpheli ürün eklemeden önce ilgili iş emrini seçmelisiniz", "Message"));
                return;
            }

            var prm = glueShopOrder.Text.CreateParameters("@ShopOrder");
            if (ToolsMessageBox.Question(this, MessageTextHelper.GetMessageText("000", "989", "@ShopOrder numaralı iş emrine manuel olarak şüpheli ürün eklemek istiyor musunuz?", "Message"), prm))
            {
                FrmNumericKeyboard frm = new FrmNumericKeyboard("Iskarta miktarını giriniz");

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm.value > 0)
                    {
                        var selectedModel = (vw_ShopOrderGridModel)glueShopOrder.GetSelectedDataRow();
                        if (frm.value < selectedModel.revisedQtyDue)
                        {
                            var product = (Product)glueProduct.GetSelectedDataRow();

                            var detail = ShopOrderProductionDetailHelper.CreateAndInsertScrapProductionDetail(ToolsMdiManager.frmOperatorActive, selectedModel, product, true, userModel, 1, frm.value, frm.value, true);
                            ToolsMdiManager.frmOperatorActive.productionDetails.Add(detail);//?

                            ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(StaticValues.specialCodeScrap.Id, scrapReasons.Id, detail.Id);

                            detail.ProductionStateID = StaticValues.specialCodeScrap.Id;
                            detail.IfsScrapTypeId = scrapReasons.Id;
                            detail.IfsReported = true;
                            detail.UpdatedAt = DateTime.Now;

                            ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = selectedModel;
                            ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
                            ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)selectedModel.Id);

                            prm.Add("@Amount", frm.value);
                            ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "990", "@ShopOrder numaralı iş emrine @Amount şüpheli ürün eklenmiştir", "Message"));
                            ToolsMdiManager.frmOperatorActive.container.Visible = false;
                        }
                        else
                            ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "894", "Şüpheli ürün miktarı iş emri miktarından az olmalıdır!", "Message"));
                    }
                    else
                    {
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "991", "Şüpheli ürün miktarı sıfırdan büyük olmalıdır!", "Message"));
                    }
                }
            }
        }
        private void btnSplitProductionDetail_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle >= 0)
            {
                var row = (QuestionableProductGridModel)gridView1.GetRow(gridView1.FocusedRowHandle);
                if (row.Unit == Units.ad.ToText() && row.Quantity < 2)
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "992", "Şüpheli ürün ayrıştırabilmek için adet miktarı 1'den büyük olmalıdır!", "Message"));
                    return;
                }
                FrmSplitProductDetail frm = new FrmSplitProductDetail(row);
                if (frm.ShowDialog() == DialogResult.OK)
                    InitShopOrderProductionDetails();
            }
        }
        #endregion

        #region SAVE DATA SetProductionDetailScrap
        private void SetProductionDetailScrap(string note)
        {
            try
            {
                IfsSendproductionDetails.Clear();
                if (selectedRows.Count == 0 && glueShopOrder.EditValue != null)
                {
                    decimal enteredQty = 0;
                    if (!decimal.TryParse(spnQuantity.Value.ToString(), out enteredQty))
                    {
                        var prm = decimal.Parse(spnQuantity.Value.ToString()).CreateParameters("@Value");
                        ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "951", "@Value geçerli bir değer değil", "Message"), prm);
                        return;
                    }

                    var totalQuantity = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p =>
                        p.ShopOrderOperationID == (Guid)glueShopOrder.EditValue &&
                        p.ProductionStateID == StaticValues.specialCodeOk.Id).Sum(x => x.Quantity);

                    if (enteredQty > totalQuantity)
                    {
                        ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "975", "Online kontrolde ilgili malzemeden yeterli miktarda ürün bulunamadı", "Message"));
                        return;
                    }

                    var selectedLastList = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p =>
                                    p.ShopOrderOperationID == (Guid)glueShopOrder.EditValue &&
                                    p.ProductionStateID == StaticValues.specialCodeOk.Id)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Take((int)spnQuantity.Value)
                                    .ToList();

                    var sumQuantity = selectedLastList.Sum(x => x.Quantity);

                    #region ManualQuantityEntered

                    if (sumQuantity != (decimal)spnQuantity.Value)
                    {
                        ShopOrderProductionDetail lastProdDetail = new ShopOrderProductionDetail();
                        ShopOrderProductionDetail splitProdDetail = new ShopOrderProductionDetail();

                        decimal listLastSum = 0;
                        decimal surplus = 0;
                        List<ShopOrderProductionDetail> selectedLastListRev = new List<ShopOrderProductionDetail>();
                        foreach (var item in selectedLastList)
                        {
                            selectedLastListRev.Add(item);
                            listLastSum += item.Quantity;

                            if (listLastSum == enteredQty)
                                break;

                            else if (listLastSum > enteredQty)
                            {
                                surplus = listLastSum - enteredQty;
                                lastProdDetail = item;

                                if (lastProdDetail.Quantity > surplus)
                                {
                                    if (enteredQty <= 0)
                                    {
                                        ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "950", "Geçerli bir değer girmelisiniz", "Message"));
                                        return;
                                    }

                                    splitProdDetail.ShopOrderOperationID = lastProdDetail.ShopOrderOperationID;
                                    splitProdDetail.ShopOrderProductionID = lastProdDetail.ShopOrderProductionID;
                                    splitProdDetail.WorkCenterID = lastProdDetail.WorkCenterID;
                                    splitProdDetail.ResourceID = lastProdDetail.ResourceID;
                                    splitProdDetail.ShiftId = lastProdDetail.ShiftId;
                                    splitProdDetail.StartDate = lastProdDetail.StartDate;
                                    splitProdDetail.EndDate = lastProdDetail.EndDate;
                                    splitProdDetail.Unit = lastProdDetail.Unit;
                                    splitProdDetail.BoxID = lastProdDetail.BoxID;
                                    splitProdDetail.ProductionStateID = lastProdDetail.ProductionStateID;
                                    splitProdDetail.Factor = lastProdDetail.Factor;
                                    splitProdDetail.Divisor = lastProdDetail.Divisor;
                                    splitProdDetail.Printed = lastProdDetail.Printed;
                                    splitProdDetail.TypeId = lastProdDetail.TypeId;
                                    splitProdDetail.IfsReported = lastProdDetail.IfsReported;
                                    splitProdDetail.ProductID = lastProdDetail.ProductID;
                                    splitProdDetail.IfsScrapTypeId = lastProdDetail.IfsScrapTypeId;
                                    splitProdDetail.ParHandlingUnitID = lastProdDetail.ParHandlingUnitID;
                                    splitProdDetail.OrderNo = lastProdDetail.OrderNo;
                                    splitProdDetail.OperationNo = lastProdDetail.OperationNo;
                                    splitProdDetail.CustomerBoxID = lastProdDetail.CustomerBoxID;
                                    splitProdDetail.HandlingUnitChange = lastProdDetail.HandlingUnitChange;
                                    splitProdDetail.ByProduct = lastProdDetail.ByProduct;
                                    splitProdDetail.OperatorNote = lastProdDetail.OperatorNote;
                                    splitProdDetail.QualityOperatorNote = lastProdDetail.QualityOperatorNote;
                                    splitProdDetail.CrewSize = lastProdDetail.CrewSize;
                                    splitProdDetail.CreatedAt = lastProdDetail.CreatedAt;
                                    splitProdDetail.UpdatedAt = lastProdDetail.UpdatedAt;
                                    splitProdDetail.Active = lastProdDetail.Active;

                                    splitProdDetail.Barcode = lastProdDetail.Barcode;
                                    splitProdDetail.Quantity = surplus;
                                    lastProdDetail.Quantity -= surplus;
                                    if (lastProdDetail.ManualInput < lastProdDetail.Quantity)
                                        splitProdDetail.ManualInput = 0;
                                    else
                                    {
                                        splitProdDetail.ManualInput = lastProdDetail.ManualInput - lastProdDetail.Quantity;
                                        lastProdDetail.ManualInput = lastProdDetail.Quantity;
                                    }
                                    if (lastProdDetail.Quantity >= lastProdDetail.HandlingUnitQuantity)
                                    {
                                        splitProdDetail.HandlingUnitQuantity = 0;
                                        splitProdDetail.IfsReported = false;
                                    }
                                    else
                                    {
                                        splitProdDetail.HandlingUnitQuantity = lastProdDetail.HandlingUnitQuantity - lastProdDetail.Quantity;
                                        lastProdDetail.HandlingUnitQuantity = lastProdDetail.Quantity;
                                    }

                                    ShopOrderProductionDetailManager.Current.Update(lastProdDetail);
                                    ToolsMdiManager.frmOperatorActive.productionDetails.Remove(lastProdDetail);
                                }
                                break;
                            }
                        }

                        selectedLastList.Clear();
                        selectedLastList.AddRange(selectedLastListRev);

                        var res = ShopOrderProductionDetailManager.Current.Insert(splitProdDetail).ListData[0];
                        ToolsMdiManager.frmOperatorActive.productionDetails.Add(res);
                        ToolsMdiManager.frmOperatorActive.productionDetails.Add(lastProdDetail);
                    }
                    #endregion


                    if (selectedLastList != null && selectedLastList.Count > 0)
                    {
                        foreach (var selected in selectedLastList)
                        {
                            int rowHandle = gridView1.LocateByValue("Id", selected.Id);
                            if (rowHandle < 0)
                                continue;
                            var qpgm = (QuestionableProductGridModel)gridView1.GetRow(rowHandle);

                            var scrapHandlingUnit = ToolsMdiManager.frmOperatorActive.handlingUnits.Where(x => x.Id == selected.BoxID).FirstOrDefault();

                            ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(
                                ProductionStateID: StaticValues.specialCodeScrap.Id,
                                IfsScrapTypeId: scrapReasons.Id,
                                Id: qpgm.Id);
                            ShopOrderProductionDetailManager.Current.UpdateOperatorNote(qpgm.Id, note);

                            if (scrapHandlingUnit != null)
                            {
                                scrapHandlingUnit.Quantity -= qpgm.Quantity;
                                HandlingUnitManager.Current.UpdateHandlingUnitQuantity(scrapHandlingUnit.Id, scrapHandlingUnit.Quantity);
                            }

                            qpgm.ShopOrderProductionDetail.BoxID = Guid.Empty;
                            qpgm.ShopOrderProductionDetail.CustomerBoxID = Guid.Empty;
                            ShopOrderProductionDetailManager.Current.UpdateBoxID(qpgm.ShopOrderProductionDetail.Id, Guid.Empty);

                            qpgm.ShopOrderProductionDetail.ProductionStateID = StaticValues.specialCodeScrap.Id;
                            qpgm.ShopOrderProductionDetail.IfsScrapTypeId = scrapReasons.Id;
                            qpgm.ShopOrderProductionDetail.IfsReported = true;
                            qpgm.ShopOrderProductionDetail.UpdatedAt = DateTime.Now;
                            qpgm.ShopOrderProductionDetail.OperatorNote = note;

                            IfsSendproductionDetails.Add(qpgm);
                        }
                    }
                }
                foreach (var rowHandle in selectedRows)
                {
                    var qpgm = (QuestionableProductGridModel)gridView1.GetRow(rowHandle);

                    if (qpgm.ShopOrderProductionDetail.ProductionStateID == StaticValues.specialCodeScrap.Id)
                    {
                        ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "993", "Bu ürün daha önce şüpheliye ayrılmıştır", "Message"));
                        continue;
                    }
                    var scrapHandlingUnit = ToolsMdiManager.frmOperatorActive.handlingUnits.Where(x => x.Id == qpgm.ShopOrderProductionDetail.BoxID).FirstOrDefault();

                    ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(
                                            ProductionStateID: StaticValues.specialCodeScrap.Id,
                                            IfsScrapTypeId: scrapReasons.Id,
                                            Id: qpgm.Id);
                    ShopOrderProductionDetailManager.Current.UpdateOperatorNote(qpgm.Id, note);

                    if (scrapHandlingUnit != null)
                    {
                        scrapHandlingUnit.Quantity -= qpgm.Quantity;
                        HandlingUnitManager.Current.UpdateHandlingUnitQuantity(scrapHandlingUnit.Id, scrapHandlingUnit.Quantity);
                    }

                    qpgm.ShopOrderProductionDetail.BoxID = Guid.Empty;
                    qpgm.ShopOrderProductionDetail.CustomerBoxID = Guid.Empty;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(qpgm.ShopOrderProductionDetail.Id, Guid.Empty);

                    qpgm.ShopOrderProductionDetail.ProductionStateID = StaticValues.specialCodeScrap.Id;
                    qpgm.ShopOrderProductionDetail.IfsScrapTypeId = scrapReasons.Id;
                    qpgm.ShopOrderProductionDetail.IfsReported = true;
                    qpgm.ShopOrderProductionDetail.UpdatedAt = DateTime.Now;
                    qpgm.ShopOrderProductionDetail.OperatorNote = note;

                    IfsSendproductionDetails.Add(qpgm);
                }
                if (ToolsMdiManager.frmOperatorActive.panelDetail.ScrapLabelPrint)
                {
                    foreach (var qpgm in IfsSendproductionDetails)
                    {
                        ShopOrderProductionDetailHelper.PrintScrapProduction(qpgm.Product, qpgm.ShopOrderProduction, qpgm.ShopOrderGridModel, qpgm.ShopOrderProductionDetail, qpgm.Machine, qpgm.Resource, userModel);
                    }
                }

            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }
        #endregion

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0 && ((QuestionableProductGridModel)gridView1.GetRow(e.RowHandle)).OrderNo != glueShopOrder.Text)
                e.Appearance.BackColor = Color.Aquamarine;
            else
                e.Appearance.BackColor = Color.White;
        }
    }

}
