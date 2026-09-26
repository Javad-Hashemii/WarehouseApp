using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class PurchaseRequestItem:BaseEntity
    {
        public int PurchaseRequestId { get; set; }
        public int ProductId { get; set; }
        public int? RequestItemId { get; set; }

        public decimal QuantityRequested { get; set; }
        public decimal QuantityApproved { get; set; }
        public decimal QuantityPurchased { get; set; }
        public decimal QuantityReceived { get; set; }
        public string? Notes { get; set; }

        public PurchaseRequest PurchaseRequest { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public RequestItem? RequestItem { get; set; }
    }
}
