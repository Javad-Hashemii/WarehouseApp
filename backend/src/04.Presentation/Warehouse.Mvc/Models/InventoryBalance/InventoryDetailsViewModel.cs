namespace Warehouse.Mvc.Models.InventoryBalance
{
    public class InventoryDetailsViewModel
    {
        public int StockLocationId { get; set; }

        public string StockLocationName { get; set; } = null!;

        public List<InventoryBalanceItemViewModel> Items { get; set; } = new();
    }
}
