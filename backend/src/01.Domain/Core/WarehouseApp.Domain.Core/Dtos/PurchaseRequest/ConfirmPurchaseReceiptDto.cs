using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ConfirmPurchaseReceiptDto
    {
        public int PurchaseRequestId { get; set; }
        public List<ConfirmPurchaseReceiptItemDto> Items { get; set; } = new();
    }
}
