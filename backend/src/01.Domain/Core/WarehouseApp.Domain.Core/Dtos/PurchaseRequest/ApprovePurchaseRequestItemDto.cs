using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ApprovePurchaseRequestItemDto
    {
        public int PurchaseRequestItemId { get; set; }
        public decimal QuantityApproved { get; set; }

    }
}
