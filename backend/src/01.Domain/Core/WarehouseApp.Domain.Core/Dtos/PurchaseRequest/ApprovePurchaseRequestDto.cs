using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ApprovePurchaseRequestDto
    {
        public int PurchaseRequestId { get; set; }
        public List<ApprovePurchaseRequestItemDto> Items { get; set; } = new();
    }
}
