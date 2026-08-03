using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class StockLocation: BaseEntity
    {
        public int? WarehouseId { get; set; }
        public int? WorkshopId { get; set; }

        public Warehouse? Warehouse { get; set; }
        public Workshop? Workshop { get; set; }

        public ICollection<InventoryBalance> InventoryBalances { get; set; }
            = new List<InventoryBalance>();

        public ICollection<InventoryTransaction> OutgoingTransactions { get; set; }
            = new List<InventoryTransaction>();

        public ICollection<InventoryTransaction> IncomingTransactions { get; set; }
            = new List<InventoryTransaction>();
    }
}
