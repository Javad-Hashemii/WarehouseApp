using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class GetPurchaseRequestItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public decimal QuantityRequested { get; set; }
        public decimal QuantityApproved { get; set; }
        public decimal QuantityToPurchase { get; set; }
        public decimal QuantityReceived { get; set; }

    }
}
