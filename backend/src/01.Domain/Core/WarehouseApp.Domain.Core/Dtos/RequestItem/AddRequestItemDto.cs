using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestItem
{
    public class AddRequestItemDto
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityRequested { get; set; }
    }
}
