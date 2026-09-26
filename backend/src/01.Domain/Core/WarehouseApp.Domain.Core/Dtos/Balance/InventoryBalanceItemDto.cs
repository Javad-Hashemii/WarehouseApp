using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class InventoryBalanceItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Quantity { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
