using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class ManagerInventoryLocationsDto
    {
        public int WorkshopStockLocationId { get; set; }

        public int? WarehouseStockLocationId { get; set; }
    }
}
