using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class InventoryLocationDto
    {
        public int StockLocationId { get; set; }
        public string Name { get; set; } = null!;
        public string CityName { get; set; } = null!;
    }
}
