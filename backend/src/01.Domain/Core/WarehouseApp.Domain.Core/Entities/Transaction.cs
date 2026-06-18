using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Transaction:BaseEntity
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }

        public TransactionType TransactionType { get; set; }
        public decimal Quantity { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? RequestId { get; set; }
        public int PerformedByUserId { get; set; }

        public string? Notes { get; set; }

        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public Request? Request { get; set; }
    }
}
