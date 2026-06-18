using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Domain.Core.Dtos.InventoryBalance
{
    public class GetAllInventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastUpdatedAt   { get; set; }

    }
}
