using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Balance
{
    public class GetInventoryBalanceDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public int StockLocationId { get; set; }
        public string StockLocationName { get; set; } = null!;

        public string LocationType { get; set; } = null!;

        public decimal Quantity { get; set; }

        public DateTime LastUpdatedAt { get; set; }

    }
}
