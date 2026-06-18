using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.InventoryTransaction
{
    public class GetAllTransactionsDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public int RequestId { get; set; } //???? should be null or what
        public int PerformedByUserId { get; set; } //wtf is this
        public string? Notes { get; set; }

    }
}
