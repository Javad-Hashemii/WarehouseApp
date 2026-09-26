using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class GetInventoryLocationsDto
    {
        public List<InventoryLocationDto> Warehouses { get; set; } = new();
        public List<InventoryLocationDto> Workshops { get; set; } = new();
    }
}
