namespace Warehouse.Mvc.Models.Request
{
    public class CreateRequestItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Unit { get; set; }
        public decimal Quantity { get; set; }
    }
}
