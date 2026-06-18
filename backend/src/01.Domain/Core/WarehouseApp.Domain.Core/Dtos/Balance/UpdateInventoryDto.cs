using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.InventoryBalance
{
    public class UpdateInventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public int UpdatedByUserId { get; set; }
        public int InventoryId { get; set; }

    }
}
