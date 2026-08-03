using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class InventoryBalance:BaseEntity
    {
        public int ProductId { get; set; }
        public int StockLocationId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Product Product { get; set; } = null!;
        public StockLocation StockLocation { get; set; } = null!;
    }
}
