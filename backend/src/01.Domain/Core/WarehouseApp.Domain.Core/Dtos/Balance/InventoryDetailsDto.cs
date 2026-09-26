using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class InventoryDetailsDto
    {
        public int StockLocationId { get; set; }

        public string StockLocationName { get; set; } = null!;

        public List<InventoryBalanceItemDto> Items { get; set; } = new();
    }
}
