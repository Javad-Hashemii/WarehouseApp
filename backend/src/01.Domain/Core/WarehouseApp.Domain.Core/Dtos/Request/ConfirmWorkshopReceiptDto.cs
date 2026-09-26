using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class ConfirmWorkshopReceiptDto
    {
        public int RequestId { get; set; }
        public List<ConfirmWorkshopReceiptItemDto> Items { get; set; }
    }
}
