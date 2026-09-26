using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class ConfirmPurchaseDecisionDto
    {
        public int RequestId { get; set; }
        public List<ConfirmPurchaseDecisionItemDto> Items { get; set; } = new();
    }
}
