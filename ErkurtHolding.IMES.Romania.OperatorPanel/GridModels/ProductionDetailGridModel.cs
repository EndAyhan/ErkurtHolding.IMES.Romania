using System;
using System.ComponentModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.GridModels
{
    /// <summary>
    /// View model for production details shown in grids.
    /// Provides localized display names via <see cref="LocalizedDisplayNameAttribute"/>.
    /// </summary>
    public class ProductionDetailGridModel : INotifyPropertyChanged
    {
        private Guid _Id;
        private Guid _ShopOrderOperationID;
        private Guid _ShopOrderProductionID;
        private Guid _ProductID;
        private string _PartHandlingBoxBarcode;
        private string _OrderNo;
        private string _OperationNo;
        private string _Barcode;
        private string _Serial;
        private DateTime _CreateAt;
        private decimal _Quantity;
        private decimal _ManualInput;
        private string _ResourceName;

        [Browsable(false)]
        public Guid Id { get { return _Id; } set { _Id = value; NotifyPropertyChanged(nameof(Id)); } }

        [Browsable(false)]
        public Guid ShopOrderOperationID { get { return _ShopOrderOperationID; } set { _ShopOrderOperationID = value; NotifyPropertyChanged(nameof(ShopOrderOperationID)); } }

        [Browsable(false)]
        public Guid ShopOrderProductionID { get { return _ShopOrderProductionID; } set { _ShopOrderProductionID = value; NotifyPropertyChanged(nameof(ShopOrderProductionID)); } }

        [Browsable(false)]
        public Guid ProductID { get { return _ProductID; } set { _ProductID = value; NotifyPropertyChanged(nameof(ProductID)); } }

        [LocalizedDisplayName("DN", "100", "İş Emri Numarası", "DisplayName")]
        public string OrderNo { get { return _OrderNo; } set { _OrderNo = value; NotifyPropertyChanged(nameof(OrderNo)); } }

        [LocalizedDisplayName("DN", "110", "Ürün Adı", "DisplayName")]
        public string ProductName { get; set; }

        [LocalizedDisplayName("DN", "111", "Ürün Kodu", "DisplayName")]
        public string PartNo { get; set; }

        [LocalizedDisplayName("DN", "101", "Operasyon", "DisplayName")]
        public string OperationNo { get { return _OperationNo; } set { _OperationNo = value; NotifyPropertyChanged(nameof(OperationNo)); } }

        [LocalizedDisplayName("DN", "102", "Barkod", "DisplayName")]
        public string Barcode { get { return _Barcode; } set { _Barcode = value; NotifyPropertyChanged(nameof(Barcode)); } }

        [LocalizedDisplayName("DN", "103", "Lot No", "DisplayName")]
        public string Serial { get { return _Serial; } set { _Serial = value; NotifyPropertyChanged(nameof(Serial)); } }

        [LocalizedDisplayName("DN", "112", "Miktar", "DisplayName")]
        public decimal Quantity { get { return _Quantity; } set { _Quantity = value; NotifyPropertyChanged(nameof(Quantity)); } }

        [LocalizedDisplayName("DN", "105", "USK", "DisplayName")]
        public decimal ManualInput { get { return _ManualInput; } set { _ManualInput = value; NotifyPropertyChanged(nameof(ManualInput)); } }

        [LocalizedDisplayName("DN", "106", "Tarih", "DisplayName")]
        public DateTime CreateAt { get { return _CreateAt; } set { _CreateAt = value; NotifyPropertyChanged(nameof(CreateAt)); } }

        [LocalizedDisplayName("DN", "113", "Kaynak", "DisplayName")]
        public string ResourceName { get { return _ResourceName; } set { _ResourceName = value; NotifyPropertyChanged(nameof(ResourceName)); } }

        [LocalizedDisplayName("DN", "114", "Koli Barkodu", "DisplayName")]
        public string PartHandlingBoxBarcode { get { return _PartHandlingBoxBarcode; } set { _PartHandlingBoxBarcode = value; NotifyPropertyChanged(nameof(PartHandlingBoxBarcode)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
