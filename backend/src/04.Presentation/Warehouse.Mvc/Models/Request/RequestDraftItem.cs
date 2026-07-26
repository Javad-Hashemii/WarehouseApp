namespace Warehouse.Mvc.Models.Request
{
    public class RequestDraftItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal QuantityRequested { get; set; }
    }
}
