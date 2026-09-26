namespace Warehouse.Mvc.Models.Purchase
{
    public class ConfirmPurchaseReceiptItemViewModel
    {
        public int PurchaseRequestItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public decimal QuantityPurchased { get; set; }

        public decimal QuantityReceived { get; set; }

        public decimal QuantityReceivedNow { get; set; }
    }
}
