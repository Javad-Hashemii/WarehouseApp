namespace Warehouse.Mvc.Models.InventoryBalance
{
    public class AdminInventoryIndexViewModel
    {
        public List<InventoryLocationViewModel> Warehouses { get; set; } = new();

        public List<InventoryLocationViewModel> Workshops { get; set; } = new();
    }
}
