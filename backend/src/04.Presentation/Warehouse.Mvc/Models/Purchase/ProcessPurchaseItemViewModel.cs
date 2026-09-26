namespace Warehouse.Mvc.Models.Purchase
{
    public class ProcessPurchaseItemViewModel
    {
        public int PurchaseRequestItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public decimal QuantityApproved { get; set; }

        public decimal QuantityPurchased { get; set; }
    }
}
