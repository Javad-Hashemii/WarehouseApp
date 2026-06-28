using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestItem
{
    public class CreateRequestItemsDto
    {
        public int ProductId { get; set; }
        public decimal QuantityRequested { get; set; }
    }
}
