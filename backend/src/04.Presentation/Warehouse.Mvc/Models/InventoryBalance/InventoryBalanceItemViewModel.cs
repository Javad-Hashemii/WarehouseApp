namespace Warehouse.Mvc.Models.InventoryBalance
{
    public class InventoryBalanceItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Quantity { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
