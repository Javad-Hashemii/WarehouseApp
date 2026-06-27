using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Transaction:BaseEntity
    {
        public int ProductId { get; set; }
        public int? SourceWarehouseId { get; set; } // add source and destination warehouse and replace it with this add workshop? nullable? aughhhh
        public int? DestinationWarehouseId { get; set; } 

        public TransactionType TransactionType { get; set; }
        public decimal Quantity { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? RequestId { get; set; }
        public int PerformedByUserId { get; set; }

        public string? Notes { get; set; }

        public Product Product { get; set; } = null!;
        public Warehouse? SourceWarehouse { get; set; }
        public Warehouse? DestinationWarehouse { get; set; }
        public Request? Request { get; set; }
    }
}
