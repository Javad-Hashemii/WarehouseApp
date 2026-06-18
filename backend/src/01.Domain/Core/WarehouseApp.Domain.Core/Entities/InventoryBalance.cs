using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class InventoryBalance:BaseEntity
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
    }
}
