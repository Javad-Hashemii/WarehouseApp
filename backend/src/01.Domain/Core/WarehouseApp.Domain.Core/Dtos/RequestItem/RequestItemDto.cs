using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestItem
{
    public class RequestItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal QuantityRequested { get; set; }
    }
}
