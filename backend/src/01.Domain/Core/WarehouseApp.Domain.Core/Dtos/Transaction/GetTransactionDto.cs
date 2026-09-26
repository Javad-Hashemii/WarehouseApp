using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Transaction
{
    public class GetRequestTransactionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? SourceStockLocationName { get; set; }
        public string? DestinationStockLocationName { get; set; }
        public TransactionType TransactionType { get; set; }
        public InventoryTransactionStatus Status { get; set; }
        public decimal Quantity { get; set; }
        public decimal? QuantityReceived { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public int PerformedByUserId { get; set; }
        public string? PerformedByUsername { get; set; }
        public string? Notes { get; set; }

    }
}
