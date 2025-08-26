using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.ComponentModel;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.GridModels
{
    /// <summary>
    /// Read-only row model for "Şüpheli Ürün" (Questionable Product) listings.
    /// Values are projected from domain entities and exposed with localized display names
    /// for use in DevExpress grid columns.
    /// </summary>
    public class QuestionableProductGridModel
    {
        // ---------- Visible, localized columns ----------
        [LocalizedDisplayName("DN", "111", "Ürün Kodu", "DisplayName")]
        public string PartNo => Product?.PartNo ?? string.Empty;

        [LocalizedDisplayName("DN", "100", "İş Emri Numarası", "DisplayName")]
        public string OrderNo => ShopOrderGridModel?.orderNo ?? string.Empty;

        [LocalizedDisplayName("DN", "101", "Operasyon", "DisplayName")]
        public double OperationNo => ShopOrderGridModel?.operationNo ?? 0;

        [LocalizedDisplayName("DN", "106", "Tarih", "DisplayName")]
        public DateTime StartDate => ShopOrderProductionDetail?.StartDate ?? DateTime.MinValue;

        [LocalizedDisplayName("DN", "102", "Barkod", "DisplayName")]
        public string Barcode => ShopOrderProductionDetail?.Barcode ?? string.Empty;

        [LocalizedDisplayName("DN", "103", "Lot No", "DisplayName")]
        public string Serial => (ShopOrderProductionDetail?.serial.ToString()) ?? string.Empty;

        [LocalizedDisplayName("DN", "112", "Miktar", "DisplayName")]
        public decimal Quantity => ShopOrderProductionDetail?.Quantity ?? 0m;

        [LocalizedDisplayName("DN", "109", "Birim", "DisplayName")]
        public string Unit => ShopOrderProductionDetail?.Unit ?? string.Empty;

        // ---------- Technical / hidden columns ----------
        [Browsable(false)]
        public Guid Id => ShopOrderProductionDetail?.Id ?? Guid.Empty;

        [Browsable(false)]
        public Guid ProductID => ShopOrderProductionDetail?.ProductID ?? Guid.Empty;

        [Browsable(false)]
        public Guid ShopOrderID => ShopOrderProductionDetail?.ShopOrderOperationID ?? Guid.Empty;

        // ---------- Source objects (hidden) ----------
        [Browsable(false)]
        public ShopOrderProductionDetail ShopOrderProductionDetail { get; set; }

        [Browsable(false)]
        public Product Product { get; set; }

        [Browsable(false)]
        public ShopOrderProduction ShopOrderProduction { get; set; }

        [Browsable(false)]
        public vw_ShopOrderGridModel ShopOrderGridModel { get; set; }

        [Browsable(false)]
        public Machine Machine { get; set; }

        [Browsable(false)]
        public Machine Resource { get; set; }
    }
}
