using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class ConfirmPurchaseDecisionItemDto
    {
        public int? RequestItemId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityToPurchase { get; set; }
    }
}
