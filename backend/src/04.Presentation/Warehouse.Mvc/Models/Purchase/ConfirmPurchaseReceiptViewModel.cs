namespace Warehouse.Mvc.Models.Purchase
{
    public class ConfirmPurchaseReceiptViewModel
    {
        public int PurchaseRequestId { get; set; }

        public string PurchaseRequestNumber { get; set; } = null!;

        public string WorkshopName { get; set; } = null!;

        public List<ConfirmPurchaseReceiptItemViewModel> Items { get; set; } = new();
    }
}
