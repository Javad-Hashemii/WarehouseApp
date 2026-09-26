using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ProcessPurchaseItemDto
    {
        public int PurchaseRequestItemId { get; set; }

        public decimal QuantityPurchased { get; set; }
    }
}
