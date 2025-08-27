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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    /// <summary>
    /// UI for marking selected production details as "questionable product" (şüpheli ürün),
    /// with barcode scanning, manual injection, and printing support.
    /// </summary>
    public partial class ucQuestionableProduct : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly ObservableCollection<int> _selectedRows = new ObservableCollection<int>();
        private readonly List<QuestionableProductGridModel> _ifsSendProductionDetails = new List<QuestionableProductGridModel>();
        private readonly UserModel _userModel;

        private ScrapReasons _scrapReasons;
        private List<QuestionableProductGridModel> _gridModels = new List<QuestionableProductGridModel>();

        private bool _editValueChanged;

        #region Constructor
        /// <summary>
        /// Initializes the control.
        /// </summary>
        public ucQuestionableProduct(UserModel userModel)
        {
            InitializeComponent();
            LanguageHelper.InitializeLanguage(this);

            _userModel = userModel;
            btnManualAddScrapProduct.Visible = ToolsMdiManager.frmOperatorActive.panelDetail.AllowManualScrapInjection;

            WireSelectedRowsCounter();
            ResolveScrapReasonOrAbort();
            PrepareGrid();
            InitProduct();
            InitShopOrder();
        }
        #endregion

        #region Init Helpers
        /// <summary>
        /// Keeps spnQuantity in sync with selected row count and locks it when selection is active.
        /// </summary>
        private void WireSelectedRowsCounter()
        {
            _selectedRows.CollectionChanged += delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    spnQuantity.Value = _selectedRows.Count;
                    spnQuantity.Enabled = false;
                    spnQuantity.ReadOnly = true;
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    spnQuantity.Value = _selectedRows.Count;

                    if (_selectedRows.Count == 0)
                    {
                        spnQuantity.Enabled = true;
                        spnQuantity.ReadOnly = false;
                    }
                }
            };
        }

        /// <summary>
        /// Loads scrap reason configured for "unclear product reject" for current machine.
        /// Shows localized error and exits panel if not present.
        /// </summary>
        private void ResolveScrapReasonOrAbort()
        {
            _scrapReasons = ScrapReasonsManager.Current.GetScrapReasonsByRejectMessage(
                StaticValues.panel.BranchId,
                ToolsMdiManager.frmOperatorActive.machine.Id,
                StaticValues.scrapReasonsUnclearProductRejectMessage);

            if (_scrapReasons == null)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "983",
                    "IFS üzerinde iş merkezi için tanımlı ŞÜPHELİ ÜRÜN bilgisi olmadığı için devam edilemiyor. Lütfen sistem yöneticisine başvurunuz", "Message"));
                ToolsMdiManager.frmOperatorActive.container.Visible = false;
            }
        }

        /// <summary>
        /// Prepares grid datasource and default sort.
        /// </summary>
        private void PrepareGrid()
        {
            _gridModels = _gridModels.OrderByDescending(x => x.Barcode).ToList();

            gridControl1.DataSource = _gridModels;

            var view = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null)
            {
                view.SortInfo.ClearSorting();
                view.SortInfo.Add(view.Columns["Barcode"], DevExpress.Data.ColumnSortOrder.Descending);
            }
        }

        /// <summary>
        /// Initializes product lookup with current operator active products.
        /// </summary>
        private void InitProduct()
        {
            var products = ToolsMdiManager.frmOperatorActive.products;
            glueProduct.Properties.DataSource = products;
        }

        /// <summary>
        /// Initializes shop order lookup and sets default.
        /// </summary>
        private void InitShopOrder()
        {
            try
            {
                var shoporders = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
                glueShopOrder.Properties.DataSource = shoporders;

                var first = shoporders.FirstOrDefault();
                if (first != null)
                    glueShopOrder.EditValue = first.Id;
            }
            catch
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "639",
                    "İş Emri Olmadan Şüpheliye Ayırmak İçin Tamam' a Basın Sadece Barcod Okutarak İşlem Yapabilirsiniz\n", "Message"));
            }
        }
        #endregion

        #region Barcode
        /// <summary>
        /// Enter on barcode textbox: tries select row by Serial or Barcode;
        /// if not found, asks services and adds to grid if valid.
        /// </summary>
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBarcode.Text))
                return;

            if (e.KeyCode != Keys.Enter)
                return;

            var rowHandle = gridView1.LocateByValue("Serial", txtBarcode.Text);
            if (rowHandle >= 0)
            {
                gridView1.SelectRow(rowHandle);
                txtBarcode.SelectAll();
                return;
            }

            rowHandle = gridView1.LocateByValue("Barcode", txtBarcode.Text);
            if (rowHandle >= 0)
            {
                gridView1.SelectRow(rowHandle);
                txtBarcode.SelectAll();
                return;
            }

            // not found in grid: query by barcode/serial
            var pd = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetailByBarcodeOrSerial(txtBarcode.Text);
            if (pd == null)
            {
                var prm = txtBarcode.Text.CreateParameters("@Barcode");
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "984",
                    "@Barcode Barkoduna ait Ürün bulunamadı", "Message"), prm);
                txtBarcode.SelectAll();
                return;
            }

            if (pd.ProductionStateID == StaticValues.specialCodeScrap.Id)
            {
                var prm = txtBarcode.Text.CreateParameters("@Barcode");
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "985",
                    "@Barcode Barkodu daha önce şüpheliye ayrılmıştır", "Message"), prm);
                txtBarcode.SelectAll();
                return;
            }

            if (pd.ProductionStateID == StaticValues.specialCodeNotOk.Id)
            {
                var prm = txtBarcode.Text.CreateParameters("@Barcode");
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "986",
                    "@Barcode Barkodu daha önce hurdaya ayrılmıştır", "Message"), prm);
                txtBarcode.SelectAll();
                return;
            }

            // add to grid
            AddItemToGridControl(
                pd,
                ProductManager.Current.GetProductById(pd.ProductID),
                vw_ShopOrderGridModelManager.Current.GetShopOrderOperationModelById(pd.ShopOrderOperationID),
                ShopOrderProductionManager.Current.GetShopOrderProductionById(pd.ShopOrderProductionID),
                MachineManager.Current.GetMachineById(pd.WorkCenterID),
                MachineManager.Current.GetMachineById(pd.ResourceID));

            gridControl1.RefreshDataSource();
            gridView1.RefreshData();

            var newRowHandle = gridView1.LocateByValue("Serial", pd.serial.ToString());
            if (newRowHandle >= 0)
            {
                gridView1.FocusedRowHandle = newRowHandle;
                gridView1.SelectRow(newRowHandle);
            }

            txtBarcode.SelectAll();
        }
        #endregion

        #region Lookup Events
        private void glueProduct_EditValueChanged(object sender, EventArgs e)
        {
            if (_editValueChanged) return;

            if (_selectedRows.Count > 0 && ConfirmSaveSelectionsAndPersist())
            {
                UpdateActiveShopOrderAndQuantities();
            }

            _selectedRows.Clear();
            spnQuantity.Value = 0;

            _editValueChanged = true;
            var productId = (Guid)glueProduct.EditValue;
            var target = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(p => p.ProductID == productId);
            glueShopOrder.EditValue = target.Id;
            _editValueChanged = false;

            InitShopOrderProductionDetails();
        }

        private void glueShopOrder_EditValueChanged(object sender, EventArgs e)
        {
            if (_editValueChanged) return;

            if (_selectedRows.Count > 0 && ConfirmSaveSelectionsAndPersist())
            {
                UpdateActiveShopOrderAndQuantities();
            }

            _selectedRows.Clear();
            spnQuantity.Value = 0;

            _editValueChanged = true;
            var shopOrderId = (Guid)glueShopOrder.EditValue;
            var target = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.First(s => s.Id == shopOrderId);
            glueProduct.EditValue = target.ProductID;
            _editValueChanged = false;

            InitShopOrderProductionDetails();
        }

        /// <summary>
        /// Shows confirm dialog and persists current selection as questionable.
        /// </summary>
        private bool ConfirmSaveSelectionsAndPersist()
        {
            if (!ToolsMessageBox.Question(this, MessageTextHelper.GetMessageText("000", "987",
                    "Seçimleri kaydetmek istiyor musunuz?", "Message")))
                return false;

            var frm = new FrmNote();
            if (frm.ShowDialog() != DialogResult.OK)
                return false;

            SetProductionDetailScrap(frm.GetNote());
            return true;
        }

        /// <summary>
        /// Updates active SO context and quantities after save.
        /// </summary>
        private void UpdateActiveShopOrderAndQuantities()
        {
            var sel = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels
                .First(x => x.Id == (Guid)glueShopOrder.EditValue);
            ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = sel;
            ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
            ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)glueShopOrder.EditValue);
        }
        #endregion

        #region Grid Init/Bind
        /// <summary>
        /// Binds OK-state details for selected shop order/product to grid.
        /// </summary>
        private void InitShopOrderProductionDetails()
        {
            if (glueShopOrder.EditValue == null || string.IsNullOrEmpty(glueShopOrder.Text))
                return;

            var result = ToolsMdiManager.frmOperatorActive.productionDetails
                .Where(p => p.ShopOrderOperationID == (Guid)glueShopOrder.EditValue
                         && p.ProductID == (Guid)glueProduct.EditValue
                         && p.ProductionStateID == StaticValues.specialCodeOk.Id)
                .OrderBy(p => p.serial)
                .ToList();

            _gridModels.Clear();
            _selectedRows.Clear();
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
            gridView1.RefreshData();
        }

        /// <summary>
        /// Adds a row item (projection) into the grid list.
        /// </summary>
        private void AddItemToGridControl(
            ShopOrderProductionDetail item,
            Product product,
            vw_ShopOrderGridModel sogm,
            ShopOrderProduction sop,
            Machine machine,
            Machine resource)
        {
            var model = new QuestionableProductGridModel
            {
                ShopOrderProductionDetail = item,
                Product = product,
                ShopOrderProduction = sop,
                ShopOrderGridModel = sogm,
                Machine = machine,
                Resource = resource
            };

            _gridModels.Add(model);
        }
        #endregion

        #region Grid Events
        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                spnQuantity.Value += 1;
                _selectedRows.Add(e.ControllerRow);
            }
            else if (e.Action == CollectionChangeAction.Remove)
            {
                spnQuantity.Value -= 1;
                _selectedRows.Remove(e.ControllerRow);
            }
            else if (e.Action == CollectionChangeAction.Refresh)
            {
                var rows = gridView1.GetSelectedRows();
                _selectedRows.Clear();
                if (rows.Length != 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        _selectedRows.Add(rows[i]);
                }
            }
        }
        #endregion

        #region Quantity Spin
        private void spnQuantity_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            decimal value;
            if (!decimal.TryParse(Convert.ToString(e.NewValue), out value))
            {
                var prm = Convert.ToString(e.NewValue).CreateParameters("@Value");
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "951",
                    "@Value geçerli bir değer değil", "Message"), prm);
                e.Cancel = true;
                return;
            }

            if (value < 0)
            {
                e.Cancel = true;
                return;
            }

            btnSave.Enabled = value > 0;
        }
        #endregion

        #region Buttons
        private void btnSave_Click(object sender, EventArgs e)
        {
            // sync _selectedRows with current grid selection
            _selectedRows.Clear();
            foreach (var idx in gridView1.GetSelectedRows())
                _selectedRows.Add(idx);

            if (spnQuantity.Value > 0)
            {
                // guard: shop order required when saving by quantity path
                if (!HasActiveShopOrderForQuantityPath())
                    return;

                var frm = new FrmNote();
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                SetProductionDetailScrap(frm.GetNote());

                if (glueShopOrder.EditValue != null)
                {
                    UpdateActiveShopOrderAndQuantities();
                }
            }

            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private bool HasActiveShopOrderForQuantityPath()
        {
            if (ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.Count() == 0
                || (QuestionableProductGridModel)gridView1.GetRow(0) == null)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "638",
                    "Herhangi Bir İş Emri Seçili Olmadığı İçin Miktar Girerek İşlem Yapamazsınız.\n", "Message"));
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void btnManualAddScrapProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(glueShopOrder.Text))
            {
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "988",
                    "Şüpheli ürün eklemeden önce ilgili iş emrini seçmelisiniz", "Message"));
                return;
            }

            var prm = glueShopOrder.Text.CreateParameters("@ShopOrder");
            if (!ToolsMessageBox.Question(this, MessageTextHelper.GetMessageText("000", "989",
                "@ShopOrder numaralı iş emrine manuel olarak şüpheli ürün eklemek istiyor musunuz?", "Message"), prm))
                return;

            var prompt = MessageTextHelper.GetMessageText("000", "612", "Miktar bilgisi girmelisiniz", "Message");
            using (var frm = new FrmNumericKeyboard(prompt))
            {
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                if (frm.value <= 0)
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "991",
                        "Şüpheli ürün miktarı sıfırdan büyük olmalıdır!", "Message"));
                    return;
                }

                var selectedModel = (vw_ShopOrderGridModel)glueShopOrder.GetSelectedDataRow();
                if (frm.value >= selectedModel.revisedQtyDue)
                {
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "894",
                        "Şüpheli ürün miktarı iş emri miktarından az olmalıdır!", "Message"));
                    return;
                }

                var product = (Product)glueProduct.GetSelectedDataRow();

                // create questionable (scrap) detail
                var detail = ShopOrderProductionDetailHelper.CreateAndInsertScrapProductionDetail(
                    ToolsMdiManager.frmOperatorActive, selectedModel, product, true, _userModel, 1, frm.value, frm.value, true);

                ToolsMdiManager.frmOperatorActive.productionDetails.Add(detail);

                ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(
                    StaticValues.specialCodeScrap.Id, _scrapReasons.Id, detail.Id);

                detail.ProductionStateID = StaticValues.specialCodeScrap.Id;
                detail.IfsScrapTypeId = _scrapReasons.Id;
                detail.IfsReported = true;
                detail.UpdatedAt = DateTime.Now;

                ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive = selectedModel;
                ToolsMdiManager.frmOperatorActive.SetLabelsTextValue();
                ToolsMdiManager.frmOperatorActive.UpdateQuantities((Guid)selectedModel.Id);

                prm.Add("@Amount", frm.value);
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "990",
                    "@ShopOrder numaralı iş emrine @Amount şüpheli ürün eklenmiştir", "Message"), prm);

                ToolsMdiManager.frmOperatorActive.container.Visible = false;
            }
        }

        private void btnSplitProductionDetail_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;

            var row = (QuestionableProductGridModel)gridView1.GetRow(gridView1.FocusedRowHandle);
            if (row.Unit == Units.ad.ToText() && row.Quantity < 2)
            {
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "992",
                    "Şüpheli ürün ayrıştırabilmek için adet miktarı 1'den büyük olmalıdır!", "Message"));
                return;
            }

            var frm = new FrmSplitProductDetail(row);
            if (frm.ShowDialog() == DialogResult.OK)
                InitShopOrderProductionDetails();
        }
        #endregion

        #region Persist "Questionable" (Scrap) Flag
        /// <summary>
        /// Sets the selected rows (or last N units path) to "scrap/questionable" and prints labels if configured.
        /// </summary>
        private void SetProductionDetailScrap(string note)
        {
            try
            {
                _ifsSendProductionDetails.Clear();

                // PATH A: quantity entered (no selection) + shop order present
                if (_selectedRows.Count == 0 && glueShopOrder.EditValue != null)
                {
                    if (!TryMarkByQuantityPath(note))
                        return;
                }

                // PATH B: selected rows in grid
                foreach (var rowHandle in _selectedRows)
                {
                    var qpgm = (QuestionableProductGridModel)gridView1.GetRow(rowHandle);
                    if (qpgm == null) continue;

                    if (qpgm.ShopOrderProductionDetail.ProductionStateID == StaticValues.specialCodeScrap.Id)
                    {
                        ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "993",
                            "Bu ürün daha önce şüpheliye ayrılmıştır", "Message"));
                        continue;
                    }

                    ApplyQuestionableFlagsAndPersist(qpgm, note);
                    _ifsSendProductionDetails.Add(qpgm);
                }

                // Print labels if required
                if (ToolsMdiManager.frmOperatorActive.panelDetail.ScrapLabelPrint)
                {
                    foreach (var qpgm in _ifsSendProductionDetails)
                    {
                        ShopOrderProductionDetailHelper.PrintScrapProduction(
                            qpgm.Product, qpgm.ShopOrderProduction, qpgm.ShopOrderGridModel,
                            qpgm.ShopOrderProductionDetail, qpgm.Machine, qpgm.Resource, _userModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        /// <summary>
        /// Applies database and in-memory changes for a single row being marked as questionable.
        /// </summary>
        private void ApplyQuestionableFlagsAndPersist(QuestionableProductGridModel qpgm, string note)
        {
            var selected = qpgm.ShopOrderProductionDetail;

            // Update DB flags
            ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(
                StaticValues.specialCodeScrap.Id,
                _scrapReasons.Id,
                qpgm.Id);

            ShopOrderProductionDetailManager.Current.UpdateOperatorNote(qpgm.Id, note);

            // If this came from a handling unit, decrement and persist
            var scrapHandlingUnit = ToolsMdiManager.frmOperatorActive.handlingUnits
                .FirstOrDefault(x => x.Id == selected.BoxID);

            if (scrapHandlingUnit != null)
            {
                scrapHandlingUnit.Quantity -= qpgm.Quantity;
                HandlingUnitManager.Current.UpdateHandlingUnitQuantity(scrapHandlingUnit.Id, scrapHandlingUnit.Quantity);
            }

            // Clear box references (decouple from HU)
            selected.BoxID = Guid.Empty;
            selected.CustomerBoxID = Guid.Empty;
            ShopOrderProductionDetailManager.Current.UpdateBoxID(selected.Id, Guid.Empty);

            // Mirror state in-memory
            selected.ProductionStateID = StaticValues.specialCodeScrap.Id;
            selected.IfsScrapTypeId = _scrapReasons.Id;
            selected.IfsReported = true;
            selected.UpdatedAt = DateTime.Now;
            selected.OperatorNote = note;
        }

        /// <summary>
        /// Handles the "last N units" path when a user enters a quantity instead of selecting rows.
        /// Splits the last unit if needed to match the exact entered quantity.
        /// </summary>
        private bool TryMarkByQuantityPath(string note)
        {
            decimal enteredQty;
            if (!decimal.TryParse(spnQuantity.Value.ToString(), out enteredQty))
            {
                var prm = decimal.Parse(spnQuantity.Value.ToString()).CreateParameters("@Value");
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "951",
                    "@Value geçerli bir değer değil", "Message"), prm);
                return false;
            }

            var shopOrderId = (Guid)glueShopOrder.EditValue;
            var okItemsQuery = ToolsMdiManager.frmOperatorActive.productionDetails.Where(p =>
                p.ShopOrderOperationID == shopOrderId &&
                p.ProductionStateID == StaticValues.specialCodeOk.Id);

            var totalOkQty = okItemsQuery.Sum(x => x.Quantity);
            if (enteredQty > totalOkQty)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "975",
                    "Online kontrolde ilgili malzemeden yeterli miktarda ürün bulunamadı", "Message"));
                return false;
            }

            var selectedLastList = okItemsQuery
                .OrderByDescending(x => x.CreatedAt)
                .Take((int)spnQuantity.Value)
                .ToList();

            var sumQuantity = selectedLastList.Sum(x => x.Quantity);

            // If sum of last N doesn't exactly equal enteredQty, split the last unit to match enteredQty exactly.
            if (sumQuantity != enteredQty)
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

                    if (listLastSum > enteredQty)
                    {
                        surplus = listLastSum - enteredQty;
                        lastProdDetail = item;

                        if (lastProdDetail.Quantity > surplus)
                        {
                            if (enteredQty <= 0)
                            {
                                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "950",
                                    "Geçerli bir değer girmelisiniz", "Message"));
                                return false;
                            }

                            // Build split detail from the last
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

            if (selectedLastList != null && selectedLastList.Count > 0)
            {
                foreach (var selected in selectedLastList)
                {
                    int rowHandle = gridView1.LocateByValue("Id", selected.Id);
                    if (rowHandle < 0)
                        continue;

                    var qpgm = (QuestionableProductGridModel)gridView1.GetRow(rowHandle);
                    if (qpgm == null)
                        continue;

                    var scrapHandlingUnit = ToolsMdiManager.frmOperatorActive.handlingUnits
                        .Where(x => x.Id == selected.BoxID).FirstOrDefault();

                    // Update DB
                    ShopOrderProductionDetailManager.Current.UpdateProductionStateScrapTypeIDIfsSend(
                        ProductionStateID: StaticValues.specialCodeScrap.Id,
                        IfsScrapTypeId: _scrapReasons.Id,
                        Id: qpgm.Id);
                    ShopOrderProductionDetailManager.Current.UpdateOperatorNote(qpgm.Id, note);

                    // HU adjust
                    if (scrapHandlingUnit != null)
                    {
                        scrapHandlingUnit.Quantity -= qpgm.Quantity;
                        HandlingUnitManager.Current.UpdateHandlingUnitQuantity(scrapHandlingUnit.Id, scrapHandlingUnit.Quantity);
                    }

                    // Clear HU references
                    qpgm.ShopOrderProductionDetail.BoxID = Guid.Empty;
                    qpgm.ShopOrderProductionDetail.CustomerBoxID = Guid.Empty;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(qpgm.ShopOrderProductionDetail.Id, Guid.Empty);

                    // Mirror state in-memory
                    qpgm.ShopOrderProductionDetail.ProductionStateID = StaticValues.specialCodeScrap.Id;
                    qpgm.ShopOrderProductionDetail.IfsScrapTypeId = _scrapReasons.Id;
                    qpgm.ShopOrderProductionDetail.IfsReported = true;
                    qpgm.ShopOrderProductionDetail.UpdatedAt = DateTime.Now;
                    qpgm.ShopOrderProductionDetail.OperatorNote = note;

                    _ifsSendProductionDetails.Add(qpgm);
                }
            }

            return true;
        }
        #endregion

        #region Row Style
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0 && ((QuestionableProductGridModel)gridView1.GetRow(e.RowHandle)).OrderNo != glueShopOrder.Text)
                e.Appearance.BackColor = Color.Aquamarine;
            else
                e.Appearance.BackColor = Color.White;
        }
        #endregion
    }
}
