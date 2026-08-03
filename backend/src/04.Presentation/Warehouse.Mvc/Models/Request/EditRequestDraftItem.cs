namespace Warehouse.Mvc.Models.Request
{
    public class EditRequestDraftItem
    {
        public int? RequestItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal QuantityRequested { get; set; }  
    }
}
