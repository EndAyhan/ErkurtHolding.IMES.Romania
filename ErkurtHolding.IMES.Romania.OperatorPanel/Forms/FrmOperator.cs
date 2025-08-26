using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGauges.Core.Drawing;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.KafkaManager;
using ErkurtHolding.IMES.Business.ReportManagers;
using ErkurtHolding.IMES.Business.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.QueryModel;
using ErkurtHolding.IMES.Entity.Reports;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.GridModels;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using ErkurtHolding.IMES.Romania.OperatorPanel.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using NotificationHelper = ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.NotificationHelper;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmOperator : DevExpress.XtraEditors.XtraForm
    {
        public PanelDetail panelDetail { get; set; }
        public bool restart { get; set; } = false;
        public InterruptionGridModel interruptionGridModel { get; set; }
        public ShopOrderProduction shopOrderProduction { get; set; }
        public ProcessHandlingUnitModel halfHandlingUnitProcess { get; set; } = null;
        public UserModel manuelLabelUser { get; set; }
        public ShopOrderProductionDetail shopOrderProductionDetailPrescriptionControlCount { get; set; }
        public decimal _manualInputBySquareMeters { get; set; }
        public decimal _manuelInputByKilogram { get; set; }
        public string _SquareMetersDescription { get; set; }
        public string _PartiNoKilogramDescription { get; set; }
        public Machine machine { get; set; }
        public ResourceStatus resourceStatus { get; set; }
        public UserModel machineDownFinishUser { get; set; }
        public ShopOrderProductionDetail ShopOrderProductionDetailPrescriptionControlCount { get; set; }
        public UserModel machinePeriyodicStartUser { get; set; }
        public MaintenanceDetail PrMaintenanceActive { get; set; }
        public UserModel machindeDownStartUser { get; set; }
        public OpInterruptionCause opInterruptionCause { get; set; }
        public UserModel interrupstionCouseStartUser { get; set; }


        public ObservableCollection<OpcOtherReadModel> opcOtherReadModels { get; set; } = new ObservableCollection<OpcOtherReadModel>();
        public List<PrintLabelModel> printLabelModels { get; set; } = new List<PrintLabelModel>();
        public List<StockPopupValues> sockets { get; set; } = new List<StockPopupValues>();
        public List<ShopOrderProductionDetail> productionDetailsByProducts { get; set; } = new List<ShopOrderProductionDetail>();
        public List<ShopOrderProductionDetail> shopOrderProductionDetails { get; set; } = new List<ShopOrderProductionDetail>();
        public List<ShopOrderOperation> shopOrderOperations { get; set; } = new List<ShopOrderOperation>();
        public List<HandlingUnit> handlingUnits { get; set; } = new List<HandlingUnit>();
        public List<Product> products { get; set; } = new List<Product>();
        public List<Product> byProducts { get; set; } = new List<Product>();
        public List<ShopOrderProductionDetail> productionDetails { get; set; } = new List<ShopOrderProductionDetail>();
        public List<PartHandlingUnit> partHandlingUnits { get; set; } = new List<PartHandlingUnit>();
        public List<PartHandlingUnit> partHandlingUnitsByProduct { get; set; } = new List<PartHandlingUnit>();
        public Guid SubcontractorShopOrderProductionId { get; set; } = Guid.Empty;
        public Guid SubcontractorParHandlingUnitID { get; set; } = Guid.Empty;
        public vw_ShopOrderGridModel SubcontractorShopOrder { get; set; } = null;
        public List<ShopOrderProduction> exShopOrderProductions { get; set; } = new List<ShopOrderProduction>();
        public List<IssueMaterialAllocModel> issueMaterialAllocModels { get; set; } = new List<IssueMaterialAllocModel>();
        public List<UserModel> faultUserModels { get; set; } = new List<UserModel>();
        public List<MaintenanceMain> PrMaintenance { get; set; } = new List<MaintenanceMain>();

        private static BrushObject blueBO = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:DodgerBlue");
        private static BrushObject yellowBO = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:Goldenrod");
        private static BrushObject redBO = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:OrangeRed");
        private Color greenC = Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
        private Color yellowC = Color.Goldenrod;
        private Color redC = Color.OrangeRed;
        private Color grayC = Color.Gray;
        public ObservableCollection<UserModel> Users = new ObservableCollection<UserModel>();
        public Dictionary<Guid, ProcessHandlingUnitModel> halfHandlingUnit = new Dictionary<Guid, ProcessHandlingUnitModel>();
        public bool squareMeterPlcControl = true;
        public bool kilogramPlcControl = false;
        public bool pokayokeFlag = false;
        private BackgroundWorker BgWorker;
        int factorCounter = 0;
        TimeSpan ts;
        RealTimeSource rtsInterruptionGrid;
        public string currentOtpCode;
        public DateTime otpExpirationTime;


        public FrmOperator()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }



        private Machine _resource;
        public Machine resource
        {
            get
            {
                return _resource;
            }
            set
            {
                if (value.Definition == value.resourceName)
                    this.Text = $"{value.Definition}";
                else
                    this.Text = $"{value.Definition} - {value.resourceName}";

                _resource = value;
            }
        }

        private ObservableCollection<Fault> _faults;
        public ObservableCollection<Fault> faults
        {
            get
            {
                if (_faults == null)
                {
                    _faults = new ObservableCollection<Fault>();
                }

                return _faults;
            }
            set
            {
                _faults = value;
                gcFaults.DataSource = faults;
            }
        }

        private vw_ShopOrderGridModel _vw_ShopOrderGridModelActive;
        public vw_ShopOrderGridModel vw_ShopOrderGridModelActive
        {
            get
            {
                if (_vw_ShopOrderGridModelActive == null)
                    _vw_ShopOrderGridModelActive = new vw_ShopOrderGridModel();

                return _vw_ShopOrderGridModelActive;
            }
            set
            {
                if (value == null)
                    value = new vw_ShopOrderGridModel();
                _vw_ShopOrderGridModelActive = value;
            }
        }

        private List<vw_ShopOrderGridModel> _vw_ShopOrderGridModels;
        public List<vw_ShopOrderGridModel> vw_ShopOrderGridModels
        {
            get
            {
                if (_vw_ShopOrderGridModels == null)
                    _vw_ShopOrderGridModels = new List<vw_ShopOrderGridModel>();
                return _vw_ShopOrderGridModels;
            }
            set
            {
                if (value != null)
                {
                    gcWorkShopOrder.DataSource = value;
                    if (value.Count != 0)
                    {
                        value.ForEach(x => { x.opStartDate = DateTime.Now; });
                        vw_ShopOrderGridModelActive = value.OrderByDescending(x => x.opStartDate).First();
                        tmrWorkShopOrder.Start();
                    }
                }

                _vw_ShopOrderGridModels = value;
            }
        }

        private InterruptionCause _interruptionCause;
        public InterruptionCause interruptionCause
        {
            get
            {
                return _interruptionCause;
            }
            set
            {
                ToolsMdiManager.InterrutionImage(this, value != null);
                _interruptionCause = value;
            }
        }

        private bool _causeInterruptionWaiting;
        public bool causeInterruptionWaiting
        {
            get { return _causeInterruptionWaiting; }
            set
            {
                if (value)
                    InsertWaitingDurationInterruptionCouse();//Bekleme duruşu başlatma
                else
                    UpdateWaitingDurationInterruptionCouse();//Bekleme Duruşu sonlandırma


                _causeInterruptionWaiting = value;
            }
        }

        private ScrapReasons _scrapReason;
        public ScrapReasons scrapReason
        {
            get
            {
                if (_scrapReason == null)
                    _scrapReason = ScrapReasonsManager.Current.GetScrapReasonsByRejectMessage(
                StaticValues.panel.BranchId,
                machine.Id,
                StaticValues.scrapReasonsUnclearProductRejectMessage);

                return _scrapReason;
            }
        }

        private List<ShopOrderProductionDetail> _exShopOrderProductionDetails;
        public List<ShopOrderProductionDetail> exShopOrderProductionDetails
        {
            get
            {
                if (_exShopOrderProductionDetails == null)
                {
                    if (vw_ShopOrderGridModels.HasEntries())
                    {
                        _exShopOrderProductionDetails = ShopOrderProductionDetailManager.Current.GetExShopOrderProductionByShopOrderID(vw_ShopOrderGridModels.Select(x => x.Id).ToList<Guid>());
                    }
                }
                return _exShopOrderProductionDetails;
            }
            set
            {
                _exShopOrderProductionDetails = value;
            }
        }

        private int _targetQty = 0;
        public int TargetQty
        {
            get
            {
                if (_targetQty == 0)
                {
                    double maxMachRunFactor = 0;
                    if (shopOrderProduction != null /*&& shopOrderStatus == ShopOrderStatus.End*/)
                    {
                        try
                        {
                            maxMachRunFactor = vw_ShopOrderGridModels.Max(x => x.machRunFactor) * 60;

                            DateTime start = shopOrderProduction.OrderStartDate;
                            DateTime end = new DateTime(start.Year, start.Month, start.Day, StaticValues.shift.EndDate.Hour, StaticValues.shift.EndDate.Minute, StaticValues.shift.EndDate.Second);
                            if (start > end)
                                end = end.AddDays(1);
                            double totalMinutes = (end - start).TotalMinutes;
                            _targetQty = (int)Math.Floor(totalMinutes / maxMachRunFactor);
                        }
                        catch
                        {
                        }
                    }
                }
                return _targetQty;
            }
            set
            {
                _targetQty = value;
            }
        }

        private List<ProdStructAlternate> _prodStructAlternates;
        public List<ProdStructAlternate> prodStructAlternates
        {
            get
            {
                if (_prodStructAlternates == null)
                    _prodStructAlternates = new List<ProdStructAlternate>();
                return _prodStructAlternates;
            }
            set
            {
                if (value == null)
                {
                    _prodStructAlternates = new List<ProdStructAlternate>();
                    byProducts.Clear();
                    partHandlingUnitsByProduct.Clear();
                }
                else
                {
                    byProducts.Clear();
                    partHandlingUnitsByProduct.Clear();
                    foreach (var item in value)
                    {
                        var p = ProductManager.Current.GetProductById(item.ComponentPartID);
                        byProducts.Add(p);

                        var phs = PartHandlingUnitManager.Current.GetPartHandlingUnit(p.PartNo);
                        if (phs.HasEntries() && phs.Count > 1)
                        {
                            FrmPartHandlingUnitSelect frm = new FrmPartHandlingUnitSelect(phs);
                            if (frm.ShowDialog() == DialogResult.OK)
                                partHandlingUnitsByProduct.Add(frm.partHandlingUnit);
                        }
                        else if (phs.HasEntries() && phs.Count == 1)
                            partHandlingUnitsByProduct.Add(phs[0]);
                        else
                        {
                            var prm = p.PartNo.CreateParameters("@PartNo");
                            prm.Add("@Description", p.Description);
                            ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "913", "@PartNo - @Description için IFS üzerinde taşıma kasası tanımlı değildir", "Message"), prm);
                        }
                    }
                }

                _prodStructAlternates = value;
            }
        }

        #region FORM STATUSES
        private ShopOrderStatus _shopOrderStatus;
        public ShopOrderStatus shopOrderStatus
        {
            get
            {
                return _shopOrderStatus;
            }
            set
            {
                DateTime dt = DateTime.Now;

                switch (value)
                {
                    case ShopOrderStatus.Start://uygulamanın ilk açılması ve 
                        break;
                    case ShopOrderStatus.End://setup bitiyor ve üretime başlıyor
                        if (!restart)
                        {
                            shopOrderProduction.SetupFinishDate = dt;
                            ShopOrderProductionManager.Current.SetShopOrderProductionSetupFinishDate(shopOrderProduction.Id, dt);
                        }
                        else
                        {
                            if (shopOrderProductionDetails.Count > 0)
                            {
                                dt = shopOrderProductionDetails.Max(x => x.EndDate);
                            }
                        }

                        CreateShopOrderProductionDetails();
                        shopOrderProductionDetails.ForEach(x => { x.StartDate = dt; });

                        SetMachineStateColor(MachineStateColor.Run);
                        //Session probleminden sonra handlechange içerisine alınd
                        //StaticValues.opcClient.MachineLock(panelDetail.OPCNodeIdMachineControl, false);
                        StaticValues.opcClient.MachineLock(panelDetail.OPCNodeIdMachineControl, false);

                        CheckIfHalfFilledHandlingUnitsExistInShopOrder();

                        break;
                    case ShopOrderStatus.StartProduction://Setup başlangıcı
                        try
                        {
                            lblWorkCenterStartTime.Text = dt.ToString();
                            if (!restart)
                            {
                                if (panelDetail.Scales)
                                {
                                    //FrmScales frm = new FrmScales(vw_ShopOrderGridModels);
                                    //if (frm.ShowDialog() == DialogResult.OK)
                                    //{
                                    //}
                                }
                                else
                                {
                                    container.Tag = ContainerSelectUserControl.SetupCheckList;
                                    container.Visible = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "914", "IFS Soap Servisine Ulaşılamadı", "Message"), ex);
                        }
                        break;
                }
                ToolsRibbonManager.RibbonButtonStatus(value);

                _shopOrderStatus = value;
            }
        }

        private InterruptionCauseOptions _interruptionCauseOptions;
        public InterruptionCauseOptions interruptionCauseOptions
        {
            get
            {
                return _interruptionCauseOptions;
            }
            set
            {
                switch (value)
                {
                    case InterruptionCauseOptions.Start:
                        if (!restart)
                        {
                            container.Tag = ContainerSelectUserControl.InterruptionCause;
                            container.Visible = true;
                            ToolsRibbonManager.RibbonButtonStatus(value);
                        }
                        break;
                    case InterruptionCauseOptions.Waiting:
                        if (ToolsMdiManager.frmOperatorActive == this)
                            ToolsRibbonManager.RibbonButtonStatus(value);
                        container.Tag = ContainerSelectUserControl.InterruptionCauseDuration;
                        container.Visible = true;
                        SetMachineStateColor(MachineStateColor.MachineDown);

                        if (!interruptionCause.OnlineState)//İŞ Emri seçilmedi duruşunu kapat
                            causeInterruptionWaiting = false;
                        break;

                    case InterruptionCauseOptions.AutoMaintenance:
                        if (ToolsMdiManager.frmOperatorActive == this)
                            ToolsRibbonManager.RibbonButtonStatus(InterruptionCauseOptions.Start);
                        container.Tag = ContainerSelectUserControl.MachineAutoMaintanenceCheckList;
                        container.Visible = true;
                        //SetMachineStateColor(MachineStateColor.Setup);

                        break;

                    case InterruptionCauseOptions.End:

                        if (!interruptionCause.OnlineState)//İŞ Emri seçilmedi duruşunu Başlat
                            causeInterruptionWaiting = true;

                        foreach (var item in interruptionGridModel.interruptionCauseGridModels)
                        {
                            if (item.Id == interruptionCause.Id && item.ResourceID == resource.Id)
                            {
                                item.InterruptionFinishDate = DateTime.Now;
                                item.State = false;
                            }
                        }
                        interruptionGridModel.changeData = true;
                        //rtsInterruptionGrid.Suspend();
                        gvInterruption.BeginDataUpdate();
                        gvInterruption.EndDataUpdate();
                        //rtsInterruptionGrid.Resume();
                        //((PLinqInstantFeedbackSource)gridControl1.ItemsSource).Refresh();
                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => Refresh()), DispatcherPriority.ApplicationIdle);

                        interruptionCause = null;
                        container.Visible = false;
                        if (ToolsMdiManager.frmOperatorActive == this)
                            ToolsRibbonManager.RibbonButtonStatus(value);
                        SetMachineStateColor(MachineStateColor.Run);
                        if (!tmrWorkShopOrder.Enabled) tmrWorkShopOrder.Enabled = true;
                        break;
                }

                _interruptionCauseOptions = value;
            }
        }

        private MachineDownTimeButtonStatus _machineDownTimeButtonStatus;
        public MachineDownTimeButtonStatus machineDownTimeButtonStatus
        {
            get { return _machineDownTimeButtonStatus; }
            set
            {
                switch (value)
                {
                    case MachineDownTimeButtonStatus.Start:
                        container.Visible = false;
                        SetMachineStateColor(MachineStateColor.Run);
                        ToolsMdiManager.InterrutionImage(this, false);
                        break;
                    case MachineDownTimeButtonStatus.InterventionStart:
                        container.Tag = ContainerSelectUserControl.MachineDown;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case MachineDownTimeButtonStatus.Waiting:
                        container.Tag = ContainerSelectUserControl.MachineDownDuration;
                        container.Visible = true;
                        SetMachineStateColor(MachineStateColor.Fault);
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case MachineDownTimeButtonStatus.InterventionStop:
                        container.Tag = ContainerSelectUserControl.MachineDownMaintanenceStart;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case MachineDownTimeButtonStatus.BarBtnQualtyApproved:
                        break;
                    case MachineDownTimeButtonStatus.End:
                        container.Tag = ContainerSelectUserControl.MachineDownStop;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case MachineDownTimeButtonStatus.Cancel:
                        container.Visible = false;
                        SetMachineStateColor(MachineStateColor.Run);
                        ToolsMdiManager.InterrutionImage(this, false);
                        break;
                    default:
                        break;
                }
                ToolsRibbonManager.RibbonButtonStatus(value);
                _machineDownTimeButtonStatus = value;
            }
        }

        private PrMaintenanceButtonStatus _prMaintenanceButtonStatus;
        public PrMaintenanceButtonStatus prMaintenanceButtonStatus
        {
            get { return _prMaintenanceButtonStatus; }
            set
            {
                container.Visible = false;
                switch (value)
                {
                    case PrMaintenanceButtonStatus.Start:
                        container.Visible = false;
                        SetMachineStateColor(MachineStateColor.Run);
                        ToolsMdiManager.InterrutionImage(this, false);
                        break;
                    case PrMaintenanceButtonStatus.InterventionStart:
                        container.Tag = ContainerSelectUserControl.PrMaintenance;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case PrMaintenanceButtonStatus.InterventionStop:
                        container.Tag = ContainerSelectUserControl.PrMaintenanceStart;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case PrMaintenanceButtonStatus.End:
                        container.Tag = ContainerSelectUserControl.PrMaintenanceFinish;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, true);
                        break;
                    case PrMaintenanceButtonStatus.FinishMaintenance:
                        container.Tag = ContainerSelectUserControl.PrMaintenance;
                        container.Visible = true;
                        ToolsMdiManager.InterrutionImage(this, false);
                        break;
                    default:
                        break;
                }
                ToolsRibbonManager.RibbonButtonStatus(value);
                _prMaintenanceButtonStatus = value;
            }
        }
        #endregion

        #region FORM LOAD
        private void FrmOperator_Load(object sender, EventArgs e)
        {
            if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id || panelDetail.ProcessBarcodeControl || panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id || panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeSupplierPark.Id)
            {
                lblProcessBarcode.Visible = true;
                txtBarcode.Visible = true;
                lblValue9.Text = "0";
                lblDescription9.Text = MessageTextHelper.GetMessageText("000", "893", "Okutulan Ürün Sayısı", "Message");
            }
        }
        #endregion

        #region counter
        private decimal _counter;
        private decimal lastCounterValue = -1;
        public decimal counter
        {
            get
            {
                return _counter;
            }
            set
            {
                try
                {
                    if (!tmrWorkShopOrder.Enabled)
                        tmrWorkShopOrder.Enabled = true;
                    if (value == -1)
                    {
                        _counter = 0;
                        lastCounterValue = -1;

                        lblTotalProductionCount.Text = "0";
                        lblRealizeAmount.Text = "0";
                        lblPLC.Text = "0";
                        lblCurrentAmount.Text = "0";
                        lblScrapCount.Text = "0";
                        lblBoxAmount.Text = "0";
                        lblValue9.Text = "0";

                        return;
                    }
                    // Eğer değer değişmemişse çık
                    if (value == lastCounterValue)
                        return;

                    // Cache'i güncelle
                    lastCounterValue = value;

                    DateTime dt = DateTime.Now;
                    if (vw_ShopOrderGridModels == null || vw_ShopOrderGridModels.Count == 0 || shopOrderStatus != ShopOrderStatus.End)
                        return;

                    //if (products.First().unitMeas == Units.ad.DescriptionAttr())
                    //{
                    var count = value - _counter;
                    if (count <= 0)
                        return;

                    _counter = value;

                    SetLabelsTextValue();

                    //recete kontrolden sonra eger counter gelirse islem yapması saglandı
                    if (shopOrderProductionDetailPrescriptionControlCount != null)
                    {
                        int prescriptionControlCount = shopOrderProductionDetailPrescriptionControlCount.PrescriptionControlCount + 1;
                        ShopOrderProductionDetailManager.Current.UpdatePrescriptionControlCount(shopOrderProductionDetailPrescriptionControlCount.Id, prescriptionControlCount);
                        shopOrderProductionDetailPrescriptionControlCount = null;
                    }

                    if (products.First().unitMeas == Units.m2.ToText() && squareMeterPlcControl)
                    {
                        squareMeterPlcControl = false;
                        if (_manualInputBySquareMeters > 0)
                            InsertShopOrderProductionDetailM2(dt);
                    }
                    else if (products.First().unitMeas == Units.kg.ToText())
                    {
                        if (kilogramPlcControl)
                        {
                            kilogramPlcControl = false;
                            if (_manuelInputByKilogram > 0)
                                InsertShopOrderProductionDetailKilogram(dt);
                        }
                        else
                        {
                            InsertShopOrderProductionDetailKilogramPLC(dt);
                        }
                    }
                    else if (products.First().unitMeas == Units.ad.ToText())
                    {
                        for (int i = 1; i <= count; i++)
                        {


                            if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id)
                            {
                                InsertShopOrderProductionDetailPress(dt);
                            }
                            //else if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeSupplierPark.Id)
                            //{
                            //    InsertShopOrderProductionDetailSupplierPark(dt);
                            //}
                            else if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypePlcBarcode.Id)//Önce PLC sonra barkod okutma
                            {

                            }
                            else if (panelDetail.PrintProductBarcode == false && panelDetail.ProcessBarcode == false)//mamül ve proses etiketi almıyor  ise tek satır kaydet ve çarpan yaz
                            {
                                InsertShopOrderProductionDetailNoBarcode(dt);
                            }
                            else
                            {
                                if (processNewActive)
                                    InsertShopOrderProductionDetailProcess(dt);
                                else
                                    InsertShopOrderProductionDetail(dt);
                            }
                            dt = DateTime.Now;
                        }
                    }

                    //for (int i = 1; i <= count; i++)
                    //{
                    //    ByProductInsert(dt);
                    //}

                    CheckOrderQuantityAndAutoFinishOrder();
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region START / STOP / CHANGE ORDER
        private void InitializeShopOrder(UserModel userModel, List<vw_ShopOrderGridModel> selectShopOrders)
        {
            //iş emri operasyon bilgisini plcye göndermek için kullanılır birkez gönderilir ve sonra false olur
            bool flag = true;
            //product Alan 16 Flag : makineye reçete bilgisi göndermenin aktifliğini kontrol edilmesi için kullanılıyor
            UInt32 productAlan16Flag = 0;

            sockets.Clear();

            vw_ShopOrderGridModels = selectShopOrders;
            if (processNewActive)
            {
                var shopOrders = vw_ShopOrderGridModels.OrderByDescending(x => x.opStartDate).ToList();
                for (int i = 0; i < shopOrders.Count; i++)
                {
                    if (i == 0)
                        processShopOrderStart(shopOrders[0], ref productAlan16Flag, ref flag);
                    else
                        ShopOrderStartHelper.ShopOrderProductionDetailSettings(this, shopOrders[i], !processNewActive);
                }
            }
            else
            {
                foreach (var shopOrder in vw_ShopOrderGridModels)
                {
                    processShopOrderStart(shopOrder, ref productAlan16Flag, ref flag);
                }
            }

            shopOrderProduction = ShopOrderStartHelper.InsertShopOrderProduction(machine.Id, resource.Id, userModel, processNewActive);
            if (!String.IsNullOrWhiteSpace(StaticValues.SubcontractorWorkcenterId) && !String.IsNullOrWhiteSpace(StaticValues.SubcontractorResourceId))
            {
                var wc = new Guid(StaticValues.SubcontractorWorkcenterId);
                var rs = new Guid(StaticValues.SubcontractorResourceId);
                var subcontractorShopOrders = vw_ShopOrderGridModelManager.Current.GetShopOrderOperations(wc, rs);
                if (subcontractorShopOrders != null)
                {
                    List<vw_ShopOrderGridModel> subcontractorShopOrder = null;
                    foreach (var item in vw_ShopOrderGridModels)
                    {
                        subcontractorShopOrder = subcontractorShopOrders.Where(x => x.orderNo == item.orderNo && x.operationNo == item.operationNo - 10 && x.operationDescription.Contains("FASON")).ToList();
                        if (subcontractorShopOrder.Count > 0)
                            break;
                    }
                    if (subcontractorShopOrder != null && subcontractorShopOrder.Count > 0)
                    {
                        var product = ProductManager.Current.GetProductById(subcontractorShopOrder[0].ProductID);
                        if (product != null)
                        {
                            var resultPartHandlingUnits = PartHandlingUnitManager.Current.GetPartHandlingUnit(product.PartNo);
                            if (resultPartHandlingUnits != null)
                            {
                                SubcontractorParHandlingUnitID = resultPartHandlingUnits[0].Id;
                                SubcontractorShopOrder = subcontractorShopOrder[0];
                                SubcontractorShopOrderProductionId = ShopOrderStartHelper.InsertShopOrderProduction(wc, rs, userModel, processNewActive).Id;
                            }
                        }
                    }
                }
            }

            startEnergy = 0; // energyDBHelper.GetCurrentTotalEnergyFromOPC(resource.Id);
            StartEnergyTimer();
            foreach (var item in selectShopOrders)
            {
                PanelStatus panelStatus = new PanelStatus();
                panelStatus.ShopOrderProductionId = shopOrderProduction.Id;
                panelStatus.PanelDetailId = panelDetail.Id;
                panelStatus.PartHandlingUnitId = partHandlingUnits.First(u => u.PartNo == item.PartNo).Id;
                panelStatus.ShopOrderOperationId = item.Id;
                panelStatus.EnergyStartValue = startEnergy;
                PanelStatusManager.Current.Insert(panelStatus);
            }

            //Aktif forma user ekleme
            UserLoginHelper.StartShopOrderOperationUserLogin(this.machine, userModel);

            if (products.First().unitMeas == Units.ad.ToText())
                StaticValues.opcClient.ResetCounter(panelDetail.OPCNodeIdCounterReset);

            //Parça Üretim Süresi Gönderme metodu 
            var time = selectShopOrders.Min(x => x.machRunFactor);
            time = time * 3600;
            if (panelDetail.OPCNodeIdProductionTime != null && panelDetail.OPCNodeIdProductionTime != String.Empty)
            {
                StaticValues.opcClient.WriteNode(panelDetail.OPCNodeIdProductionTime, Convert.ToUInt16(time));
            }
            //Jetlerde İş Emri Var Yok Bilgisi
            if (panelDetail.OPCNodeIdShopOrder != null && panelDetail.OPCNodeIdShopOrder != "")
            {
                StaticValues.opcClient.WriteNode(panelDetail.OPCNodeIdShopOrder, true);
            }

            //POKAYOKE
            if (opcOtherReadModels.Any(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id))
            {
                var nodeId = opcOtherReadModels.First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id).NodeId;
                StaticValues.opcClient.WriteNode(nodeId, !pokayokeFlag);
            }

            //Machine Program NodeID (Product alan 16)
            if (productAlan16Flag > 0)
            {
                var nodeId = opcOtherReadModels.First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypeProgramNodeId.Id).NodeId;
                StaticValues.opcClient.WriteNode(nodeId, productAlan16Flag);
            }

            //Sockets Send PLC
            if (sockets.Count > 0)
            {
                var socketNo = sockets.Max(x => x.DistinctiveValue);
                var socket = sockets.First(x => x.DistinctiveValue == socketNo);
                StaticValues.opcClient.WriteNode(panelDetail.OPCNodeIdSocketAdress, Convert.ToUInt16(socket.Socket));
            }

            shopOrderStatus = ShopOrderStatus.StartProduction;
            SetMachineStateColor(MachineStateColor.Setup);
            RefreshTargetProduction();

            if (StaticValues.QrCodeGenerator == "TRUE" || StaticValues.QrCodeGenerator != null)
            {
                QrCodeHelper.ShowQrCodeForm(ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModelActive.PartNo);
            }

            try
            {
                resourceStatus = new ResourceStatus()
                {
                    MachineId = resource.Id,
                    ShopOrderProductionId = shopOrderProduction.Id,
                    Status = (int)ResourceWorkingStatus.Working
                };
                // Deactivate all old ResourceStatus if there are any where active is true
                ResourceStatusManager.Current.StopAllResourceStatus(resource.Id);
                var response = ResourceStatusManager.Current.Insert(resourceStatus);
                resourceStatus = response.ListData[0];

                foreach (var item in selectShopOrders)
                {
                    var resourceWorkOrders = new ResourceWorkOrders()
                    {
                        ResourceStatusId = resourceStatus.Id,
                        ShopOrderOperationsId = item.Id,
                        ShopOrderProductionId = shopOrderProduction.Id
                    };
                    var r = ResourceWorkOrdersManager.Current.Insert(resourceWorkOrders);
                }
            }
            catch { }

            oeePanel.RefreshOEEValues();
        }

        private void processShopOrderStart(vw_ShopOrderGridModel shopOrder, ref uint productAlan16Flag, ref bool flag)
        {
            try
            {
                var product = ProductManager.Current.GetProductById(shopOrder.ProductID);
                products.Add(product);

                if (productAlan16Flag == 0)
                    UInt32.TryParse(product.alan16, out productAlan16Flag);

                //Socket numaralarını kaydet
                if (panelDetail.OPCNodeIdSocketAdress != null && panelDetail.OPCNodeIdSocketAdress != "")
                {
                    var socketResult = StockPopupValueManager.Current.GetStockPopupValues((Guid)panelDetail.ResourceID, product.Id);
                    if (socketResult.HasEntries())
                        sockets.AddRange(socketResult);
                }

                //pokayoke alan 9 değeri 
                pokayokeFlag = product.alan15 == "1";

                if (!ShopOrderStartHelper.ShopOrderStartControl(product, shopOrder))
                    throw new Exception("Üretim için ihtiyaç duyulan yarı mamül bekleme süresi uygun değildir.");

                //Etiket ve yazıcı ayarları
                ShopOrderStartHelper.PrintAndLabelSettings(this, product, panelDetail);

                //PRESLER İÇİN Operasyon 10 ise buton + plc + barkod gibi çalış değil ise barkod+ plc gibi çalış
                //PLC'ler için eğer OPC ayarlarında ilgili parametre gidildiyse calışma modu yazılıyor
                flag = ShopOrderStartHelper.SetMachineRunMode(this, flag, panelDetail, shopOrder);


                //PART HANDLING SETTINGS
                var resultPartHandlingUnits = PartHandlingUnitManager.Current.GetPartHandlingUnit(product.PartNo);
                if (resultPartHandlingUnits == null || resultPartHandlingUnits.Count == 0)
                {
                    throw new Exception("IFS verileri üzerinde taşıma kasası bulunamadığı için iş emri başlatılamıyor.");
                }
                else if (resultPartHandlingUnits.Count > 1)
                {
                    FrmPartHandlingUnitSelect frmPartHandlingUnit = new FrmPartHandlingUnitSelect(resultPartHandlingUnits);
                    if (frmPartHandlingUnit.ShowDialog() == DialogResult.OK)
                    {
                        partHandlingUnits.Add(frmPartHandlingUnit.partHandlingUnit);
                    }
                    else
                    {
                        throw new Exception("Taşıma kasası seçimi yapılmadığı için iş emri başlatılamıyor.");
                    }
                }
                else
                {
                    partHandlingUnits.Add(resultPartHandlingUnits[0]);
                }

                //Eski ürütimler var ise ekleme
                ShopOrderStartHelper.ShopOrderProductionDetailSettings(this, shopOrder, !processNewActive);
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "918", "Beklenmedik Hata. İş Emri başlatılamadı", "Message"), ex);
                vw_ShopOrderGridModels = new System.Collections.Generic.List<Entity.Views.vw_ShopOrderGridModel>();
                return;
            }
        }

        private void FinishShopOrder(UserModel userModel)
        {
            if (processNewActive)
            {
                if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperations[0]))
                {
                    //Finish main product from shopOrderOperation
                    PartHandlingUnitProductionFinish(shopOrderOperations, products.First(), false);
                    //Finish byProduct from shopOrderOperation
                    foreach (var byProduct in byProducts)
                        PartHandlingUnitProductionFinish(shopOrderOperations, byProduct, true);
                }
            }
            else
            {
                foreach (var shopOrderOperation in shopOrderOperations)
                {
                    if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperation))
                    {
                        var shopOrderOperations = new List<ShopOrderOperation>() { shopOrderOperation };
                        //Finish main product from shopOrderOperation
                        PartHandlingUnitProductionFinish(shopOrderOperations, products.First(x => x.Id == shopOrderOperation.PartID), false);
                        //Finish byProduct from shopOrderOperation
                        foreach (var byProduct in byProducts)
                        {
                            if (prodStructAlternates.Any(x => x.PartID == shopOrderOperation.PartID && x.ComponentPartID == byProduct.Id))
                            {
                                PartHandlingUnitProductionFinish(shopOrderOperations, byProduct, true);
                            }
                        }
                    }
                }
            }

            var users = Users.ToList();
            foreach (var item in users)
            {
                UserLoginHelper.StartShopOrderOperationFinishLogin(machine, item);
            }
            ShopOrderOperationHelper.ShopOrderOperationFinish(this, userModel);
            StopEnergyTimer();
            PanelStatusManager.Current.DeletePanelStatus(panelDetail.Id);
            shopOrderProduction = null;
            exShopOrderProductionDetails = null;
            SubcontractorShopOrderProductionId = Guid.Empty;
            SubcontractorParHandlingUnitID = Guid.Empty;
            SubcontractorShopOrder = null;
            partHandlingUnits.Clear();
            products.Clear();
            TargetQty = 0;
            RefreshTargetProduction();

            try
            {
                if (resourceStatus != null)
                {
                    resourceStatus.Active = false;
                    resourceStatus.UpdatedAt = DateTime.Now;
                    ResourceStatusManager.Current.Update(resourceStatus);
                    ResourceWorkOrdersManager.Current.StopWorkOrdersByResourceStatusId(resourceStatus.Id);
                    resourceStatus = null;
                }
            }
            catch { }
            try
            {
                var shift = ShiftBookHelper.GetCurrentShift();
                ShiftBookHelper.ShiftBookData(userModel, ToolsMdiManager.frmOperatorActive.resource.Id, shift);
            }
            catch { }

            oeePanel.Clear();
        }

        public void PartHandlingUnitProductionFinish(List<ShopOrderOperation> shopOrderOperations, Product selectedProduct, bool isByProduct)
        {
            try
            {
                List<ShopOrderProductionDetail> pd = (isByProduct ? productionDetailsByProducts : productionDetails);

                // Hesaplama doğru yapılması için BoxID olmayan tüm ürünlerde HandlingUnitQuantity = 0 yapılıyor
                foreach (var product in pd.Where(p => p.BoxID == Guid.Empty && p.ProductID == selectedProduct.Id && p.ProductionStateID == StaticValues.specialCodeOk.Id))
                {
                    product.HandlingUnitQuantity = 0;
                }

                PartHandlingUnit phu;
                double maxQuantityCapacity;
                if (partHandlingUnits.Any(s => s.PartNo == selectedProduct.PartNo))
                {
                    phu = partHandlingUnits.First(s => s.PartNo == selectedProduct.PartNo);
                    maxQuantityCapacity = phu.MaxQuantityCapacity;
                }
                else if (partHandlingUnitsByProduct.Any(s => s.PartNo == selectedProduct.PartNo))
                {
                    phu = partHandlingUnitsByProduct.First(s => s.PartNo == selectedProduct.PartNo);
                    maxQuantityCapacity = phu.MaxQuantityCapacity;
                }
                else
                {
                    var prm = selectedProduct.PartNo.CreateParameters("@PartNo");
                    throw new Exception(ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "632", "@PartNo için Kasa bulunamadığı için Otomatik bildirim yapılamadı.", "Message"), prm));
                }
                List<ShopOrderProductionDetail> shopOrderProductionDetailList;
                // IFS'e gönderilmeyen kasa içi miktarından fazla ürün üretildiyse bu ürünleri IFS'e gönder
                if (!processNewActive)
                {
                    if (halfHandlingUnit.ContainsKey(shopOrderOperations[0].Id))
                    {
                        halfHandlingUnitProcess = halfHandlingUnit[shopOrderOperations[0].Id];
                    }
                    else
                        halfHandlingUnitProcess = null;
                }
                else
                {
                    //Process İş emirlerinde farklı productionda üretim devam ediyorsa ve daha bildirimi yapılmamışsa mükerrer kayıda yol açmaktadır. Bu yüzden burada iş emrini kapatırken bildirim yaparken production kontrolü yapıyorum.25.02.2025
                    pd = pd.Where(x => x.ShopOrderProductionID == this.shopOrderProduction.Id).ToList();
                }
                while (pd.Where(p => p.ProductID == selectedProduct.Id && p.ProductionStateID == StaticValues.specialCodeOk.Id && p.Quantity > p.HandlingUnitQuantity).Sum(y => y.Quantity - y.HandlingUnitQuantity) > 0)
                {
                    shopOrderProductionDetailList = new List<ShopOrderProductionDetail>();
                    decimal currentQuantity = 0;
                    if (halfHandlingUnitProcess != null)
                        currentQuantity = halfHandlingUnitProcess.Quantity;
                    bool flag = false;
                    // Fill shopOrderProductionDetailList until maxQuantityCapacity reached or no more unsend productionDetails exist
                    foreach (var productDetail in pd.Where(pr => pr.ProductID == selectedProduct.Id && pr.ProductionStateID == StaticValues.specialCodeOk.Id && pr.Quantity > pr.HandlingUnitQuantity).ToList())
                    {
                        shopOrderProductionDetailList.Add(productDetail);

                        if ((productDetail.Quantity - productDetail.HandlingUnitQuantity) + currentQuantity >= (decimal)maxQuantityCapacity)
                        {
                            productDetail.HandlingUnitQuantity += (decimal)maxQuantityCapacity - currentQuantity;
                            currentQuantity = (decimal)maxQuantityCapacity;

                            ShopOrderProductionDetailManager.Current.Update(productDetail);
                            flag = true;
                        }
                        else
                        {
                            currentQuantity += productDetail.Quantity - productDetail.HandlingUnitQuantity;
                            productDetail.HandlingUnitQuantity = productDetail.Quantity;
                            ShopOrderProductionDetailManager.Current.Update(productDetail);
                        }

                        if (flag)
                            break;
                    }

                    ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();
                    reportHandlingUnitHelper.product = selectedProduct;
                    ReportProcessHandlingUnitHelper report = new ReportProcessHandlingUnitHelper();//PrintLabel için oluşturuldu

                    if (OperatorPanelConfigurationHelper.PrintBoxLabel(shopOrderOperations[0]))
                        reportHandlingUnitHelper.printLabelModel = printLabelModels.First(pl => pl.ProductId == reportHandlingUnitHelper.product.Id && pl.productionLabelType == ProductionLabelType.Box);

                    reportHandlingUnitHelper.machine = machine;
                    reportHandlingUnitHelper.resource = resource;
                    reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                    reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                    reportHandlingUnitHelper.shopOrderProductionDetails = shopOrderProductionDetailList;

                    var queryResult = shopOrderProductionDetailList.GroupBy(p => p.ShopOrderOperationID).ToList();

                    string boxBarcode = (halfHandlingUnitProcess != null ? halfHandlingUnitProcess.BoxBarcode : "");
                    // Create a HandlingUnit for each ShopOrderOperation using the same BoxBarcode and send to IFS
                    for (int i = 0; i < queryResult.Count; i++)
                    {
                        var details = shopOrderProductionDetailList.Where(x => x.ShopOrderOperationID == queryResult[i].Key).ToList();
                        try
                        {
                            reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(z => z.Id == queryResult[i].Key);
                        }
                        catch (Exception)
                        {
                            reportHandlingUnitHelper.shopOrderOperation = ShopOrderOperationManager.Current.GetShopOrderOperationById(queryResult[i].Key);
                        }
                        reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.Single(x => x.Id == queryResult[i].Key);

                        reportHandlingUnitHelper.handlingUnit = null;
                        if (halfHandlingUnitProcess != null)
                        {
                            var hu = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(queryResult[i].Key);
                            if (hu != null)
                                hu = hu.Where(x => x.Barcode == halfHandlingUnitProcess.BoxBarcode).ToList();
                            if (hu.HasEntries())
                                reportHandlingUnitHelper.handlingUnit = hu[0];
                        }

                        if (reportHandlingUnitHelper.handlingUnit == null)
                        {
                            HandlingUnit handlingUnit = new HandlingUnit();
                            handlingUnit.PartHandlingUnitID = phu.Id;
                            handlingUnit.ShopOrderID = queryResult[i].Key;
                            handlingUnit.ShopOrderProductionID = shopOrderProduction.Id;
                            handlingUnit.Quantity = details.Sum(x => x.Quantity);
                            handlingUnit.ManuelInput = 0;
                            handlingUnit.Description = "PLC";
                            handlingUnit.Barcode = boxBarcode;
                            var myhandlingUnit = HandlingUnitManager.Current.Insert(handlingUnit).ListData[0];

                            if (boxBarcode.Length == 0)
                            {
                                boxBarcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);
                                myhandlingUnit.Barcode = boxBarcode;
                                HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);
                            }

                            reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                        }
                        else
                        {
                            reportHandlingUnitHelper.handlingUnit.Quantity += details.Sum(x => x.Quantity);
                            HandlingUnitManager.Current.Update(reportHandlingUnitHelper.handlingUnit);
                            reportHandlingUnitHelper.handlingUnit.ManuelInput = 0;
                        }

                        foreach (var shopOrderProductionDetail in details)
                        {
                            shopOrderProductionDetail.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                            shopOrderProductionDetail.IfsReported = true;
                            ShopOrderProductionDetailManager.Current.Update(shopOrderProductionDetail);
                        }

                        reportHandlingUnitHelper.shopOrderProductionDetails = details;
                        //IfsSendReport.IFSSendApproveOp(reportHandlingUnitHelper);

                        reportHandlingUnitHelper.handlingUnit.SendIfs = true;
                        reportHandlingUnitHelper.handlingUnit.SendDate = DateTime.Now;
                        HandlingUnitManager.Current.UpdateBoxIfsSend(reportHandlingUnitHelper.handlingUnit, DateTime.Now);


                        report.shopOrderOperations.Add(reportHandlingUnitHelper.shopOrderOperation);
                        report.vwShopOrderGridModels.Add(reportHandlingUnitHelper.vwShopOrderGridModel);
                        report.handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);
                    }

                    if (halfHandlingUnitProcess != null)
                    {
                        var allHandlingUnits = HandlingUnitManager.Current.GetHandlingUnitsByBarcode(halfHandlingUnitProcess.BoxBarcode);
                        foreach (var hu in allHandlingUnits)
                        {
                            if (!report.handlingUnits.Any(x => x.Id == hu.Id))
                                report.handlingUnits.Add(hu);
                        }
                    }

                    // Print Label if parameter is set in panelDetail
                    if (OperatorPanelConfigurationHelper.PrintBoxLabel(shopOrderOperations[0]))
                    {
                        report.machine = machine;
                        report.resource = resource;
                        report.shopOrderProduction = shopOrderProduction;
                        report.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                        report.shopOrderProductionDetails = shopOrderProductionDetailList;
                        report.printLabelModel = reportHandlingUnitHelper.printLabelModel;
                        report.PrintLabel();
                    }
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void HandleOrderChange(List<vw_ShopOrderGridModel> orders, List<vw_ShopOrderGridModel> newOrders, decimal qtyDue, decimal qtyDone)
        {
            var users = Users.ToList();
            var user = users[0];

            FinishShopOrder(user);
            DeactivateShopOrderOperation(orders);
            InitializeShopOrder(user, newOrders);

            foreach (var item in users)
            {
                if (item.IfsEmplooyeId != user.IfsEmplooyeId)
                    UserLoginHelper.StartShopOrderOperationUserLogin(this.machine, item);
            }

            ShowOrderChangeInfo(orders, newOrders, qtyDue, qtyDone);
        }

        private void HandleOrderCompletion(List<vw_ShopOrderGridModel> orders, decimal qtyDue, decimal qtyDone)
        {
            FinishShopOrder(Users[0]);
            DeactivateShopOrderOperation(orders);
            ShowOrderCompletionInfo(orders, qtyDue, qtyDone);
        }

        private void CheckOrderQuantityAndAutoFinishOrder()
        {
            // If process shop order then do nothing
            if (processNewActive)
                return;

            // If panelDetail automaticFinishOrder is not set return
            if (!panelDetail.AutomaticFinishShopOrder && !panelDetail.AutoEndShopOrder)
                return;

            // If Container is visible return
            if (container.Visible)
                return;

            // Check interruption status and return if interruption is running
            if (interruptionCause != null)
                return;

            // Check fault status and return if active fault is running
            if (faults.Count > 0 && faults.Last().Active)
                return;

            // Check order quantity. If done amount less than due amount return
            decimal qtyDue = vw_ShopOrderGridModelActive.revisedQtyDue;
            decimal qtyDone = productionDetails
                .Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.ProductionStateID == StaticValues.specialCodeOk.Id && x.Active)
                .Sum(y => y.Quantity);
            if (qtyDone + exShopOrderProductionDetails.Where(z => z.ProductionStateID == StaticValues.specialCodeOk.Id && z.Active).Sum(x => x.Quantity) < qtyDue)
                return;

            // Everything checked and ready to auto finish order and start new order if possible
            bool startNewOrders = true;
            var orders = vw_ShopOrderGridModels.ToList();
            var newOrders = FindNextOrders(orders, out startNewOrders);

            if (panelDetail.AutomaticFinishShopOrder)
            {
                if (startNewOrders)
                {
                    HandleOrderChange(orders, newOrders, qtyDue, qtyDone);
                }
                else
                {
                    HandleOrderCompletion(orders, qtyDue, qtyDone);
                }
            }
            else if (panelDetail.AutoEndShopOrder)
            {
                HandleOrderCompletion(orders, qtyDue, qtyDone);
            }
        }

        private List<vw_ShopOrderGridModel> FindNextOrders(List<vw_ShopOrderGridModel> orders, out bool startNewOrders)
        {
            var newOrders = new List<vw_ShopOrderGridModel>();
            var shopOrders = vw_ShopOrderGridModelManager.Current.GetShopOrderOperationModels(StaticValues.branch.Id, machine.Id, false);

            if (shopOrders.HasEntries())
            {
                foreach (var order in orders)
                {
                    var list = shopOrders
                        .Where(x => x.ProductID == order.ProductID && x.operationNo == order.operationNo)
                        .OrderByDescending(x => x.opStartDate)
                        .ToList();

                    if (!list.HasEntries())
                    {
                        startNewOrders = false;
                        return null;
                    }

                    var newOrder = list.FirstOrDefault(item => IsOrderValid(item));
                    if (newOrder == null)
                    {
                        startNewOrders = false;
                        return null;
                    }
                    newOrders.Add(newOrder);
                }
            }
            else
            {
                startNewOrders = false;
            }

            startNewOrders = newOrders.Any();
            return newOrders;
        }

        private bool IsOrderValid(vw_ShopOrderGridModel item)
        {
            var pds = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetails(item.Id);
            if (!pds.HasEntries()) return true;

            decimal foundQty = pds
                .Where(x => x.ProductionStateID == StaticValues.specialCodeOk.Id && x.Active)
                .Sum(y => y.Quantity);

            return foundQty < item.revisedQtyDue;
        }

        private void DeactivateShopOrderOperation(List<vw_ShopOrderGridModel> orders)
        {
            foreach (var order in orders)
            {
                var so = ShopOrderOperationManager.Current.GetShopOrderOperationById(order.Id);
                so.Active = false;
                ShopOrderOperationManager.Current.Update(so);
            }
        }

        private void ShowOrderChangeInfo(List<vw_ShopOrderGridModel> oldOrders, List<vw_ShopOrderGridModel> newOrders, decimal qtyDue, decimal qtyDone)
        {
            string msg = "İş Emri otomatik olarak değiştirilmiştir\r\n\r\n" +
                $"İş Merkezi: {Text}\r\n" +
                $"İş Emri miktarı: {qtyDue}\r\n" +
                $"Üretim miktarı: {qtyDone}\r\n" +
                "İş emri no: ";
            for (int i = 0; i < oldOrders.Count; i++)
                msg += $"\r\n{oldOrders[i].orderNo} -> {newOrders[i].orderNo}";
            ToolsMessageBox.BigWarning(this, msg);
        }

        private void ShowOrderCompletionInfo(List<vw_ShopOrderGridModel> orders, decimal qtyDue, decimal qtyDone)
        {
            string msg = "İş Emri otomatik olarak kapatılmıştır\r\n\r\n" +
                $"İş Merkezi: {Text}\r\n" +
                $"İş Emri miktarı: {qtyDue}\r\n" +
                $"Üretim miktarı: {qtyDone + exShopOrderProductionDetails.Sum(x => x.Quantity)}\r\n" +
                "İş emri no: ";
            for (int i = 0; i < orders.Count; i++)
                msg += (i == 0 ? "" : ",") + orders[i].orderNo;
            ToolsMessageBox.BigWarning(this, msg);
        }
        #endregion

        #region INSERT SHOP ORDER PRODUCTION
        private void InsertShopOrderProductionDetail(DateTime dateTime)
        {
            //ToDO : tekrsr sayısına göre kaydet
            foreach (var item in shopOrderProductionDetails)
            {
                //var factor = shopOrderOperations.First(x => x.PartID == item.ProductID).alan1;
                int count = 1;
                Product selectedProduct = products.First(x => x.Id == item.ProductID);
                if (int.TryParse(shopOrderOperations.First(x => x.PartID == item.ProductID).alan1, out count) == false)
                {
                    var prm = selectedProduct.PartNo.CreateParameters("@PartNo");
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "927", "IFS Verileri üzerinde çarpan değeri bulunamadı.\r\nAdmin Info: PartNo : @PartNo", "Message"), prm);
                }
                for (int i = 0; i < count; i++)
                {
                    item.EndDate = dateTime;
                    item.Quantity = 1;
                    item.Id = Guid.NewGuid();
                    item.HandlingUnitQuantity = 0;
                    var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                    var result = resultx.ListData[0];

                    productionDetails.Add(result);

                    ReportProductHelper reportProductHelper = new ReportProductHelper();

                    reportProductHelper.shopOrderOperation = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                    reportProductHelper.shopOrderProduction = shopOrderProduction;
                    reportProductHelper.shopOrderProductionDetail = result;
                    reportProductHelper.machine = machine;
                    reportProductHelper.resource = resource;
                    reportProductHelper.product = selectedProduct;
                    reportProductHelper.userModel = Users.OrderByDescending(x => x.Role).First();

                    //Adet sayma Kasa doldu bilgisi
                    if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID)))
                        PartHandlingUnitMaxQuotaCheck(selectedProduct, item);

                    if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProcess.Id &&
                        panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypePLC.Id)
                    {
                        result.Barcode = result.serial.ToString();
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                    }
                    else if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProcess.Id &&
                        panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id &&
                        reportProductHelper.shopOrderOperation.operationNo == 10)
                    {
                        result.Barcode = result.serial.ToString();
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                    }
                    else if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id)
                    {
                        //result.Barcode = shopOrderProductionDetailProcess.Barcode;
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);
                    }

                    //Print
                    try
                    {
                        if (panelDetail.PrintProductBarcode && reportProductHelper.shopOrderOperation.alan5 == "TRUE")
                        {
                            var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportProductHelper.product, machine, resource, ProductionLabelType.Product);

                            if (selectedPrintLabelModel != null)
                            {
                                reportProductHelper.printLabelModel = selectedPrintLabelModel;
                            }
                            else if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product))
                            {
                                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product);
                            }

                            reportProductHelper.PrintLabel();
                            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
                        }
                        else if (panelDetail.ProcessBarcode && reportProductHelper.shopOrderOperation.alan7 == "TRUE")
                        {
                            if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process))
                                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process);

                            reportProductHelper.PrintLabel();
                            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
                        }
                    }
                    catch
                    {
                        //ToDo: Log yazılacak neden yazdırılamadığına dair
                    }
                }
            }
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        private void InsertShopOrderProductionDetailProcess(DateTime dateTime)
        {
            //ToDO : tekrsr sayısına göre kaydet
            foreach (var item in shopOrderProductionDetails)
            {
                int count = 1;
                Product selectedProduct = products.First(x => x.Id == item.ProductID);
                if (int.TryParse(shopOrderOperations.First(x => x.PartID == item.ProductID).alan1, out count) == false)
                {
                    var prm = selectedProduct.PartNo.CreateParameters("@PartNo");
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "927", "IFS Verileri üzerinde çarpan değeri bulunamadı.\r\nAdmin Info: PartNo : @PartNo", "Message"), prm);
                }
                for (int i = 0; i < count; i++)
                {
                    item.EndDate = dateTime;
                    item.Quantity = 1;
                    item.Id = Guid.NewGuid();
                    item.HandlingUnitQuantity = 0;
                    var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                    var result = resultx.ListData[0];
                    productionDetails.Add(result);

                    ReportProductHelper reportProductHelper = new ReportProductHelper();
                    reportProductHelper.shopOrderOperation = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                    reportProductHelper.shopOrderProduction = shopOrderProduction;
                    reportProductHelper.shopOrderProductionDetail = result;
                    reportProductHelper.machine = machine;
                    reportProductHelper.resource = resource;
                    reportProductHelper.product = selectedProduct;
                    reportProductHelper.userModel = Users.OrderByDescending(x => x.Role).First();

                    //Adet sayma Kasa doldu bilgisi
                    if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperations[0]))
                        PartHandlingUnitMaxQuotaCheckProcess(selectedProduct, item);

                    if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProcess.Id &&
                        panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypePLC.Id)
                    {
                        result.Barcode = result.serial.ToString();
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                    }
                    else if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProcess.Id &&
                        panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id &&
                        reportProductHelper.shopOrderOperation.operationNo == 10)
                    {
                        result.Barcode = result.serial.ToString();
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                    }
                    else if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id)
                    {
                        //result.Barcode = shopOrderProductionDetailProcess.Barcode;
                        ShopOrderProductionDetailManager.Current.UpdateBarcode(result);
                    }

                    //Print
                    try
                    {
                        if (panelDetail.PrintProductBarcode && reportProductHelper.shopOrderOperation.alan5 == "TRUE")
                        {
                            var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportProductHelper.product, machine, resource, ProductionLabelType.Product);

                            if (selectedPrintLabelModel != null)
                            {
                                reportProductHelper.printLabelModel = selectedPrintLabelModel;

                            }
                            else if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product))
                            {
                                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product);
                            }

                            reportProductHelper.PrintLabel();
                            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
                        }
                        else if (panelDetail.ProcessBarcode && reportProductHelper.shopOrderOperation.alan7 == "TRUE")
                        {
                            if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process))
                                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process);

                            reportProductHelper.PrintLabel();
                            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
                        }
                    }
                    catch
                    {
                        //ToDo: Log yazılacak neden yazdırılamadığına dair
                    }
                }
            }
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        //public void InsertShopOrderProductionDetailSupplierPark(DateTime dateTime)
        //{
        //    supplierPartProductionDetailSelected.Id = Guid.NewGuid();
        //    supplierPartProductionDetailSelected.EndDate = dateTime;
        //    supplierPartProductionDetailSelected.Quantity = 1;
        //    supplierPartProductionDetailSelected.HandlingUnitQuantity = 1;
        //    supplierPartProductionDetailSelected.CompanyPersonId = Users.First().CompanyPersonId;

        //    var resultx = ShopOrderProductionDetailManager.Current.Insert(supplierPartProductionDetailSelected);
        //    var result = resultx.ListData[0];

        //    Product selectedProduct = products.First(x => x.Id == result.ProductID);
        //    productionDetails.Add(result);

        //    IfsSendReport.IFSSendApproveOpSupplierPark(vw_ShopOrderGridModels.First(x => x.Id == result.ShopOrderOperationID), Users.First(), result);

        //    ShopOrderProductionDetailManager.Current.UpdateIfsReportedAndPrinted(result);

        //    //Print
        //    ReportProductHelper reportProductHelper = new ReportProductHelper();

        //    reportProductHelper.shopOrderOperation = vw_ShopOrderGridModels.First(x => x.Id == result.ShopOrderOperationID);
        //    reportProductHelper.shopOrderProduction = shopOrderProduction;
        //    reportProductHelper.shopOrderProductionDetail = result;
        //    reportProductHelper.machine = machine;
        //    reportProductHelper.resource = resource;
        //    reportProductHelper.product = selectedProduct;
        //    reportProductHelper.userModel = Users.OrderByDescending(x => x.Role).First();

        //    try
        //    {
        //        if (panelDetail.PrintProductBarcode && reportProductHelper.shopOrderOperation.alan5 == "TRUE")
        //        {
        //            var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportProductHelper.product, machine, resource, ProductionLabelType.Product);

        //            if (selectedPrintLabelModel != null)
        //            {
        //                reportProductHelper.printLabelModel = selectedPrintLabelModel;

        //            }
        //            else if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product))
        //                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product);

        //            reportProductHelper.PrintLabel();
        //            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
        //        }
        //        else if (panelDetail.ProcessBarcode && reportProductHelper.shopOrderOperation.alan7 == "TRUE")
        //        {
        //            if (printLabelModels.Any(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process))
        //                reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process);

        //            reportProductHelper.PrintLabel();
        //            result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
        //        }
        //    }
        //    catch
        //    {
        //        //ToDo: Log yazılacak neden yazdırılamadığına dair
        //    }

        //    CreateShopOrderProductionDetails();
        //    SetLabelsTextValue();
        //}

        public void InsertShopOrderProductionDetailM2(DateTime dateTime)
        {
            foreach (var item in shopOrderProductionDetails)
            {
                var prm = item.ProductID.CreateParameters("@ProductID");
                int count = 1;
                if (int.TryParse(shopOrderOperations.First(x => x.PartID == item.ProductID).alan1, out count) == false)
                {
                    throw new Exception(ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "601", "IFS Verileri üzerinde çarpan değeri bulunamadı.\r\nAdmin Info: ProductID : @ProductID", "Message"), prm));
                }

                var product_ = products.First(p => p.Id == item.ProductID);
                item.EndDate = dateTime;
                decimal width = 1;
                if (product_.alan6 == null)
                {
                    throw new Exception(ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "631", "IFS Verileri üzerinde en bilgisi bulunamadı.\r\nAdmin Info: ProductID : @ProductID", "Message"), prm));
                }

                product_.alan6 = product_.alan6.Replace(',', '.');
                if (!decimal.TryParse(product_.alan6, NumberStyles.Any, CultureInfo.InvariantCulture, out width))
                {
                    throw new Exception(ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "631", "IFS Verileri üzerinde en bilgisi bulunamadı.\r\nAdmin Info: ProductID : @ProductID", "Message"), prm));
                }

                item.Quantity = _manualInputBySquareMeters * width;
                item.HandlingUnitQuantity = _manualInputBySquareMeters * width;
                item.Id = Guid.NewGuid();
                item.ManualInput = _manualInputBySquareMeters * width;
                var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                var result = resultx.ListData[0];

                Product selectedProduct = products.First(x => x.Id == result.ProductID);
                productionDetails.Add(result);

                ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                PartHandlingCreateAndSendIFSM2(selectedProduct, item);
            }
            _manualInputBySquareMeters = 0;
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        private void InsertShopOrderProductionDetailKilogram(DateTime dateTime)
        {
            foreach (var item in shopOrderProductionDetails)
            {
                var production = productionDetails.Where(x => x.BoxID == Guid.Empty && x.ShopOrderProductionID == shopOrderProduction.Id).ToList();
                if (production.Count == 0)
                {
                    item.EndDate = dateTime;
                    item.Quantity = _manuelInputByKilogram;
                    item.HandlingUnitQuantity = _manuelInputByKilogram;
                    item.Id = Guid.NewGuid();
                    item.ManualInput = _manuelInputByKilogram;
                    var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                    var result = resultx.ListData[0];

                    Product selectedProduct = products.First(x => x.Id == result.ProductID);
                    productionDetails.Add(result);

                    ShopOrderProductionDetailManager.Current.UpdateBarcode(result);

                    PartHandlingCreateAndSendIFSKilogram(selectedProduct, item);
                }
                else
                {
                    production[0].EndDate = dateTime;
                    production[0].ManualInput = _manuelInputByKilogram - production[0].Quantity;
                    production[0].Quantity = _manuelInputByKilogram;
                    production[0].HandlingUnitQuantity = _manuelInputByKilogram;
                    ShopOrderProductionDetailManager.Current.Update(production[0]);

                    Product selectedProduct = products.First(x => x.Id == production[0].ProductID);

                    PartHandlingCreateAndSendIFSKilogram(selectedProduct, item);
                }
            }
            _manuelInputByKilogram = 0;
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        private void InsertShopOrderProductionDetailKilogramPLC(DateTime dateTime)
        {

            foreach (var item in shopOrderProductionDetails)
            {
                if (shopOrderOperations.First(x => x.PartID == item.ProductID).alan8 != "TRUE")
                {
                    continue;
                }
                Product selectedProduct = products.First(x => x.Id == item.ProductID);
                decimal weight = 1;
                if (decimal.TryParse(selectedProduct.alan17, out weight) == false)
                {
                    var prm = selectedProduct.PartNo.CreateParameters("@PartNo");
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "928", "IFS Verileri üzerinde birim dönüşüm ağırlığı bulunamadı.\r\nAdmin Info: PartNo : @PartNo", "Message"), prm);
                }

                var production = productionDetails.Where(x => x.BoxID == Guid.Empty && x.ShopOrderProductionID == shopOrderProduction.Id).ToList();
                if (production.Count == 0)
                {
                    item.EndDate = dateTime;
                    item.Quantity = weight;
                    item.HandlingUnitQuantity = 0;
                    item.Id = Guid.NewGuid();
                    item.ManualInput = 0;
                    var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                    var result = resultx.ListData[0];

                    productionDetails.Add(result);
                    ShopOrderProductionDetailManager.Current.UpdateBarcode(result);
                }
                else
                {
                    production[0].EndDate = dateTime;
                    production[0].Quantity += weight;
                    //production[0].HandlingUnitQuantity += weight;
                    production[0].ManualInput = 0;
                    ShopOrderProductionDetailManager.Current.Update(production[0]);
                }

                //PartHandlingCreateAndSendIFSKilogram(selectedProduct, item);
            }
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        private void InsertShopOrderProductionDetailPress(DateTime dateTime)
        {
            //ToDO : tekrsr sayısına göre kaydet
            foreach (var item in shopOrderProductionDetails)
            {
                //var factor = shopOrderOperations.First(x => x.PartID == item.ProductID).alan1;
                int count = 1;
                Product selectedProduct = products.First(x => x.Id == item.ProductID);
                if (int.TryParse(shopOrderOperations.First(x => x.PartID == item.ProductID).alan1, out count) == false)
                {
                    var prm = selectedProduct.PartNo.CreateParameters("@PartNo");
                    ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "927", "IFS Verileri üzerinde çarpan değeri bulunamadı.\r\nAdmin Info: PartNo : @PartNo", "Message"), prm);
                }
                for (int i = 0; i < count; i++)
                {
                    item.EndDate = dateTime;
                    item.Quantity = 1;
                    item.HandlingUnitQuantity = 0;
                    item.Id = Guid.NewGuid();
                    var resultx = ShopOrderProductionDetailManager.Current.Insert(item);
                    var result = resultx.ListData[0];
                    productionDetails.Add(result);


                    ReportProductHelper reportProductHelper = new ReportProductHelper();
                    reportProductHelper.shopOrderOperation = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                    reportProductHelper.shopOrderProduction = shopOrderProduction;
                    reportProductHelper.shopOrderProductionDetail = result;
                    reportProductHelper.machine = machine;
                    reportProductHelper.resource = resource;
                    reportProductHelper.product = selectedProduct;

                    PrintLabelModel selectedPrintLabelModel = null;
                    ProductionLabelType labelType = ProductionLabelType.Product;
                    bool labelTypeFound = false;
                    if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProduct.Id || selectedProduct.PrintProductLabel)
                    {
                        labelType = ProductionLabelType.Product;
                        labelTypeFound = true;
                        reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Product);
                        selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportProductHelper.product, machine, resource, ProductionLabelType.Product);
                    }
                    else if (panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProcess.Id)
                    {
                        labelType = ProductionLabelType.Process;
                        labelTypeFound = true;
                        reportProductHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportProductHelper.product.Id && x.productionLabelType == ProductionLabelType.Process);
                        selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportProductHelper.product, machine, resource, ProductionLabelType.Process);
                    }

                    if (selectedPrintLabelModel != null)
                    {
                        reportProductHelper.printLabelModel = selectedPrintLabelModel;
                        reportProductHelper.userModel = Users.OrderByDescending(x => x.Role).First();
                        reportProductHelper.PrintLabel();

                        result.Printed = ShopOrderProductionDetailManager.Current.UpdatePrinted(result);
                    }
                    else
                    {
                        var prn = reportProductHelper.product.PartNo.CreateParameters("@PartNo");
                        prn.Add("@Machine", machine.Code);
                        prn.Add("@Resource", resource.resourceName);
                        prn.Add("@PrinterType", (labelTypeFound ? labelType.ToText() : ""));
                        ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "636", "Tanımlı yazıcı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                    }
                }
            }

            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }

        private void InsertShopOrderProductionDetailNoBarcode(DateTime dateTime)
        {
            // seçili iş emirlerini için acılmış productionDetail'leri kaydet ve IFS'e gönder
            foreach (var item in shopOrderProductionDetails)
            {
                item.EndDate = dateTime;
                item.Quantity = Convert.ToDecimal(shopOrderOperations.First(x => x.PartID == item.ProductID).alan1);
                item.Id = Guid.NewGuid();
                var result = ShopOrderProductionDetailManager.Current.Insert(item).ListData[0];
                Product selectedProduct = products.First(x => x.Id == result.ProductID);
                productionDetails.Add(result);

                //Adet sayma Kasa doldu bilgisi
                //ToDo: Kasa etiketi yazdırma ve IFS gönerme ayrı ayrı kontrol edilecek;
                //ToDo: ProductionDetail tablosunda çarpan sayısı çok olan ürünler için kontrol edilmesi gereken Kasa içine konulan toplam değer alanı açılacak ürün barkodu ve proses barkodu olmayan ürünler için kasaya konulan miktar bilgisi ayrıca boxlabel içinde de programlanması gerekiyor; 
                if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID)))
                    if (processNewActive)
                        PartHandlingUnitMaxQuotaCheckProcess(selectedProduct, result);
                    else
                        PartHandlingUnitMaxQuotaCheckNoProductBarcode(selectedProduct, result);
            }
            CreateShopOrderProductionDetails();
            SetLabelsTextValue();
        }
        #endregion

        #region CREATE HANDLING UNITS
        private void PartHandlingCreateAndSendIFSM2(Product selectedProduct, ShopOrderProductionDetail item)
        {
            try
            {
                ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();

                reportHandlingUnitHelper.product = selectedProduct;
                reportHandlingUnitHelper.printLabelModel = printLabelModels.First(p => p.ProductId == reportHandlingUnitHelper.product.Id && p.productionLabelType == ProductionLabelType.Box);
                reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.machine = machine;
                reportHandlingUnitHelper.resource = resource;
                reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));

                reportHandlingUnitHelper.handlingUnit = new HandlingUnit();
                reportHandlingUnitHelper.handlingUnit.PartHandlingUnitID = partHandlingUnits.First(x => x.PartNo == reportHandlingUnitHelper.product.PartNo).Id;//partHandlingUnitID;
                reportHandlingUnitHelper.handlingUnit.ShopOrderID = reportHandlingUnitHelper.shopOrderOperation.Id;
                reportHandlingUnitHelper.handlingUnit.ShopOrderProductionID = reportHandlingUnitHelper.shopOrderProduction.Id;
                reportHandlingUnitHelper.handlingUnit.Quantity = productionDetails.First(x => (x.BoxID == null || x.BoxID == Guid.Empty) && x.ProductionStateID == StaticValues.specialCodeOk.Id).Quantity;
                reportHandlingUnitHelper.handlingUnit.ManuelInput = item.ManualInput;
                reportHandlingUnitHelper.handlingUnit.Description = _SquareMetersDescription;

                var myhandlingUnit = HandlingUnitManager.Current.Insert(reportHandlingUnitHelper.handlingUnit).ListData[0];

                myhandlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);

                HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);

                reportHandlingUnitHelper.handlingUnit = myhandlingUnit;

                reportHandlingUnitHelper.shopOrderProductionDetails = productionDetails.Where(p => p.BoxID == Guid.Empty && p.ProductionStateID == StaticValues.specialCodeOk.Id).ToList();

                foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                {
                    shopOrderProductionDetail.BoxID = myhandlingUnit.Id;
                    productionDetails.First(x => x.Id == shopOrderProductionDetail.Id).BoxID = myhandlingUnit.Id;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(shopOrderProductionDetail);
                }

                //IfsSendReport.IFSSendApproveOpM2(reportHandlingUnitHelper);

                foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                {
                    ShopOrderProductionDetailManager.Current.UpdateIfsReportedAndPrinted(shopOrderProductionDetail);
                }

                HandlingUnitManager.Current.UpdateBoxIfsSend(myhandlingUnit);
                handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);

                var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportHandlingUnitHelper.product, machine, resource, ProductionLabelType.Box);
                if (selectedPrintLabelModel != null)
                {
                    reportHandlingUnitHelper.printLabelModel = selectedPrintLabelModel;
                    reportHandlingUnitHelper.PrintLabel();

                    return;
                }
                else
                {
                    var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                    prn.Add("@Machine", machine.Code);
                    prn.Add("@Resource", resource.resourceName);
                    prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                    ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "636", "Tanımlı yazıcı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                }

                if (printLabelModels.Any(x => x.ProductId == reportHandlingUnitHelper.product.Id))
                {

                    reportHandlingUnitHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportHandlingUnitHelper.product.Id);
                    reportHandlingUnitHelper.PrintLabel();
                }
                else
                {
                    var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                    prn.Add("@Machine", machine.Code);
                    prn.Add("@Resource", resource.resourceName);
                    prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                    ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "637", "Tanımlı etiket dizaynı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void PartHandlingCreateAndSendIFSKilogram(Product selectedProduct, ShopOrderProductionDetail item)
        {
            try
            {
                ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();

                reportHandlingUnitHelper.product = selectedProduct;
                reportHandlingUnitHelper.printLabelModel = printLabelModels.First(p => p.ProductId == reportHandlingUnitHelper.product.Id && p.productionLabelType == ProductionLabelType.Box);
                reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.machine = machine;
                reportHandlingUnitHelper.resource = resource;
                reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));

                reportHandlingUnitHelper.handlingUnit = new HandlingUnit();

                reportHandlingUnitHelper.handlingUnit.PartHandlingUnitID = partHandlingUnits.First(x => x.PartNo == reportHandlingUnitHelper.product.PartNo).Id;//partHandlingUnitID;
                reportHandlingUnitHelper.handlingUnit.ShopOrderID = reportHandlingUnitHelper.shopOrderOperation.Id;
                reportHandlingUnitHelper.handlingUnit.ShopOrderProductionID = reportHandlingUnitHelper.shopOrderProduction.Id;
                reportHandlingUnitHelper.handlingUnit.Quantity = _manuelInputByKilogram;
                reportHandlingUnitHelper.handlingUnit.ManuelInput = _manuelInputByKilogram;
                if (_PartiNoKilogramDescription == null)
                {
                    reportHandlingUnitHelper.handlingUnit.Description = $"Kilogram orderNo:{reportHandlingUnitHelper.vwShopOrderGridModel.orderNo} OperationNo{reportHandlingUnitHelper.shopOrderOperation.operationNo} ";
                }
                else
                {
                    reportHandlingUnitHelper.handlingUnit.Description = _PartiNoKilogramDescription;
                }

                var myhandlingUnit = HandlingUnitManager.Current.Insert(reportHandlingUnitHelper.handlingUnit).ListData[0];

                myhandlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);

                HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);

                reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                reportHandlingUnitHelper.shopOrderProductionDetails = new List<ShopOrderProductionDetail>();
                reportHandlingUnitHelper.shopOrderProductionDetails.Add(productionDetails.Last(p => p.BoxID == Guid.Empty && p.ProductionStateID == StaticValues.specialCodeOk.Id));

                foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                {
                    shopOrderProductionDetail.BoxID = myhandlingUnit.Id;
                    shopOrderProductionDetail.IfsReported = true;
                    //productionDetails.First(x => x.Id == shopOrderProductionDetail.Id && x.ProductionStateID == StaticValues.specialCodeOk.Id).BoxID = myhandlingUnit.Id;
                    //productionDetails.First(x => x.Id == shopOrderProductionDetail.Id && x.ProductionStateID == StaticValues.specialCodeOk.Id).IfsReported = true;
                    ShopOrderProductionDetailManager.Current.Update(shopOrderProductionDetail);
                }

                //IfsSendReport.IFSSendApproveOpKilogram(reportHandlingUnitHelper);

                foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                {
                    ShopOrderProductionDetailManager.Current.UpdateIfsReportedAndPrinted(shopOrderProductionDetail);
                }

                HandlingUnitManager.Current.UpdateBoxIfsSend(myhandlingUnit);
                handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);

                var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportHandlingUnitHelper.product, machine, resource, ProductionLabelType.Box);
                if (selectedPrintLabelModel != null)
                {
                    reportHandlingUnitHelper.printLabelModel = selectedPrintLabelModel;
                    reportHandlingUnitHelper.PrintLabel();

                    return;
                }
                else
                {
                    var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                    prn.Add("@Machine", machine.Code);
                    prn.Add("@Resource", resource.resourceName);
                    prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                    ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "636", "Tanımlı yazıcı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                }

                if (printLabelModels.Any(x => x.ProductId == reportHandlingUnitHelper.product.Id))
                {

                    reportHandlingUnitHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportHandlingUnitHelper.product.Id);
                    reportHandlingUnitHelper.PrintLabel();
                }
                else
                {
                    var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                    prn.Add("@Machine", machine.Code);
                    prn.Add("@Resource", resource.resourceName);
                    prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                    ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "637", "Tanımlı etiket dizaynı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }
        #endregion

        #region CHECKS
        private void PartHandlingUnitMaxQuotaCheck(Product selectedProduct, ShopOrderProductionDetail item)
        {
            try
            {
                var selectedPartHandling = partHandlingUnits.First(s => s.PartNo == selectedProduct.PartNo);
                double productionDetailsTotalQuantity = 0;
                foreach (var p in productionDetails.Where(x => x.ProductionStateID == StaticValues.specialCodeOk.Id))
                {
                    if (p.ProductID == selectedProduct.Id && p.BoxID == Guid.Empty)
                        productionDetailsTotalQuantity += (double)p.Quantity;
                }

                if (halfHandlingUnit.ContainsKey(item.ShopOrderOperationID))
                {
                    halfHandlingUnitProcess = halfHandlingUnit[item.ShopOrderOperationID];
                    productionDetailsTotalQuantity += (double)halfHandlingUnitProcess.Quantity;
                }
                else
                    halfHandlingUnitProcess = null;

                if (productionDetailsTotalQuantity >= selectedPartHandling.MaxQuantityCapacity)
                {
                    ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();
                    reportHandlingUnitHelper.product = selectedProduct;
                    reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID);
                    reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                    reportHandlingUnitHelper.machine = machine;
                    reportHandlingUnitHelper.resource = resource;
                    reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                    reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                    if (halfHandlingUnitProcess == null)
                    {
                        reportHandlingUnitHelper.shopOrderProductionDetails = productionDetails.Where(p => p.ProductID == selectedProduct.Id && p.BoxID == Guid.Empty && p.ProductionStateID == StaticValues.specialCodeOk.Id).OrderBy(x => x.serial).Take((int)selectedPartHandling.MaxQuantityCapacity).ToList();
                        reportHandlingUnitHelper.handlingUnit = new HandlingUnit();
                        reportHandlingUnitHelper.handlingUnit.PartHandlingUnitID = selectedPartHandling.Id;
                        reportHandlingUnitHelper.handlingUnit.ShopOrderID = item.ShopOrderOperationID;
                        reportHandlingUnitHelper.handlingUnit.ShopOrderProductionID = item.ShopOrderProductionID;
                        reportHandlingUnitHelper.handlingUnit.Quantity = reportHandlingUnitHelper.shopOrderProductionDetails.Sum(x => x.Quantity);

                        var myhandlingUnit = HandlingUnitManager.Current.Insert(reportHandlingUnitHelper.handlingUnit).ListData[0];
                        myhandlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);
                        HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);
                        reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                    }
                    else
                    {
                        reportHandlingUnitHelper.shopOrderProductionDetails = productionDetails.Where(p => p.ProductID == selectedProduct.Id && p.BoxID == Guid.Empty && p.ProductionStateID == StaticValues.specialCodeOk.Id).OrderBy(x => x.serial).Take((int)(selectedPartHandling.MaxQuantityCapacity - (double)halfHandlingUnitProcess.Quantity)).ToList();

                        reportHandlingUnitHelper.handlingUnit = HandlingUnitManager.Current.GetHandlingUnitByBarcodeOrSerial(halfHandlingUnitProcess.BoxBarcode);
                        reportHandlingUnitHelper.handlingUnit.Quantity += reportHandlingUnitHelper.shopOrderProductionDetails.Sum(x => x.Quantity);
                        HandlingUnitManager.Current.Update(reportHandlingUnitHelper.handlingUnit);
                        reportHandlingUnitHelper.handlingUnit.ManuelInput = 0;
                    }

                    //IFS SEND 
                    //IfsSendReport.IFSSendApproveOp(reportHandlingUnitHelper);

                    foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                    {
                        shopOrderProductionDetail.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                        shopOrderProductionDetail.HandlingUnitQuantity = shopOrderProductionDetail.Quantity;
                        var memItem = productionDetails.First(x => x.Id == item.Id);
                        memItem.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                        memItem.HandlingUnitQuantity = memItem.Quantity;
                        ShopOrderProductionDetailManager.Current.UpdateBoxIDAndHandlingUnitQuantity(shopOrderProductionDetail);
                    }

                    if (panelDetail.BoxFillsUp)
                    {
                        reportHandlingUnitHelper.printLabelModel = printLabelModels.First(p => p.ProductId == reportHandlingUnitHelper.product.Id && p.productionLabelType == ProductionLabelType.Box);

                        var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportHandlingUnitHelper.product, machine, resource, ProductionLabelType.Box);
                        if (selectedPrintLabelModel != null)
                        {
                            reportHandlingUnitHelper.printLabelModel = selectedPrintLabelModel;
                            reportHandlingUnitHelper.PrintLabel();
                        }
                        else
                        {
                            var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                            prn.Add("@Machine", machine.Code);
                            prn.Add("@Resource", resource.resourceName);
                            prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                            ToolsMessageBox.Warning(this, ToolsMessageBox.ReplaceParameters(MessageTextHelper.GetMessageText("000", "636", "Tanımlı yazıcı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                        }
                    }
                    else if (printLabelModels.Any(x => x.ProductId == reportHandlingUnitHelper.product.Id))
                    {
                        //reportHandlingUnitHelper.printLabelModel = printLabelModels.First(x => x.ProductId == reportHandlingUnitHelper.product.Id);
                        //reportHandlingUnitHelper.PrintLabel();
                    }
                    else
                    {
                        //ToolsMessageBox.Warning(this, "Tanımlı etiket dizaynı bulunamadı." +
                        //        $"\r\n\r\nReferans no: {reportHandlingUnitHelper.product.PartNo}" +
                        //        $"\r\nİş merkezi: {machine.Code}" +
                        //        $"\r\nKaynak: {resource.resourceName}" +
                        //        $"\r\nYazıcı tipi: {ProductionLabelType.Box.DescriptionAttr()}");
                    }

                    foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                    {
                        ShopOrderProductionDetailManager.Current.UpdateIfsReportedAndPrinted(shopOrderProductionDetail);
                    }

                    HandlingUnitManager.Current.UpdateBoxIfsSend(reportHandlingUnitHelper.handlingUnit);
                    if (!handlingUnits.Any(x => x.Id == reportHandlingUnitHelper.handlingUnit.Id))
                        handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);
                    halfHandlingUnitProcess = null;
                    halfHandlingUnit.Remove(item.ShopOrderOperationID);
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void PartHandlingUnitMaxQuotaCheckProcess(Product selectedProduct, ShopOrderProductionDetail item)
        {
            try
            {
                var maxQuantityCapacity = partHandlingUnits.First(s => s.PartNo == selectedProduct.PartNo).MaxQuantityCapacity;
                var quantityCapacity = maxQuantityCapacity;

                if (halfHandlingUnitProcess != null)
                    quantityCapacity -= (double)halfHandlingUnitProcess.Quantity;

                List<ShopOrderProductionDetail> shopOrderProductionDetailList;
                // IFS'e gönderilmeyen kasa içi miktarından fazla ürün üretildiyse bu ürünleri IFS'e gönder
                while (productionDetails.Where(x => x.ProductionStateID == StaticValues.specialCodeOk.Id).Sum(y => y.Quantity - y.HandlingUnitQuantity) >= (decimal)quantityCapacity)
                {
                    shopOrderProductionDetailList = new List<ShopOrderProductionDetail>();
                    decimal currentQuantity = 0;
                    bool flag = false;
                    foreach (var p in productionDetails.Where(pr => pr.ProductID == selectedProduct.Id && pr.ProductionStateID == StaticValues.specialCodeOk.Id && pr.Quantity > pr.HandlingUnitQuantity).ToList())
                    {
                        shopOrderProductionDetailList.Add(p);

                        if ((p.Quantity - p.HandlingUnitQuantity) + currentQuantity >= (decimal)quantityCapacity)
                        {
                            currentQuantity += p.Quantity - p.HandlingUnitQuantity;
                            p.HandlingUnitQuantity = p.Quantity - (currentQuantity - (decimal)quantityCapacity);
                            ShopOrderProductionDetailManager.Current.Update(p);
                            flag = true;
                        }
                        else
                        {
                            currentQuantity += p.Quantity - p.HandlingUnitQuantity;
                            p.HandlingUnitQuantity = p.Quantity;
                            ShopOrderProductionDetailManager.Current.Update(p);

                            if (currentQuantity >= (decimal)quantityCapacity)
                            {
                                p.HandlingUnitQuantity = p.Quantity - (currentQuantity - (decimal)quantityCapacity);
                                ShopOrderProductionDetailManager.Current.Update(p);

                                flag = true;
                            }
                        }

                        if (flag)
                            break;
                    }

                    ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();
                    reportHandlingUnitHelper.product = selectedProduct;
                    ReportProcessHandlingUnitHelper report = new ReportProcessHandlingUnitHelper();//PrintLabel için oluşturuldu

                    if (panelDetail.PrintProductBarcode || panelDetail.BoxFillsUp)
                        reportHandlingUnitHelper.printLabelModel = printLabelModels.First(pd => pd.ProductId == reportHandlingUnitHelper.product.Id && pd.productionLabelType == ProductionLabelType.Box);

                    reportHandlingUnitHelper.machine = machine;
                    reportHandlingUnitHelper.resource = resource;
                    reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                    reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                    reportHandlingUnitHelper.shopOrderProductionDetails = shopOrderProductionDetailList;

                    var queryResult = shopOrderProductionDetailList.GroupBy(p => p.ShopOrderOperationID).ToList();

                    string boxBarcode = (halfHandlingUnitProcess != null ? halfHandlingUnitProcess.BoxBarcode : "");
                    for (int i = 0; i < queryResult.Count; i++)
                    {
                        var details = shopOrderProductionDetailList.Where(x => x.ShopOrderOperationID == queryResult[i].Key).ToList();
                        reportHandlingUnitHelper.shopOrderOperation = ShopOrderOperationManager.Current.GetShopOrderOperationById(queryResult[i].Key);
                        reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.Single(x => x.Id == queryResult[i].Key);

                        reportHandlingUnitHelper.handlingUnit = null;
                        if (halfHandlingUnitProcess != null)
                        {
                            var hu = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(queryResult[i].Key);
                            if (hu != null)
                                hu = hu.Where(x => x.Barcode == halfHandlingUnitProcess.BoxBarcode).ToList();
                            if (hu.HasEntries())
                                reportHandlingUnitHelper.handlingUnit = hu[0];
                        }

                        if (reportHandlingUnitHelper.handlingUnit == null)
                        {
                            HandlingUnit handlingUnit = new HandlingUnit();
                            handlingUnit.PartHandlingUnitID = partHandlingUnits[0].Id;
                            handlingUnit.ShopOrderID = queryResult[i].Key;
                            handlingUnit.ShopOrderProductionID = shopOrderProduction.Id;
                            handlingUnit.Quantity = details.Sum(x => x.Quantity);
                            handlingUnit.ManuelInput = 0;
                            handlingUnit.Description = "PLC";
                            handlingUnit.Barcode = boxBarcode;
                            var myhandlingUnit = HandlingUnitManager.Current.Insert(handlingUnit).ListData[0];

                            if (boxBarcode.Length == 0)
                            {
                                boxBarcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);
                                myhandlingUnit.Barcode = boxBarcode;
                                HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);
                            }

                            reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                        }
                        else
                        {
                            reportHandlingUnitHelper.handlingUnit.Quantity += details.Sum(x => x.Quantity);
                            HandlingUnitManager.Current.Update(reportHandlingUnitHelper.handlingUnit);
                            reportHandlingUnitHelper.handlingUnit.ManuelInput = 0;
                        }
                        if (!handlingUnits.Any(x => x.Id == reportHandlingUnitHelper.handlingUnit.Id))
                            handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);

                        foreach (var shopOrderProductionDetail in details)
                        {
                            shopOrderProductionDetail.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                            productionDetails.First(x => x.Id == item.Id).BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                            ShopOrderProductionDetailManager.Current.UpdateBoxID(shopOrderProductionDetail);
                        }

                        reportHandlingUnitHelper.shopOrderProductionDetails = details;
                        //IfsSendReport.IFSSendApproveOp(reportHandlingUnitHelper);

                        reportHandlingUnitHelper.handlingUnit.SendIfs = true;
                        reportHandlingUnitHelper.handlingUnit.SendDate = DateTime.Now;
                        HandlingUnitManager.Current.UpdateBoxIfsSend(reportHandlingUnitHelper.handlingUnit, DateTime.Now);

                        report.shopOrderOperations.Add(reportHandlingUnitHelper.shopOrderOperation);
                        report.vwShopOrderGridModels.Add(reportHandlingUnitHelper.vwShopOrderGridModel);
                        report.handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);
                    }

                    if (halfHandlingUnitProcess != null)
                    {
                        var allHandlingUnits = HandlingUnitManager.Current.GetHandlingUnitsByBarcode(halfHandlingUnitProcess.BoxBarcode);
                        foreach (var hu in allHandlingUnits)
                        {
                            if (!report.handlingUnits.Any(x => x.Id == hu.Id))
                                report.handlingUnits.Add(hu);
                        }
                    }

                    if (panelDetail.BoxFillsUp)
                    {
                        report.machine = machine;
                        report.resource = resource;
                        report.shopOrderProduction = shopOrderProduction;
                        report.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                        report.shopOrderProductionDetails = shopOrderProductionDetailList;
                        report.printLabelModel = reportHandlingUnitHelper.printLabelModel;
                        report.PrintLabel();
                    }

                    quantityCapacity = maxQuantityCapacity;
                    halfHandlingUnitProcess = null;
                    halfHandlingUnit.Clear();
                }
            }
            catch
            {
            }
        }

        private void PartHandlingUnitMaxQuotaCheckNoProductBarcode(Product selectedProduct, ShopOrderProductionDetail item)
        {
            var maxQuantityCapacity = partHandlingUnits.First(s => s.PartNo == selectedProduct.PartNo).MaxQuantityCapacity;
            var quantityCapacity = maxQuantityCapacity;

            if (halfHandlingUnit.ContainsKey(item.ShopOrderOperationID))
            {
                halfHandlingUnitProcess = halfHandlingUnit[item.ShopOrderOperationID];
                quantityCapacity -= (double)halfHandlingUnitProcess.Quantity;
            }
            else
                halfHandlingUnitProcess = null;

            List<ShopOrderProductionDetail> shopOrderProductionDetailList;
            // IFS'e gönderilmeyen kasa içi miktarından fazla ürün üretildiyse bu ürünleri IFS'e gönder
            while (productionDetails.Where(p => p.ProductID == selectedProduct.Id && p.ProductionStateID == StaticValues.specialCodeOk.Id).Sum(y => y.Quantity - y.HandlingUnitQuantity) >= (decimal)quantityCapacity)
            {
                shopOrderProductionDetailList = new List<ShopOrderProductionDetail>();
                decimal currentQuantity = 0;
                bool flag = false;
                foreach (var p in productionDetails.Where(pr => pr.ProductID == selectedProduct.Id && pr.ProductionStateID == StaticValues.specialCodeOk.Id && pr.Quantity > pr.HandlingUnitQuantity).ToList())
                {
                    shopOrderProductionDetailList.Add(p);

                    if ((p.Quantity - p.HandlingUnitQuantity) + currentQuantity >= (decimal)quantityCapacity)
                    {
                        currentQuantity += p.Quantity - p.HandlingUnitQuantity;
                        p.HandlingUnitQuantity = p.Quantity - (currentQuantity - (decimal)quantityCapacity);
                        ShopOrderProductionDetailManager.Current.Update(p);
                        flag = true;
                    }
                    else
                    {
                        currentQuantity += p.Quantity - p.HandlingUnitQuantity;
                        p.HandlingUnitQuantity = p.Quantity;
                        ShopOrderProductionDetailManager.Current.Update(p);

                        if (currentQuantity >= (decimal)quantityCapacity)
                        {
                            p.HandlingUnitQuantity = p.Quantity - (currentQuantity - (decimal)quantityCapacity);
                            ShopOrderProductionDetailManager.Current.Update(p);

                            flag = true;
                        }
                    }

                    if (flag)
                        break;
                }

                ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();

                reportHandlingUnitHelper.product = selectedProduct;

                if (panelDetail.PrintProductBarcode || panelDetail.BoxFillsUp)
                    reportHandlingUnitHelper.printLabelModel = printLabelModels.First(pd => pd.ProductId == reportHandlingUnitHelper.product.Id && pd.productionLabelType == ProductionLabelType.Box);

                reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                reportHandlingUnitHelper.machine = machine;
                reportHandlingUnitHelper.resource = resource;
                reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                if (halfHandlingUnitProcess == null)
                {
                    reportHandlingUnitHelper.handlingUnit = new HandlingUnit();
                    reportHandlingUnitHelper.handlingUnit.PartHandlingUnitID = partHandlingUnits.First(x => x.PartNo == reportHandlingUnitHelper.product.PartNo).Id;//partHandlingUnitID;
                    reportHandlingUnitHelper.handlingUnit.ShopOrderID = reportHandlingUnitHelper.shopOrderOperation.Id;
                    reportHandlingUnitHelper.handlingUnit.ShopOrderProductionID = reportHandlingUnitHelper.shopOrderProduction.Id;
                    reportHandlingUnitHelper.handlingUnit.Quantity = (decimal)quantityCapacity;
                    reportHandlingUnitHelper.handlingUnit.ManuelInput = (decimal)quantityCapacity;

                    var myhandlingUnit = HandlingUnitManager.Current.Insert(reportHandlingUnitHelper.handlingUnit).ListData[0];
                    myhandlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);
                    HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);
                    reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                }
                else
                {
                    reportHandlingUnitHelper.handlingUnit = HandlingUnitManager.Current.GetHandlingUnitByBarcodeOrSerial(halfHandlingUnitProcess.BoxBarcode);
                    reportHandlingUnitHelper.handlingUnit.Quantity = (decimal)maxQuantityCapacity;
                    reportHandlingUnitHelper.handlingUnit.ManuelInput = 0;

                    HandlingUnitManager.Current.Update(reportHandlingUnitHelper.handlingUnit);
                }

                reportHandlingUnitHelper.shopOrderProductionDetails = shopOrderProductionDetailList;

                foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                {
                    shopOrderProductionDetail.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                    productionDetails.First(x => x.Id == item.Id).BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(shopOrderProductionDetail);
                }

                //IfsSendReport.IFSSendApproveOp(reportHandlingUnitHelper);

                if (panelDetail.BoxFillsUp)
                    reportHandlingUnitHelper.PrintLabel();

                quantityCapacity = maxQuantityCapacity;
                halfHandlingUnitProcess = null;
                halfHandlingUnit.Remove(item.ShopOrderOperationID);
            }
        }

        public void PartHandlingUnitMaxQuotaCheckButtonAndReadBarcode(Product selectedProduct, ShopOrderProductionDetail item)
        {
            try
            {
                var maxQuantityCapacity = partHandlingUnits.First(s => s.PartNo == selectedProduct.PartNo).MaxQuantityCapacity;
                var quantityCapacity = maxQuantityCapacity;

                if (halfHandlingUnit.ContainsKey(item.ShopOrderOperationID))
                {
                    halfHandlingUnitProcess = halfHandlingUnit[item.ShopOrderOperationID];
                    quantityCapacity -= (double)halfHandlingUnitProcess.Quantity;
                }
                else
                    halfHandlingUnitProcess = null;

                if (selectedProduct.unitMeas == Units.ad.ToText() && productionDetails.Where(p => p.ProductID == selectedProduct.Id && p.BoxID == Guid.Empty && p.serial.ToString() == p.Barcode && p.ProductionStateID == StaticValues.specialCodeOk.Id).ToList().Count >= quantityCapacity)
                {
                    if (OperatorPanelConfigurationHelper.DoAutomaticProductionNotification(shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID)) && products.First().unitMeas == Units.ad.ToText())//adet için kasa dolduğunda otomatik bildirim gönder
                    {
                        ReportHandlingUnitHelper reportHandlingUnitHelper = new ReportHandlingUnitHelper();
                        reportHandlingUnitHelper.product = selectedProduct;
                        if (panelDetail.PrintProductBarcode || panelDetail.BoxFillsUp)
                            reportHandlingUnitHelper.printLabelModel = printLabelModels.First(p => p.ProductId == reportHandlingUnitHelper.product.Id && p.productionLabelType == ProductionLabelType.Box);
                        reportHandlingUnitHelper.shopOrderOperation = shopOrderOperations.First(x => x.Id == item.ShopOrderOperationID);
                        reportHandlingUnitHelper.vwShopOrderGridModel = vw_ShopOrderGridModels.First(x => x.Id == item.ShopOrderOperationID);
                        reportHandlingUnitHelper.machine = machine;
                        reportHandlingUnitHelper.resource = resource;
                        reportHandlingUnitHelper.shopOrderProduction = shopOrderProduction;
                        reportHandlingUnitHelper.user = Users.First(x => x.Role == Users.Max(y => y.Role));
                        if (halfHandlingUnitProcess == null)
                        {
                            reportHandlingUnitHelper.handlingUnit = new HandlingUnit();
                            reportHandlingUnitHelper.handlingUnit.PartHandlingUnitID = partHandlingUnits.First(x => x.PartNo == reportHandlingUnitHelper.product.PartNo).Id;//partHandlingUnitID;
                            reportHandlingUnitHelper.handlingUnit.ShopOrderID = reportHandlingUnitHelper.shopOrderOperation.Id;
                            reportHandlingUnitHelper.handlingUnit.ShopOrderProductionID = reportHandlingUnitHelper.shopOrderProduction.Id;
                            reportHandlingUnitHelper.handlingUnit.Quantity = (decimal)maxQuantityCapacity;

                            var myhandlingUnit = HandlingUnitManager.Current.Insert(reportHandlingUnitHelper.handlingUnit).ListData[0];
                            myhandlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(myhandlingUnit.Serial, reportHandlingUnitHelper);
                            HandlingUnitManager.Current.UpdateBoxBarcode(myhandlingUnit);
                            reportHandlingUnitHelper.handlingUnit = myhandlingUnit;
                        }
                        else
                        {
                            reportHandlingUnitHelper.handlingUnit = HandlingUnitManager.Current.GetHandlingUnitByBarcodeOrSerial(halfHandlingUnitProcess.BoxBarcode);
                            reportHandlingUnitHelper.handlingUnit.Quantity = (decimal)maxQuantityCapacity;

                            HandlingUnitManager.Current.Update(reportHandlingUnitHelper.handlingUnit);
                            reportHandlingUnitHelper.handlingUnit.ManuelInput = 0;
                        }

                        reportHandlingUnitHelper.shopOrderProductionDetails = productionDetails.Where(p => p.BoxID == Guid.Empty && p.ProductionStateID == StaticValues.specialCodeOk.Id).OrderBy(x => x.serial).Take((int)quantityCapacity).ToList();

                        foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                        {
                            shopOrderProductionDetail.BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                            productionDetails.First(x => x.Id == item.Id).BoxID = reportHandlingUnitHelper.handlingUnit.Id;
                            ShopOrderProductionDetailManager.Current.UpdateBoxID(shopOrderProductionDetail);
                        }

                        //IfsSendReport.IFSSendApproveOp(reportHandlingUnitHelper);

                        if (panelDetail.BoxFillsUp)
                        {
                            var selectedPrintLabelModel = PrintLabelHelper.GetLabelModel(reportHandlingUnitHelper.product, machine, resource, ProductionLabelType.Box);
                            if (selectedPrintLabelModel != null)
                            {
                                reportHandlingUnitHelper.printLabelModel = selectedPrintLabelModel;
                                reportHandlingUnitHelper.PrintLabel();
                            }
                            else
                            {
                                var prn = reportHandlingUnitHelper.product.PartNo.CreateParameters("@PartNo");
                                prn.Add("@Machine", machine.Code);
                                prn.Add("@Resource", resource.resourceName);
                                prn.Add("@PrinterType", ProductionLabelType.Box.ToText());
                                ToolsMessageBox.Warning(this, MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "636", "Tanımlı yazıcı bulunamadı.\n\nReferans no: @PartNo\nİş merkezi: @Machine\nKaynak: @Resource\nYazıcı tipi: @PrinterType", "Message"), prn));
                            }
                        }
                        foreach (var shopOrderProductionDetail in reportHandlingUnitHelper.shopOrderProductionDetails)
                        {
                            ShopOrderProductionDetailManager.Current.UpdateIfsReportedAndPrinted(shopOrderProductionDetail);
                        }

                        HandlingUnitManager.Current.UpdateBoxIfsSend(reportHandlingUnitHelper.handlingUnit);
                        if (!handlingUnits.Any(x => x.Id == reportHandlingUnitHelper.handlingUnit.Id))
                            handlingUnits.Add(reportHandlingUnitHelper.handlingUnit);
                        SetLabelsTextValue();

                        halfHandlingUnitProcess = null;
                        halfHandlingUnit.Remove(item.ShopOrderOperationID);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }
        #endregion

        #region OPCClient -> HandleDataChanged
        private decimal lastCycleNoProductionSignal;
        private decimal _cycleNoProductionSignal;
        public decimal cycleNoProductionSignal
        {
            get
            {
                return _cycleNoProductionSignal;
            }
            set
            {
                try
                {

                    if (lastCycleNoProductionSignal == value)
                        return;

                    // Cache'i güncelle
                    lastCycleNoProductionSignal = value;

                    if (lastCycleNoProductionSignal == 1)
                        CycleNoProductionSignalMessageSender();
                    else
                        return;

                    // Cache'i güncelle
                    _cycleNoProductionSignal = value;
                }
                catch
                {

                    throw;
                }

            }
        }

        private decimal _notShopOrderCounter;
        private decimal lastNotShopOrderCounter;
        public decimal notShopOrderCounter
        {
            get
            {
                return _notShopOrderCounter;
            }
            set
            {
                try
                {

                    if (value == -1)
                    {
                        _notShopOrderCounter = 0;
                    }
                    // Eğer değer değişmemişse çık
                    if (value == lastNotShopOrderCounter)
                        return;

                    // Cache'i güncelle
                    lastNotShopOrderCounter = value;

                    DateTime dt = DateTime.Now;

                    // Gelen Parça Sayısını Hesaplama
                    var count = value - _notShopOrderCounter;
                    if (count <= 0)
                        return;

                    // Cache'i güncelle
                    _notShopOrderCounter = value;
                    for (int i = 0; i < count; i++)
                    {
                        InsertUnexpectedProductionSignal(dt);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        private decimal _scrapCounter;
        public decimal scrapCounter
        {
            get { return _scrapCounter; }

            set
            {
                DateTime dt = DateTime.Now;
                if (vw_ShopOrderGridModels == null || vw_ShopOrderGridModels.Count == 0)
                    return;

                var count = value - _scrapCounter;
                if (count <= 0)
                    return;


                foreach (var item in vw_ShopOrderGridModels)
                {

                    var scrapProduct = products.Single(x => x.Id == item.ProductID);
                    var scrapProductionDetail = ShopOrderProductionDetailHelper.CreateAndInsertScrapProductionDetail(this, item, scrapProduct, true, Users[0], Users.Count);
                    productionDetails.Add(scrapProductionDetail);
                    ShopOrderProductionDetailHelper.PrintScrapProduction(scrapProduct, shopOrderProduction, vw_ShopOrderGridModels.First(x => x.ProductID == scrapProduct.Id), scrapProductionDetail, machine, resource, Users[0]);
                }

                _scrapCounter = value;
            }
        }
        #endregion

        #region NOTIFICATION
        private void InsertUnexpectedProductionSignal(DateTime dateTime)
        {
            try
            {
                KafkaService kafkaService = null;
                UnexpectedProductionSignal ups = new UnexpectedProductionSignal();
                decimal count = 1;

                ups.BranchID = StaticValues.branch.Id;
                ups.GroupID = this.machine.GroupId;
                ups.WorkCenterID = this.machine.Id;
                ups.ResourceID = this.resource.Id;
                ups.Quantity = count;
                if (interruptionCause != null || faults.Count > 0 && faults.Last().Active)
                    ups.InterruptionState = true;
                UnexpectedProductionSignalManager.Current.Insert(ups);
                NotificationHelper.SendKafkaAndEmailNotification("ISEMIRSIZURETIM_ALERT_TOPIC", messageDescription: "İş Emirsiz Üretim Yapılmaktadır",
                                                                 "İş Emirsiz Üretim Yapılmaktadır",
                                                                 "Alert",
                                                                 this.machine,
                                                                 this.resource,
                                                                 ups.WorkCenterID,
                                                                 TopicDefinitions.alert);
            }
            catch
            {
            }
        }

        private void CycleNoProductionSignalMessageSender()
        {
            KafkaService kafkaService = null;
            NotificationHelper.SendKafkaAndEmailNotification("URETIMSINYALIYOK_ALERT_TOPIC", messageDescription: "İş Emri Açık Çevrim Aşımı Üretim Sinyali Yok",
                                                 "Makineden Üretim Sinyali Gelmiyor",
                                                 "Alert",
                                                 this.machine,
                                                 this.resource,
                                                 this.machine.Id,
                                                 TopicDefinitions.alert);
        }
        #endregion

        #region HALF FILLED HANDLING UNITS
        private void CheckIfHalfFilledHandlingUnitsExistInShopOrder()
        {
            halfHandlingUnitProcess = null;
            halfHandlingUnit.Clear();

            // Check Panel Parameter
            if (!panelDetail.HalfCaseStatus)
                return;

            if (processNewActive)
            {
                if (OperatorPanelConfigurationHelper.PrintBoxLabel(shopOrderOperations[0]))
                {
                    // Tüm orderOperationlar içinde tüm kasaların listesi
                    var handlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(shopOrderOperations.Select(x => x.Id).ToList());

                    if (!handlingUnits.HasEntries())
                        return;

                    // Kasalar barkod ile gruplanıp miktar toplanıyor
                    var queryResult = from h in handlingUnits
                                      group h by new { h.Barcode } into grp
                                      select new
                                      {
                                          barcode = grp.Key.Barcode,
                                          quantity = grp.Sum(q => q.Quantity),
                                          lotCount = grp.Count()
                                      };

                    var maxQty = (decimal)partHandlingUnits.First(x => x.PartNo == vw_ShopOrderGridModels[0].PartNo).MaxQuantityCapacity;
                    var halfHandlingUnits = queryResult.Where(x => x.quantity < maxQty).ToList();

                    // Yarım Kasa bulunmadı
                    if (halfHandlingUnits.Count == 0)
                        return;

                    List<ProcessHandlingUnitModel> processHandlingUnitModels = new List<ProcessHandlingUnitModel>();
                    foreach (var query in halfHandlingUnits)
                    {
                        ProcessHandlingUnitModel handlingUnitModel = new ProcessHandlingUnitModel();
                        handlingUnitModel.BoxBarcode = query.barcode;
                        handlingUnitModel.Quantity = query.quantity;
                        handlingUnitModel.LotCount = query.lotCount;
                        handlingUnitModel.CreatedAt = handlingUnits.Where(x => x.Barcode == query.barcode).OrderBy(o => o.CreatedAt).First().CreatedAt;
                        processHandlingUnitModels.Add(handlingUnitModel);
                    }

                    // Yarım kasa bulundu
                    if (processHandlingUnitModels.Count == 1) // 1 adet yarım kasa bulundu. Mesaj göster ve devam.
                    {
                        halfHandlingUnitProcess = processHandlingUnitModels[0];
                        var prm = Text.CreateParameters("@Text");
                        prm.Add("@Barcode", processHandlingUnitModels[0].BoxBarcode);
                        ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "930", "@Text\r\n@Barcode Barkodlu 1 adet Yarım Kasa bulundu ve otomatik kasa bildirimlerinde kullanılacak", "Message"), prm);
                    }
                    else // 1'den fazla yarım kasa bulundu. Seçmek için pencere aç
                    {
                        var frm = new FrmSelectHandlingUnit(processHandlingUnitModels, this.Text, products.First(y => y.Id == shopOrderOperations[0].PartID).Description);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            halfHandlingUnitProcess = frm.selectedHandlingUnit;
                            var prm = Text.CreateParameters("@Text");
                            prm.Add("@Barcode", frm.selectedHandlingUnit.BoxBarcode);
                            ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "931", "@Text\r\nOtomatik kasa bildirimlerinde kullanmak üzere @Barcode Barkodlu Yarım Kasa secildi", "Message"), prm);
                        }
                    }
                }
            }
            else
            {
                foreach (var shopOrderOperation in shopOrderOperations)
                {
                    if (OperatorPanelConfigurationHelper.PrintBoxLabel(shopOrderOperation))
                    {
                        // Tüm orderOperationlar içinde tüm kasaların listesi
                        var handlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(shopOrderOperation.Id);

                        if (!handlingUnits.HasEntries())
                            return;

                        // Kasalar barkod ile gruplanıp miktar toplanıyor
                        var queryResult = from h in handlingUnits
                                          group h by new { h.Barcode } into grp
                                          select new
                                          {
                                              barcode = grp.Key.Barcode,
                                              quantity = grp.Sum(q => q.Quantity),
                                              lotCount = grp.Count()
                                          };

                        var maxQty = (decimal)partHandlingUnits.First(x => x.PartNo == products.First(y => y.Id == shopOrderOperation.PartID).PartNo).MaxQuantityCapacity;
                        var halfHandlingUnits = queryResult.Where(x => x.quantity < maxQty).ToList();

                        // Yarım Kasa bulunmadı
                        if (halfHandlingUnits.Count == 0)
                            return;

                        List<ProcessHandlingUnitModel> processHandlingUnitModels = new List<ProcessHandlingUnitModel>();
                        foreach (var query in halfHandlingUnits)
                        {
                            ProcessHandlingUnitModel handlingUnitModel = new ProcessHandlingUnitModel();
                            handlingUnitModel.BoxBarcode = query.barcode;
                            handlingUnitModel.Quantity = query.quantity;
                            handlingUnitModel.LotCount = query.lotCount;
                            handlingUnitModel.CreatedAt = handlingUnits.Where(x => x.Barcode == query.barcode).OrderBy(o => o.CreatedAt).First().CreatedAt;
                            processHandlingUnitModels.Add(handlingUnitModel);
                        }

                        // Yarım kasa bulundu
                        if (processHandlingUnitModels.Count == 1) // 1 adet yarım kasa bulundu. Mesaj göster ve devam.
                        {
                            if (!halfHandlingUnit.ContainsKey(shopOrderOperation.Id))
                                halfHandlingUnit.Add(shopOrderOperation.Id, processHandlingUnitModels[0]);
                            var prm = Text.CreateParameters("@Text");
                            prm.Add("@OrderNo", shopOrderOperation.orderNo);
                            prm.Add("@Barcode", processHandlingUnitModels[0].BoxBarcode);
                            ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "932", "@Text\r\nİş Emri No: @OrderNo\r\n@Barcode Barkodlu 1 adet Yarım Kasa bulundu ve otomatik kasa bildirimlerinde kullanılacak", "Message"), prm);
                        }
                        else // 1'den fazla yarım kasa bulundu. Seçmek için pencere aç
                        {
                            var frm = new FrmSelectHandlingUnit(processHandlingUnitModels, this.Text, products.First(y => y.Id == shopOrderOperation.PartID).Description);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                if (!halfHandlingUnit.ContainsKey(shopOrderOperation.Id))
                                    halfHandlingUnit.Add(shopOrderOperation.Id, frm.selectedHandlingUnit);
                                var prm = Text.CreateParameters("@Text");
                                prm.Add("@OrderNo", shopOrderOperation.orderNo);
                                prm.Add("@Barcode", frm.selectedHandlingUnit.BoxBarcode);
                                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "933", "@Text\r\nİş Emri No: @OrderNo\r\nOtomatik kasa bildirimlerinde kullanmak üzere @Barcode Barkodlu Yarım Kasa secildi", "Message"), prm);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region WAITING DURATION INTERRPTION
        private void InsertWaitingDurationInterruptionCouse()
        {
            InterruptionCause cause = new InterruptionCause();
            cause.ShiftID = StaticValues.shift.Id;
            cause.CompanyPersonID = Guid.Empty;
            cause.WorkCenterID = machine.Id;
            cause.CouseID = StaticValues.opInterruptionCauseBekleme.Id;
            cause.State = true;
            cause.Description = "İş Emri Seçilmedi";
            cause.InterruptionStartDate = DateTime.Now;
            cause.CreatedAt = DateTime.Now;
            cause.UpdatedAt = DateTime.Now;
            cause.Active = false;
            cause.OnlineState = false;
            InterruptionCauseManager.Current.Insert(cause);

        }
        private void UpdateWaitingDurationInterruptionCouse()
        {
            var cause = InterruptionCauseManager.Current.GetInterruptionCauseWaiting(StaticValues.opInterruptionCauseBekleme.Id, machine.Id);

            if (cause != null)
            {
                var shifts = ShiftManager.Current.GetShifts(StaticValues.branch.Id);
                bool flag = false;
                do
                {
                    foreach (var shift in shifts)
                    {
                        var newShiftStartDate = new DateTime(cause.InterruptionStartDate.Year, cause.InterruptionStartDate.Month, cause.InterruptionStartDate.Day, shift.StartDate.Hour, shift.StartDate.Minute, shift.StartDate.Second, shift.StartDate.Millisecond);

                        int differentDay = shift.StartDate.Hour > shift.EndDate.Hour ? 1 : 0;

                        var newShiftEndDate = new DateTime(cause.InterruptionStartDate.Year, cause.InterruptionStartDate.Month, cause.InterruptionStartDate.Day + differentDay, shift.EndDate.Hour, shift.EndDate.Minute, shift.EndDate.Second, shift.EndDate.Millisecond);

                        if (cause.InterruptionStartDate >= newShiftStartDate && cause.InterruptionStartDate <= newShiftEndDate)//Seçili shift
                        {
                            if (DateTime.Now < newShiftEndDate)
                            {
                                InterruptionCauseManager.Current.UpdateInterruptionCauseEndDate(cause.Id);
                                flag = false;
                                break;
                            }
                            else if (DateTime.Now > newShiftEndDate)
                            {
                                InterruptionCauseManager.Current.UpdateInterruptionCauseEndDate(cause.Id, newShiftEndDate);

                                cause.Id = Guid.NewGuid();
                                cause.InterruptionStartDate = newShiftEndDate.AddMilliseconds(1);
                                cause.State = false;
                                InterruptionCauseManager.Current.Insert(cause);
                                flag = true;
                            }
                        }
                    }
                } while (flag);
            }
        }
        #endregion

        #region SET MACHINE STATE COLOR
        public void SetMachineStateColor(MachineStateColor machineStateColor)
        {

            System.Drawing.Color color = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
            string machineState = "";
            //string caption = "";
            switch (machineStateColor)
            {
                case MachineStateColor.Run:
                    color = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
                    machineState = MachineStateColor.Run.ToText();
                    //caption = $"{machine.Definition}-{resource.resourceName}";
                    break;
                case MachineStateColor.MachineDown:
                    color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(214)))), ((int)(((byte)(5)))));
                    //caption = $"{machine.Definition}-{resource.resourceName} / {opInterruptionCause.description}";
                    xtcDetails.SelectedTabPage = xtpInterruption;
                    machineState = MachineStateColor.MachineDown.ToText();
                    break;
                case MachineStateColor.Fault:
                    color = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(82)))), ((int)(((byte)(81)))));
                    var prm = faults.Last().ErrDescription.CreateParameters("@Fault");
                    machineState = ToolsMessageBox.ReplaceParameters(MachineStateColor.Fault.ToText() + ": @Fault", prm);
                    break;
                case MachineStateColor.Setup:
                    color = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
                    machineState = MachineStateColor.Setup.ToText();
                    break;
                case MachineStateColor.ShopOrderWaiting:
                    color = System.Drawing.SystemColors.GrayText;
                    machineState = MachineStateColor.ShopOrderWaiting.ToText();
                    break;


            }
            pnlMachineStateColor.BackColor = color;
            lblMachineState.Text = machineState;
        }
        #endregion

        #region CreateShopOrderProductionDetails
        public void CreateShopOrderProductionDetails()
        {
            string orderNo = "";
            try
            {
                shopOrderProductionDetails.Clear();
                if (processNewActive)
                {
                    shopOrderProductionDetails.Add(CreateShopOrderProductionDetail(vw_ShopOrderGridModelActive));
                }
                else
                {
                    foreach (var item in vw_ShopOrderGridModels)
                    {
                        shopOrderProductionDetails.Add(CreateShopOrderProductionDetail(item));
                    }
                }
            }
            catch (Exception ex)
            {
                var prm = orderNo.CreateParameters("@OrderNo");
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "926", "İş Emrine bağlı bir sorun oluştu: \r\nİş Emri No: @OrderNo", "Message"), ex);
            }
        }

        private ShopOrderProductionDetail CreateShopOrderProductionDetail(vw_ShopOrderGridModel shopOrderGridModel)
        {
            ShopOrderProductionDetail productionDetail = new ShopOrderProductionDetail();
            productionDetail.ShopOrderOperationID = shopOrderGridModel.Id;
            productionDetail.ShopOrderProductionID = shopOrderProduction.Id;
            productionDetail.WorkCenterID = machine.Id;
            productionDetail.ResourceID = resource.Id;

            if (productionDetails.Count > 0 && productionDetails.Last().ShopOrderProductionID == shopOrderProduction.Id)
                productionDetail.StartDate = productionDetails.Last().EndDate;
            else
                productionDetail.StartDate = DateTime.Now;

            productionDetail.EndDate = DateTime.Now;
            productionDetail.Barcode = "";
            productionDetail.Unit = shopOrderGridModel.unitMeas;
            productionDetail.Quantity = 1;
            productionDetail.ProductID = shopOrderGridModel.ProductID;
            try
            {
                productionDetail.ParHandlingUnitID = partHandlingUnits.First(h => h.PartNo == shopOrderGridModel.PartNo).Id;
            }
            catch
            {
                throw new Exception("IFS üzerinde tanımlı kasa bulunmadı. lütfen sistem yöneticiniz ile temasa geçiniz.");
            }
            productionDetail.ShiftId = StaticValues.shift.Id;
            if (shopOrderGridModel.alan2 == null || shopOrderGridModel.alan2.Trim() == "")
                productionDetail.Divisor = 1;
            else
            {
                decimal v = 1;
                Decimal.TryParse(shopOrderGridModel.alan2, out v);
                productionDetail.Divisor = v;
            }
            productionDetail.ProductionStateID = StaticValues.specialCodeOk.Id;
            productionDetail.OrderNo = shopOrderGridModel.orderNo;
            productionDetail.OperationNo = shopOrderGridModel.operationNo;
            productionDetail.CrewSize = Users.Count;

            return productionDetail;
        }
        #endregion

        #region NEW PROCESS CONTROL
        private bool _processNewActive;
        public bool processNewActive
        {
            get { return _processNewActive; }
            set
            {
                _processNewActive = value;
            }
        }
        #endregion

        #region Energy Timer to read consumed energy when shop order is running
        private System.Threading.Timer timer;
        private bool isRunning = false;
        //private EnergyDBHelper _energyDBHelper;
        //public EnergyDBHelper energyDBHelper
        //{
        //    get
        //    {
        //        if (_energyDBHelper == null)
        //            _energyDBHelper = new EnergyDBHelper();
        //        return _energyDBHelper;
        //    }
        //}
        public decimal startEnergy = 0;

        public void StartEnergyTimer()
        {
            if (isRunning)
                return;

            isRunning = true;
            timer = new System.Threading.Timer(async _ => await FetchEnergyAsync(), null, 0, 5000);
            lblDescription8.Text = "Tüketilen Enerji";
        }
        public void StopEnergyTimer()
        {
            if (!isRunning)
                return;

            isRunning = false;
            timer?.Change(Timeout.Infinite, 0);
            timer?.Dispose();
            timer = null;

            lblValue8.Text = "-";
            lblDescription8.Text = "-";
        }
        private async Task FetchEnergyAsync()
        {
            try
            {
                //decimal currentEnergy = await Task.Run(() => energyDBHelper.GetCurrentTotalEnergyFromDB(resource.Id));
                //decimal totalEnergy = currentEnergy - startEnergy;
                decimal totalEnergy = 0;
                lblValue8.Text = $"{totalEnergy}";
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region LABELS TEXT CHANGES
        public void RefreshTargetProduction()
        {
            var value = Convert.ToDecimal(lblCurrentAmount.Text);

            lscTargetProduction.MaxValue = (float)TargetQty;
            lscTargetProduction.Value = (float)value;
            lsrbcTargetProduction.AppearanceRangeBar.ContentBrush = blueBO;

            if (value > 0)
            {
                if (value >= (int)TargetQty)
                    lsrbcTargetProduction.AppearanceRangeBar.ContentBrush = redBO;
                else if (value >= (int)((double)TargetQty * 0.9))
                    lsrbcTargetProduction.AppearanceRangeBar.ContentBrush = yellowBO;
            }

            gcTargetProductionAmount.Refresh();
        }

        public void SetLabelsTextValue()
        {
            decimal realizeAmount, currentAmount, totalNotOkProduction, totalHandlingUnitQuantityThisProduction = 0, PLCCounter;
            //İŞ EMRİ TOPLAM ÜRETİM
            //decimal totalProduction = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.Active == true).Sum(y => y.Quantity);

            if (processNewActive)
            {
                if (!productionDetails.HasEntries() || shopOrderProduction == null)
                    return;

                //TOPLAM GERÇEKLEŞEN ÜRETİM
                realizeAmount = productionDetails.Sum(y => y.Quantity);

                //İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN ÜRETİM (Manuel eklenen hariç)
                currentAmount = productionDetails.Where(x => x.Active == true && x.ShopOrderProductionID == shopOrderProduction.Id).Sum(y => y.Quantity);

                // İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN HATASIZ ÜRETİM (Manuel eklenen hariç)
                //totalOkProduction = productionDetails.Where(x => x.ProductionStateID == StaticValues.specialCodeOk.Id && x.StartDate != x.EndDate && x.Active == true).Sum(y => y.Quantity);

                //İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN HATALI ÜRETİM
                totalNotOkProduction = productionDetails.Where(x => x.ProductionStateID != StaticValues.specialCodeOk.Id).Sum(y => y.Quantity);
                //TOPLAM ÜRETİM BİLDİRİMİ
                //totalAmountThisProductionWithHandlingUnit = productionDetails.Where(x => x.BoxID != Guid.Empty && x.ShopOrderProductionID == shopOrderProduction.Id).Sum(x => x.HandlingUnitQuantity);

                if (handlingUnits != null)
                    totalHandlingUnitQuantityThisProduction = handlingUnits.Where(x => x.CreatedAt > shopOrderProduction.OrderStartDate).Sum(y => y.Quantity);

                decimal carpan = 1;
                decimal.TryParse(vw_ShopOrderGridModelActive.alan1, out carpan);
                PLCCounter = productionDetails.Where(x => x.ShopOrderProductionID == shopOrderProduction.Id).Sum(y => (y.Quantity - y.ManualInput) / carpan);

            }
            else
            {
                if (!productionDetails.HasEntries() || shopOrderProduction == null)
                    return;

                //TOPLAM GERÇEKLEŞEN ÜRETİM (Manuel eklenen hariç)
                realizeAmount = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.Active == true).Sum(y => y.Quantity);

                //İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN ÜRETİM (Manuel eklenen hariç)
                currentAmount = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.Active == true && x.ShopOrderProductionID == shopOrderProduction.Id).Sum(y => y.Quantity);

                // İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN HATASIZ ÜRETİM (Manuel eklenen hariç)
                //totalOkProduction = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.Active == true && x.ProductionStateID == StaticValues.specialCodeOk.Id && x.StartDate != x.EndDate).Sum(y => y.Quantity);

                //İŞ EMRİ BAŞLADIKTAN SONRA GERÇEKLEŞEN HATALI ÜRETİM
                totalNotOkProduction = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.ProductionStateID != StaticValues.specialCodeOk.Id).Sum(y => y.Quantity);

                //TOPLAM ÜRETİM BİLDİRİMİ
                //totalAmountThisProductionWithHandlingUnit = productionDetails.Where(x => x.BoxID != Guid.Empty && x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.ShopOrderProductionID == shopOrderProduction.Id).Sum(x => x.HandlingUnitQuantity);

                if (handlingUnits != null)
                    totalHandlingUnitQuantityThisProduction = handlingUnits.Where(x => x.ShopOrderID == vw_ShopOrderGridModelActive.Id && x.CreatedAt > shopOrderProduction.OrderStartDate).Sum(y => y.Quantity);

                decimal carpan = 1;
                if (!decimal.TryParse(vw_ShopOrderGridModelActive.alan1, out carpan) || carpan == 0)
                {
                    carpan = 1;
                }
                PLCCounter = productionDetails.Where(x => x.ShopOrderOperationID == vw_ShopOrderGridModelActive.Id && x.Active == true && x.ShopOrderProductionID == shopOrderProduction.Id).Sum(y => (y.Quantity - y.ManualInput) / carpan);
            }

            if (vw_ShopOrderGridModelActive.unitMeas == Units.ad.ToText())
            {
                lblTotalProductionCount.Text = vw_ShopOrderGridModelActive.revisedQtyDue.ToString();
                lblRealizeAmount.Text = Math.Round(realizeAmount, 0).ToString();
                lblCurrentAmount.Text = Math.Round(currentAmount, 0).ToString();
                lblScrapCount.Text = totalNotOkProduction.ToString();
                //lblProductionInfo.Text= totalAmountThisProductionWithHandlingUnit.ToString();
                lblBoxAmount.Text = totalHandlingUnitQuantityThisProduction.ToString();
                lblPLC.Text = Math.Round(PLCCounter, 0).ToString();
            }
            else
            {
                //ÜRETİM BİLDİRİMİ
                decimal productionNotice = 0;
                foreach (var handlingUnit in handlingUnits)
                {
                    if (vw_ShopOrderGridModelActive.Id != handlingUnit.ShopOrderID)
                        continue;
                    if (handlingUnit.ManuelInput > 0)
                        productionNotice += handlingUnit.ManuelInput;
                    else
                        productionNotice += handlingUnit.Quantity;
                }
                lblTotalProductionCount.Text = Math.Round(vw_ShopOrderGridModelActive.revisedQtyDue, 2).ToString();
                lblRealizeAmount.Text = Math.Round(realizeAmount, 2).ToString();
                lblCurrentAmount.Text = Math.Round(currentAmount, 2).ToString();
                lblScrapCount.Text = Math.Round(totalNotOkProduction, 2).ToString();
                //lblProductionInfo.Text = Math.Round(productionNotice, 2).ToString();
                lblBoxAmount.Text = Math.Round(productionNotice, 2).ToString();
                lblPLC.Text = Math.Round(productionNotice, 2).ToString();
            }

            //ToDo : Metod içeiğinde eklemen gerekiyor alttaki satırları test için bu şekilde bırakıldı
            try
            {
                decimal qt = exShopOrderProductionDetails.Sum(x => x.Quantity); ;
                lblRealizeAmount.Text = (Convert.ToDecimal(lblRealizeAmount.Text) + exShopOrderProductionDetails.Sum(x => x.Quantity)).ToString();
            }
            catch
            {
            }
        }

        public void SetProductionPerformanceChart(double target, double production)
        {
            int d = 0;
            if (Text == "Yeni Lineer 3 Fırın-Vakum 1")
                d = 1;
            ccProductionPerformance.Series[0].Points[0].Values[0] = Math.Round(production, 0);
            ccProductionPerformance.Series[0].Points[1].Values[0] = Math.Round(target, 0);
            if (target <= production)
                ccProductionPerformance.Series[0].Points[0].Color = greenC;
            else if (target * 0.85 <= production)
                ccProductionPerformance.Series[0].Points[0].Color = yellowC;
            else
                ccProductionPerformance.Series[0].Points[0].Color = redC;
            ccProductionPerformance.RefreshData();
        }

        public void Label9SetValue()
        {
            lblValue9.Text = (Convert.ToDecimal(lblValue9.Text) + 1).ToString();
        }
        #endregion

        #region SET LABEL OTHER VALUE
        public void SetLabelOpcOtherreadValue(OpcOtherReadModel read)
        {
            if (read.Location == null)
                return;
            for (int i = 1; i < 10; i++)
            {
                if (read.Location.Contains(i.ToString()))
                {
                    foreach (var item in pnlReadValueLabels.Controls)
                    {
                        if (item is LabelControl)
                        {

                            LabelControl label = item as LabelControl;
                            if (label.Name == "lblValue" + i)
                            {
                                if (read.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypeSquareMeters.Id)//METRE KARE SAYACINDA EN GÖSTERMEK İÇİN
                                {
                                    if (vw_ShopOrderGridModels.HasEntries())
                                        label.Text = Math.Round((read.readValue * vw_ShopOrderGridModels.Sum(x => Convert.ToDecimal(x.ProductAlan6))), 2).ToString();
                                }
                                else if (read.SpecialCodeId == StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id)
                                {
                                    if (label.Text == "")
                                        label.Text = read.readValue.ToString();
                                    else
                                    {
                                        label.Text = (Convert.ToDecimal(label.Text) + read.readValue).ToString();
                                    }
                                }
                                else
                                    label.Text = read.readValue.ToString();

                            }
                            if (label.Name == "lblDescription" + i)
                                label.Text = read.Description;
                        }
                    }
                }
            }
        }
        #endregion

        #region EVENT HANDLERS
        private void lblCurrentAmount_TextChanged(object sender, EventArgs e)
        {
            RefreshTargetProduction();
        }

        private void lblRealizeAmount_TextChanged(object sender, EventArgs e)
        {
            var qty = vw_ShopOrderGridModelActive.revisedQtyDue;
            var value = Convert.ToDecimal(lblRealizeAmount.Text);

            lscTotalProduction.MaxValue = (float)qty;
            lscTotalProduction.Value = (float)value;
            lsrbcTotalProduction.AppearanceRangeBar.ContentBrush = blueBO;

            if (value > 0)
            {
                if (value >= (int)qty)
                    lsrbcTotalProduction.AppearanceRangeBar.ContentBrush = redBO;
                else if (value >= (int)((double)qty * 0.9))
                    lsrbcTotalProduction.AppearanceRangeBar.ContentBrush = yellowBO;
            }

            gcTotalProductionAmount.Refresh();
        }

        private void container_VisibleChanged(object sender, EventArgs e)
        {
            if (!container.Visible)
                return;

            // Tag -> enum (boxed) unboxing once
            var selected = (ContainerSelectUserControl)container.Tag;

            // Common helpers
            void AddToContainer(UserControl uc, Size size)
            {
                container.SuspendLayout();
                try
                {
                    container.Controls.Clear();
                    container.BringToFront();
                    container.Left = 0;
                    container.Top = 0;
                    container.Size = size;
                    uc.Dock = DockStyle.Fill;
                    container.Controls.Add(uc);
                }
                finally
                {
                    container.ResumeLayout(performLayout: true);
                }
            }

            // Safe "last" without LINQ allocation
            T GetLast<T>(IList<T> list)
            {
                return (list != null && list.Count > 0) ? list[list.Count - 1] : default(T);
            }

            // If Users is IList<UserModel> this avoids multiple enumerations
            bool HasPrivilegedUser()
            {
                if (Users == null) return false;
                for (int i = 0; i < Users.Count; i++)
                {
                    if (Users[i].Role > 2) return true;
                }
                return false;
            }

            // First user fast-path (only used if list non-empty)
            UserModel FirstUserOrNull()
            {
                return (Users != null && Users.Count > 0) ? Users[0] : null;
            }

            switch (selected)
            {
                case ContainerSelectUserControl.MachineDown:
                    {
                        var uc = new ucMachineDown();
                        AddToContainer(uc, new Size(750, 471));
                        break;
                    }

                case ContainerSelectUserControl.MachineDownDuration:
                    {
                        ToolsMdiManager.frmOperatorActive = this;
                        var lastFault = GetLast(faults);
                        var uc = new ucMachineDownDuration(lastFault);
                        AddToContainer(uc, new Size(521, 369));
                        break;
                    }

                case ContainerSelectUserControl.MachineDownMaintanenceStart:
                    {
                        var lastFault = GetLast(faults);
                        var lastUser = GetLast(faultUserModels);
                        var uc = new ucMachineDownMaintanenceStart(lastFault, lastUser);
                        AddToContainer(uc, new Size(750, 469));
                        break;
                    }

                case ContainerSelectUserControl.MachineDownStop:
                    {
                        var lastFault = GetLast(faults);
                        var uc = new ucMachineDownMaintanenceFinish(lastFault, machineDownFinishUser, machine);
                        AddToContainer(uc, new Size(954, 555));
                        break;
                    }

                case ContainerSelectUserControl.PrMaintenance:
                    {
                        AddToContainer(new ucPrMaintenance(), new Size(954, 555));
                        break;
                    }

                case ContainerSelectUserControl.PrMaintenanceStart:
                    {
                        AddToContainer(new ucPrMaintenanceStart(), new Size(750, 469));
                        break;
                    }

                case ContainerSelectUserControl.PrMaintenanceFinish:
                    {
                        AddToContainer(new ucPrMaintenanceFinish(), new Size(954, 555));
                        break;
                    }

                case ContainerSelectUserControl.InterruptionCause:
                    {
                        AddToContainer(new ucInterruptionCause(), new Size(750, 471));
                        break;
                    }

                case ContainerSelectUserControl.InterruptionCauseDuration:
                    {
                        var uc = new ucInterruptionDuration(interruptionCause);
                        AddToContainer(uc, new Size(521, 369));
                        break;
                    }

                case ContainerSelectUserControl.SetupCheckList:
                    {
                        var lastOrder = vw_ShopOrderGridModels?.OrderByDescending(x => x.opStartDate).Take(1).ToList();
                        var uc = new ucSetupCheckList(lastOrder);
                        AddToContainer(uc, new Size(800, 555));
                        break;
                    }

                case ContainerSelectUserControl.MachineAutoMaintanenceCheckList:
                    {
                        var uc = new ucMachineAutoMaintanenceList(this);
                        AddToContainer(uc, new Size(800, 555));
                        break;
                    }

                case ContainerSelectUserControl.UserLogin:
                    {
                        var ucUserLogin = new ucUserLogin(PersonLoginType.ShopOrderPersonelLogin);
                        AddToContainer(ucUserLogin, new Size(410, 501));
                        break;
                    }

                case ContainerSelectUserControl.UserLogOut:
                    {
                        var ucUserLogin = new ucUserLogin(PersonLoginType.ShopOrderPersonelLogout);
                        AddToContainer(ucUserLogin, new Size(410, 501));
                        break;
                    }

                case ContainerSelectUserControl.BoxBarcode:
                    {
                        if (HasPrivilegedUser())
                        {
                            var user = FirstUserOrNull();
                            var uc = new ucChooseWorkOrder(user);
                            AddToContainer(uc, new Size(810, 555));
                        }
                        else
                        {
                            ToolsMessageBox.Error(this,
                                MessageTextHelper.GetMessageText("000", "929",
                                    "Etiket almak için yeterli yetkiye sahip değilsiniz", "Message"));
                        }
                        break;
                    }

                case ContainerSelectUserControl.ProcessBoxBarcode:
                    {
                        if (HasPrivilegedUser())
                        {
                            var user = FirstUserOrNull();
                            var uc = new ucBoxLabelProcess(user);
                            AddToContainer(uc, new Size(810, 555));
                        }
                        else
                        {
                            ToolsMessageBox.Error(this,
                                MessageTextHelper.GetMessageText("000", "929",
                                    "Etiket almak için yeterli yetkiye sahip değilsiniz", "Message"));
                        }
                        break;
                    }

                case ContainerSelectUserControl.QuestionableProduct:
                    {
                        using (var frm = new FrmUserLogin(true))
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                var uc = new ucQuestionableProduct(frm.userModel);
                                AddToContainer(uc, new Size(750, 471));
                            }
                            else
                            {
                                ToolsMdiManager.frmOperatorActive.container.Visible = false;
                            }
                        }
                        break;
                    }

                case ContainerSelectUserControl.GeneralReadResult:
                    {
                        var uc = new ucGeneralBarcodeReadResult(this.Text, lblCurrentAmount.Text, string.IsNullOrWhiteSpace(lblStatus.Text));
                        AddToContainer(uc, new Size(700, 300));

                        BgWorker = new BackgroundWorker();
                        BgWorker.DoWork += BgWorker_DoWork;
                        BgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
                        if (!BgWorker.IsBusy)
                            BgWorker.RunWorkerAsync();

                        break;
                    }

                case ContainerSelectUserControl.ShiftBook:
                    {
                        using (var frm = new FrmUserLogin(false))
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                var uc = new ucShiftBook(frm.userModel);
                                AddToContainer(uc, new Size(702, 401));
                            }
                            else
                            {
                                ToolsMdiManager.frmOperatorActive.container.Visible = false;
                            }
                        }
                        break;
                    }

                //case ContainerSelectUserControl.YellowCard:
                //    {
                //        using (var frm = new FrmUserLogin(false))
                //        {
                //            if (frm.ShowDialog() == DialogResult.OK)
                //            {
                //                var uc = new ucYellowCard(frm.userModel);
                //                AddToContainer(uc, new Size(702, 401));
                //            }
                //            else
                //            {
                //                ToolsMdiManager.frmOperatorActive.container.Visible = false;
                //            }
                //        }
                //        break;
                //    }

                default:
                    container.Controls.Clear();
                    container.Visible = false;
                    break;
            }
        }

        private void txtBarcode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (/*container.Visible || */txtBarcode.Text == string.Empty)
                return;
            if (Keys.Enter == e.KeyCode)
            {
                lblStatus.Text = "";

                try
                {
                    if (txtBarcode.Text.Length < 7) //WorCenter Kodu tab Page geçiişi
                    {
                        foreach (var frm in ToolsMdiManager.frmOperators)
                        {
                            if (frm.resource.resourceId == txtBarcode.Text)
                            {
                                ToolsMdiManager.ActivatedForm(frm);
                                txtBarcode.SelectAll();
                                break;
                            }
                        }
                        return;
                    }

                    if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id)
                    {
                        ButtonAndReadBarcodeControl(txtBarcode.Text);
                    }
                    //else if (panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeSupplierPark.Id)
                    //{
                    //    SupplierParkExtension(txtBarcode.Text);
                    //}
                    else
                    {
                        ProcessReadBarcodeHelper.ReadProcessBarcodeNew(txtBarcode.Text);
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = ex.Message;
                }

                txtBarcode.Text = "";
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {

            var text = sender as TextEdit;

            if (text.Parent.Parent.Tag.ToString() != resource.resourceId)
            {
                txtBarcode.Focus();

                txtBarcode.Text = String.Empty;
            }
            txtBarcode.SelectAll();
        }

        private void xtcDetails_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtcDetails.TabPages.IndexOf(e.Page) == 0)//iş emirleri listesi
            {

            }
            else if (xtcDetails.TabPages.IndexOf(e.Page) == 1)//Duruş Listesi
            {
                RefreshInterruptionCauseGrid();
            }
            else if (xtcDetails.TabPages.IndexOf(e.Page) == 2)//Arıza Listesi
            {

            }
            else//SÜRELER
            {

            }
        }

        private void gwWorkShopOrder_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
                return;
            vw_ShopOrderGridModelActive = (vw_ShopOrderGridModel)gwWorkShopOrder.GetRow(e.RowHandle);
            SetLabelsTextValue();
        }

        private void gwWorkShopOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.ControllerRow < 0)
                return;
            vw_ShopOrderGridModelActive = (vw_ShopOrderGridModel)gwWorkShopOrder.GetRow(e.ControllerRow);
            SetLabelsTextValue();
        }

        private void gwWorkShopOrder_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;
            vw_ShopOrderGridModelActive = (vw_ShopOrderGridModel)gwWorkShopOrder.GetRow(e.FocusedRowHandle);

            SetLabelsTextValue();
        }

        private void gwWorkShopOrder_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "gcUnboundColumn" && e.IsGetData)
            {
                DateTime date = (DateTime)gwWorkShopOrder.GetListSourceRowCellValue(e.ListSourceRowIndex, "opStartDate");
                e.Value = Math.Round((DateTime.Now - date).TotalMinutes, 1);
            }
            else if (e.Column.FieldName == "qtyComplate" && e.IsGetData)
            {
                Guid id = (Guid)gwWorkShopOrder.GetListSourceRowCellValue(e.ListSourceRowIndex, "Id");
                e.Value = productionDetails.Where(x => x.ShopOrderOperationID == id).Sum(x => x.Quantity);
            }
        }

        private void gvInterruption_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "gcUnboundColumn" && e.IsGetData)
            {
                var finishDate = gvInterruption.GetListSourceRowCellValue(e.ListSourceRowIndex, "InterruptionFinishDate");
                DateTime startDate = (DateTime)gvInterruption.GetListSourceRowCellValue(e.ListSourceRowIndex, "InterruptionStartDate");

                if (finishDate == null)
                {
                    DateTime date = (DateTime)gvInterruption.GetListSourceRowCellValue(e.ListSourceRowIndex, "InterruptionStartDate");
                    e.Value = Math.Round((DateTime.Now - date).TotalMinutes, 1);
                }
                else
                {
                    e.Value = Math.Round(((DateTime)finishDate - startDate).TotalMinutes, 1);
                }
            }
        }

        private void gvPerson_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "gcUnboundPersonPassingTime" && e.IsGetData)
                {

                    Object date = gvPerson.GetListSourceRowCellValue(e.ListSourceRowIndex, "StartDate");
                    if (date != null)
                    {

                        e.Value = Convert.ToInt32(Math.Round((DateTime.Now - Convert.ToDateTime(date)).TotalMinutes, 0));
                    }
                }
            }
            catch { }
        }

        private void tmrWorkShopOrder_Tick(object sender, EventArgs e)
        {
            gwWorkShopOrder.RefreshData();

            //Shift
            Shift shift = StaticValues.shift;

            if (interruptionCause != null)
            {
                ts = DateTime.Now - interruptionCause.InterruptionStartDate;
                lblWorkCenterStartTime.Text = ts.ToString(@"dd\.hh\:mm\:ss");
                gvInterruption.RefreshData();
            }
            else
            {
                if (shopOrderProduction != null && shopOrderStatus != ShopOrderStatus.Start)
                {
                    ts = DateTime.Now - shopOrderProduction.OrderStartDate;
                    lblWorkCenterStartTime.Text = ts.ToString(@"dd\.hh\:mm\:ss");
                }
                else
                    lblWorkCenterStartTime.Text = "";
            }

            if (Users != null && Users.Count() > 0)
            {
                gvPerson.RefreshData();
            }
            SetLabelsTextValue();
        }

        /// <summary>
        /// Sleep for 2 seconds
        /// </summary>
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2 * 1000);
        }

        /// <summary>
        /// After the background worker is done hide container
        /// </summary>
        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            container.Visible = false;
        }
        #endregion

        #region HELPER METHOTS
        private void ButtonAndReadBarcodeControl(string Serial)
        {
            factorCounter++;
            var shopOrderOperationFactor = vw_ShopOrderGridModels.Sum(x => Convert.ToInt16(x.alan1));

            try
            {
                ButtonAndReadBarcodeHelper.ButtonAndReadBarcodeControl(Serial, factorCounter, shopOrderOperationFactor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (shopOrderOperationFactor == factorCounter)
                    factorCounter = 0;
            }
        }

        //private void SupplierParkExtension(string barcode)
        //{
        //    try
        //    {
        //        if (shopOrderProduction == null)
        //            return;
        //        if (supplierPartProductionDetailSelected == null)
        //            supplierPartProductionDetailSelected = new ShopOrderProductionDetail();

        //        DataRow dataRow;
        //        var pd = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetailByBarcodeOrSerial(barcode);
        //        if (pd == null)
        //        {
        //            dataRow = ExMasHelper.GetProductDetails(barcode);
        //            if (dataRow == null)
        //                throw new Exception(MessageTextHelper.Message610);
        //            shopOrderProductionDetails.First().Barcode = barcode;
        //            supplierPartProductionDetailSelected = shopOrderProductionDetails.First();
        //        }
        //        else
        //        {
        //            var product = ProductManager.Current.GetProductById(pd.ProductID);
        //            var resultDataMatterialAlloc = ShopMaterialAllocManager.Current.GetlistByOrderID(vw_ShopOrderGridModels.First().OrderID);
        //            if (resultDataMatterialAlloc.NullAndCountControl())
        //            {

        //                if (resultDataMatterialAlloc.Any(x => x.description == product.Description))
        //                {

        //                    supplierPartProductionDetailSelected.Barcode = barcode;
        //                    supplierPartProductionDetailSelected = shopOrderProductionDetails.First();
        //                }
        //            }
        //            else
        //                throw new Exception(MessageTextHelper.Message611);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Text = ex.Message;
        //    }
        //}

        public void RefreshInterruptionCauseGrid()
        {
            RemoveOldInterruptions();
            try
            {
                rtsInterruptionGrid = new RealTimeSource()
                {
                    DataSource = interruptionGridModel.interruptionCauseGridModels.Where(x => x.ResourceID == resource.Id).ToList()
                };
                gcInterruption.DataSource = rtsInterruptionGrid;
            }
            catch { }
        }

        public void RemoveOldInterruptions()
        {
            try
            {
                var toRemove = new ObservableCollection<vw_InterruptionCauseGridModel>();
                foreach (var interruption in interruptionGridModel.interruptionCauseGridModels)
                {
                    if (interruption.ShiftID != StaticValues.shift.Id && interruption.ShopOrderProductionID != shopOrderProduction.Id)
                        toRemove.Add(interruption);
                }
                foreach (var interruption in toRemove)
                    interruptionGridModel.interruptionCauseGridModels.Remove(interruption);
            }
            catch { }
        }

        public void UpdateQuantities(Guid ShopOrderId)
        {
            var selectedShopOrder = vw_ShopOrderGridModels.First(x => x.Id == ShopOrderId);
            gwWorkShopOrder.SelectRow(gwWorkShopOrder.LocateByValue("Id", selectedShopOrder.Id));
        }

        public void RefreshFaultGridModel()
        {
            gcFaults.RefreshDataSource();
        }

        public void FocusAndSelectTxtBarcode()
        {
            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }
        #endregion
    }
}