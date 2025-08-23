using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel
{
    public static class StaticValues
    {
        public static string panelName { get => ConfigurationManager.AppSettings["PanelName"]; }
        public static string HideOEEPanel { get => ConfigurationManager.AppSettings["HideOEEPanel"] ?? "FALSE"; }
        public static string WorkCenterType { get => ConfigurationManager.AppSettings["WorkCenterType"] ?? ""; }
        public static string QrCodeGenerator { get => ConfigurationManager.AppSettings["QrCodeGenerator"] ?? "FALSE"; }
        public static string scrapReasonsUnclearProductRejectMessage { get => ConfigurationManager.AppSettings["OperatorScrapReasonsUnclearProduct"] ?? "UNCLEAR"; }
        public static string scrapReasonsReworkProductRejectMessage { get => ConfigurationManager.AppSettings["OperatorScrapReasonsReworkProduct"] ?? "REWORK"; }
        public static string ScrapPrinterName { get => (ConfigurationManager.AppSettings["ScrapPrinterName"]); }
        public static string inkjetPrintQueue { get => (ConfigurationManager.AppSettings["inkjetPrintQueue"]); }
        public static string ScrapProductDesignPath { get => (ConfigurationManager.AppSettings["ScrapProductDesignPath"]); }
        public static string SubcontractorWorkcenterId { get => ConfigurationManager.AppSettings["subcontractor_workcenter_id"]; }
        public static string SubcontractorResourceId { get => ConfigurationManager.AppSettings["subcontractor_resource_id"]; }

        private static readonly JsonText _t = new JsonText();
        /// <summary>
        /// Global access to the localization dictionary.
        /// </summary>
        public static JsonText T => _t;

        public static bool Restart { get; set; } = true;

        private static List<PrinterMachine> _printerMachines;
        public static List<PrinterMachine> printerMachines
        {
            get
            {
                if (_printerMachines == null)
                    _printerMachines = new List<PrinterMachine>();
                return _printerMachines;
            }
            set { _printerMachines = value; }
        }

        private static OpInterruptionCause _opInterruptionCauseBekleme;
        public static OpInterruptionCause opInterruptionCauseBekleme
        {
            get
            {
                if (_opInterruptionCauseBekleme == null)
                    _opInterruptionCauseBekleme = OpInterruptionCauseManager.Current.GetWaitingDuration(branch.Id, "BEKLEME");
                return _opInterruptionCauseBekleme;
            }
        }

        #region SpecialCodes
        private static SpecialCode _specialCodePrintLogTypeScrap;
        public static SpecialCode specialCodePrintLogTypeScrap
        {
            get
            {
                if (_specialCodePrintLogTypeScrap == null)
                {
                    _specialCodePrintLogTypeScrap = SpecialCodeManager.Current.GetSpecialCodeByName(PrintLogType.Scrap.ToText(), (int)SpecialCodeType.Type);
                }
                return _specialCodePrintLogTypeScrap;
            }
        }

        private static SpecialCode _specialCodePrintLogTypeProductiongDetail;
        public static SpecialCode specialCodePrintLogTypeProductiongDetail
        {
            get
            {
                if (_specialCodePrintLogTypeProductiongDetail == null)
                {
                    _specialCodePrintLogTypeProductiongDetail = SpecialCodeManager.Current.GetSpecialCodeByName(PrintLogType.ProductiongDetail.ToText(), (int)SpecialCodeType.Type);
                }
                return _specialCodePrintLogTypeProductiongDetail;
            }
        }

        private static SpecialCode _specialCodePrintLogTypeHandlingUnit;
        public static SpecialCode specialCodePrintLogTypeHandlingUnit
        {
            get
            {
                if (_specialCodePrintLogTypeHandlingUnit == null)
                {
                    _specialCodePrintLogTypeHandlingUnit = SpecialCodeManager.Current.GetSpecialCodeByName(PrintLogType.HandlingUnit.ToText(), (int)SpecialCodeType.Type);
                }
                return _specialCodePrintLogTypeHandlingUnit;
            }
        }

        private static SpecialCode _specialCodePrintLogTypeProsesHandlingUnit;
        public static SpecialCode specialCodePrintLogTypeProsesHandlingUnit
        {
            get
            {
                if (_specialCodePrintLogTypeProsesHandlingUnit == null)
                {
                    _specialCodePrintLogTypeProsesHandlingUnit = SpecialCodeManager.Current.GetSpecialCodeByName(PrintLogType.ProsesHandlingUnit.ToText(), (int)SpecialCodeType.Type);
                }
                return _specialCodePrintLogTypeProsesHandlingUnit;
            }
        }

        private static SpecialCode _specialCodeCounterReadTypeButtonAndReadBarcode;
        public static SpecialCode specialCodeCounterReadTypeButtonAndReadBarcode
        {
            get
            {
                if (_specialCodeCounterReadTypeButtonAndReadBarcode == null)
                {
                    _specialCodeCounterReadTypeButtonAndReadBarcode = SpecialCodeManager.Current.GetSpecialCodeByName(CounterReadType.BUTTONANDREADBARCODE.ToText(), 23);
                }
                return _specialCodeCounterReadTypeButtonAndReadBarcode;
            }
        }

        private static SpecialCode _specialCodeCounterReadTypeSupplierPark;
        public static SpecialCode specialCodeCounterReadTypeSupplierPark
        {
            get
            {
                if (_specialCodeCounterReadTypeSupplierPark == null)
                {
                    _specialCodeCounterReadTypeSupplierPark = SpecialCodeManager.Current.GetSpecialCodeByName(CounterReadType.SUPPLIERPARK.ToText(), 23);
                }
                return _specialCodeCounterReadTypeSupplierPark;
            }
        }

        private static SpecialCode _specialCodeCounterReadTypePlcBarcode;
        public static SpecialCode specialCodeCounterReadTypePlcBarcode
        {
            get
            {
                if (_specialCodeCounterReadTypePlcBarcode == null)
                {
                    _specialCodeCounterReadTypePlcBarcode = SpecialCodeManager.Current.GetSpecialCodeByName(CounterReadType.PLCBARCODE.ToText(), 23);
                }
                return _specialCodeCounterReadTypePlcBarcode;
            }
        }

        private static SpecialCode _specialCodeProductTypeProcess;
        public static SpecialCode specialCodeProductTypeProcess
        {
            get
            {
                if (_specialCodeProductTypeProcess == null)
                {
                    _specialCodeProductTypeProcess = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionLabelType.Process.ToText(), (int)SpecialCodeType.LabelType);
                }
                return _specialCodeProductTypeProcess;
            }

        }

        private static SpecialCode _specialCodeProductTypeProduct;
        public static SpecialCode specialCodeProductTypeProduct
        {
            get
            {
                if (_specialCodeProductTypeProduct == null)
                {
                    _specialCodeProductTypeProduct = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionLabelType.Product.ToText(), (int)SpecialCodeType.LabelType);
                }
                return _specialCodeProductTypeProduct;
            }

        }

        private static SpecialCode _specialCodeProductTypeBox;
        public static SpecialCode specialCodeProductTypeBox
        {
            get
            {
                if (_specialCodeProductTypeBox == null)
                {
                    _specialCodeProductTypeBox = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionLabelType.Box.ToText(), (int)SpecialCodeType.LabelType);
                }
                return _specialCodeProductTypeBox;
            }
        }

        private static SpecialCode _specialCodeProductTypeInkjetProcess;
        public static SpecialCode specialCodeProductTypeInkjetProcess
        {
            get
            {
                if (_specialCodeProductTypeInkjetProcess == null)
                {
                    _specialCodeProductTypeInkjetProcess = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionLabelType.InkjetProcess.ToText(), (int)SpecialCodeType.LabelType);
                }
                return _specialCodeProductTypeInkjetProcess;
            }

        }

        private static SpecialCode _specialCodeCounterReadTypePLC;
        public static SpecialCode specialCodeCounterReadTypePLC
        {
            get
            {
                if (_specialCodeCounterReadTypePLC == null)
                {
                    _specialCodeCounterReadTypePLC = SpecialCodeManager.Current.GetSpecialCodeByName(CounterReadType.PLC.ToText(), 23);
                }
                return _specialCodeCounterReadTypePLC;
            }
        }

        private static SpecialCode _specialCodeCounterReadTypeBarocodePlc;
        public static SpecialCode specialCodeCounterReadTypeBarocodePlc
        {
            get
            {
                if (_specialCodeCounterReadTypeBarocodePlc == null)
                {
                    _specialCodeCounterReadTypeBarocodePlc = SpecialCodeManager.Current.GetSpecialCodeByName(CounterReadType.BARCODEPLC.ToText(), 23);
                }
                return _specialCodeCounterReadTypeBarocodePlc;
            }

        }

        private static SpecialCode _specialCodeMachineOPCDataTypePlcRunModeParameter;
        public static SpecialCode specialCodeMachineOPCDataTypePlcRunModeParameter
        {
            get
            {
                if (_specialCodeMachineOPCDataTypePlcRunModeParameter == null)
                {
                    _specialCodeMachineOPCDataTypePlcRunModeParameter = SpecialCodeManager.Current.GetSpecialCodeByName(MachineOPCDataType.PlcRunModeParameter.ToText(), (byte)SpecialCodeType.MachineOPCDataType);
                }
                return _specialCodeMachineOPCDataTypePlcRunModeParameter;
            }
        }

        private static SpecialCode _specialCodeMachineOPCDataTypePokaYoke;
        public static SpecialCode specialCodeMachineOPCDataTypePokayoke
        {
            get
            {
                if (_specialCodeMachineOPCDataTypePokaYoke == null)
                {
                    _specialCodeMachineOPCDataTypePokaYoke = SpecialCodeManager.Current.GetSpecialCodeByName(MachineOPCDataType.PokaYoke.ToText(), (byte)SpecialCodeType.MachineOPCDataType);
                }
                return _specialCodeMachineOPCDataTypePokaYoke;
            }
        }

        private static SpecialCode _specialCodeMachineOPCDataTypeProgramNodeId;
        public static SpecialCode specialCodeMachineOPCDataTypeProgramNodeId
        {
            get
            {
                if (_specialCodeMachineOPCDataTypeProgramNodeId == null)
                {
                    _specialCodeMachineOPCDataTypeProgramNodeId = SpecialCodeManager.Current.GetSpecialCodeByName(MachineOPCDataType.MachineProgramNodeId.ToText(), (byte)SpecialCodeType.MachineOPCDataType);
                }
                return _specialCodeMachineOPCDataTypeProgramNodeId;
            }
        }
        #endregion

        #region Shift
        private static Shift _shift;
        public static Shift shift
        {
            get
            {
                _shift = ShiftManager.Current.GetShift(panel.BranchId);
                return _shift;
            }
        }

        #endregion

        #region Panel
        private static Panel _panel; public static Company company { get; set; }
        public static Branch branch { get; set; }
        public static string ifsCompany { get; set; }
        public static string ifsContract { get; set; }
        public static Panel panel
        {
            get
            {
                if (_panel == null)
                {
                    _panel = PanelManager.Current.GetPanel(panelName);
                    if (_panel == null)
                        return null;
                    company = CompanyManager.Current.GetCompany(_panel.CompanyId);
                    branch = BranchManager.Current.GetBranch(_panel.BranchId);
                    ifsCompany = company.ERPConnectionCode;
                    ifsContract = branch.ERPConnectionCode;
                }
                return _panel;
            }
        }
        #endregion

        #region ProductionStateId
        private static SpecialCode SpecialCodeScrap;
        private static SpecialCode SpecialCodeOk;
        private static SpecialCode SpecialCodeNotOk;
        private static SpecialCode SpecialCodeQualityOk;
        /// <summary>
        /// ŞÜPHELİ
        /// </summary>
        public static SpecialCode specialCodeScrap
        {
            get
            {
                if (SpecialCodeScrap == null)
                {
                    SpecialCodeScrap = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionStateId.Scrap.ToText(), 19);
                }
                return SpecialCodeScrap;
            }
        }

        /// <summary>
        /// OK
        /// </summary>
        public static SpecialCode specialCodeOk
        {
            get
            {
                if (SpecialCodeOk == null)
                {
                    SpecialCodeOk = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionStateId.OK.ToText(), 19);
                }
                return SpecialCodeOk;
            }
        }

        /// <summary>
        /// HURDA
        /// </summary>
        public static SpecialCode specialCodeNotOk
        {
            get
            {
                if (SpecialCodeNotOk == null)
                {
                    SpecialCodeNotOk = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionStateId.NotOk.ToText(), 19);
                }
                return SpecialCodeNotOk;
            }
        }

        /// <summary>
        /// HURDA
        /// </summary>
        public static SpecialCode specialCodeQualityOk
        {
            get
            {
                if (SpecialCodeQualityOk == null)
                {
                    SpecialCodeQualityOk = SpecialCodeManager.Current.GetSpecialCodeByName(ProductionStateId.QUALITYOK.ToText(), 19);
                }
                return SpecialCodeQualityOk;
            }
        }
        #endregion

        #region OPC CLIENT SETTINGS

        private static OPCClientHelper _opcClient;
        public static OPCClientHelper opcClient
        {
            get
            {
                if (_opcClient == null)
                    _opcClient = new OPCClientHelper(opcSetting);
                return _opcClient;
            }
            set
            {
                _opcClient = value;
            }
        }

        private static OPCSetting _opcSetting;

        public static OPCSetting opcSetting
        {
            get
            {
                if (_opcSetting == null)
                {
                    _opcSetting = OPCSettingManager.Current.GetListByOPCSettingById(panel.OpcSettingID);
                }
                return _opcSetting;
            }
        }
        #endregion

        #region Units
        private static SpecialCode SpecialCodeUnitSquareMeter;
        public static SpecialCode specialCodeUnitSquareMeter
        {
            get
            {
                if (SpecialCodeUnitSquareMeter == null)
                {
                    SpecialCodeUnitSquareMeter = SpecialCodeManager.Current.GetSpecialCodeByName(SpecialCodeCounterUnit.squareMeter.ToText(), 10);
                }
                return SpecialCodeUnitSquareMeter;
            }
        }
        #endregion

        #region KAFKA SERVICE
        public static string[] args { get; set; }
        private static KafkaService _kafkaService;
        public static KafkaService kafkaService
        {
            get
            {
                //if (_kafkaService == null)
                //{
                //    _kafkaService = new KafkaService(args);
                //}
                //return _kafkaService; 
                return new KafkaService(args);
            }

        }
        #endregion

        #region Company
        private static List<Company> _companies;
        public static List<Company> companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = CompanyManager.Current.Select().ListData.ToList();

                }
                return _companies;
            }
            set
            {
                _companies = value;
            }
        }
        #endregion

        #region Branch
        private static List<Branch> _branches;
        public static List<Branch> branches
        {
            get
            {
                if (_branches == null)
                {
                    _branches = BranchManager.Current.Select().ListData.ToList();

                }
                return _branches;
            }
            set
            {
                _branches = value;
            }

        }
        #endregion

        #region Department
        private static List<Department> _depertmans;
        public static List<Department> depertmans
        {
            get
            {
                if (_depertmans == null)
                {
                    _depertmans = DepartmentManager.Current.Select().ListData.ToList();

                }
                return _depertmans;
            }
            set
            {
                _depertmans = value;
            }
        }
        #endregion

        #region Group
        private static List<Group> _groups;
        public static List<Group> groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = GroupManager.Current.Select().ListData.ToList<Group>();

                }
                return _groups;
            }
            set
            {
                _groups = value;
            }
        }
        #endregion

        #region Production Department
        private static List<ProductionDepartment> _productionDepertmants;
        public static List<ProductionDepartment> productionDepartments
        {
            get
            {
                if (_productionDepertmants == null)
                {
                    _productionDepertmants = ProductionDepartmentManager.Current.Select().ListData.ToList();

                }
                return _productionDepertmants;
            }
        }
        #endregion
    }
}