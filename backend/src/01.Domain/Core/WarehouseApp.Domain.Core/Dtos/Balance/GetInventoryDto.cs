using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.InventoryBalance
{
    public class GetInventoryDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
