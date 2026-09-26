using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ConfirmPurchaseReceiptItemDto
    {
        public int PurchaseRequestItemId { get; set; }
        public decimal QuantityReceivedNow { get; set; }

    }
}
