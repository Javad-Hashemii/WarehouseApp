using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class InventoryTransaction:BaseEntity
    {
        public int ProductId { get; set; }

        public int? SourceStockLocationId { get; set; }
        public int? DestinationStockLocationId { get; set; }

        public TransactionType TransactionType { get; set; }
        public InventoryTransactionStatus Status { get; set; }

        public decimal Quantity { get; set; }
        public decimal? QuantityReceived { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime? ReceivedAt { get; set; }

        public int? RequestId { get; set; }
        public int? RequestItemId { get; set; }

        public int PerformedByUserId { get; set; }
        public ApplicationUser PerformedByUser { get; set; } = null!;

        public string? Notes { get; set; }

        public Product Product { get; set; } = null!;

        public StockLocation? SourceStockLocation { get; set; }
        public StockLocation? DestinationStockLocation { get; set; }

        public Request? Request { get; set; }
        public RequestItem? RequestItem { get; set; }

    }
}
