namespace Warehouse.Mvc.Models.Purchase
{
    public class PurchaseDecisionDraft
    {
        public int RequestId { get; set; }
        public List<PurchaseDecisionDraftItem> Items { get; set; } = new();
    }
}
