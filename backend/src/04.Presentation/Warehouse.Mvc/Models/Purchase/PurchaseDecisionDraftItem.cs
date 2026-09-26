namespace Warehouse.Mvc.Models.Purchase
{
    public class PurchaseDecisionDraftItem
    {
        public int? RequestItemId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public decimal QuantityRequested { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityShortage { get; set; }

        public decimal QuantityToPurchase { get; set; }
    }
}
