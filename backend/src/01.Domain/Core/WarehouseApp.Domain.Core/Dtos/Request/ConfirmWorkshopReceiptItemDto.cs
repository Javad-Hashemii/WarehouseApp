using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class ConfirmWorkshopReceiptItemDto
    {
        public int RequestItemId { get; set; }
        public decimal QuantityReceived { get; set; }
    }
}
