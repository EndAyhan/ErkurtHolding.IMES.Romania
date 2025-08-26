using System;
using System.ComponentModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.GridModels
{
    /// <summary>
    /// View model for handling unit rows (box labels).
    /// Provides localized display names via <see cref="LocalizedDisplayNameAttribute"/>.
    /// </summary>
    public class HandlingUnitGridModel : INotifyPropertyChanged
    {
        private Guid _Id;
        private Guid _ShopOrderOperationID;
        private Guid _ShopOrderProductionID;
        private Guid _ProductID;
        private string _OrderNo;
        private string _OperationNo;
        private string _Barcode;
        private string _Serial;
        private double _Quantity;
        private DateTime _CreateAt;
        private string _Description;
        private string _unitMeas;

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

        [LocalizedDisplayName("DN", "101", "Operasyon", "DisplayName")]
        public string OperationNo { get { return _OperationNo; } set { _OperationNo = value; NotifyPropertyChanged(nameof(OperationNo)); } }

        [LocalizedDisplayName("DN", "102", "Barkod", "DisplayName")]
        public string Barcode { get { return _Barcode; } set { _Barcode = value; NotifyPropertyChanged(nameof(Barcode)); } }

        [LocalizedDisplayName("DN", "103", "Lot No", "DisplayName")]
        public string Serial { get { return _Serial; } set { _Serial = value; NotifyPropertyChanged(nameof(Serial)); } }

        [LocalizedDisplayName("DN", "104", "Adet", "DisplayName")]
        public double Quantity { get { return _Quantity; } set { _Quantity = value; NotifyPropertyChanged(nameof(Quantity)); } }

        [LocalizedDisplayName("DN", "106", "Tarih", "DisplayName")]
        public DateTime CreateAt { get { return _CreateAt; } set { _CreateAt = value; NotifyPropertyChanged(nameof(CreateAt)); } }

        [LocalizedDisplayName("DN", "108", "Açıklama", "DisplayName")]
        public string Description { get { return _Description; } set { _Description = value; NotifyPropertyChanged(nameof(Description)); } }

        [LocalizedDisplayName("DN", "109", "Birim", "DisplayName")]
        public string unitMeas { get { return _unitMeas; } set { _unitMeas = value; NotifyPropertyChanged(nameof(unitMeas)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
