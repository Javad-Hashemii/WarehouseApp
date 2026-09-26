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
        public decimal QuantityIssued { get; set; }
        public decimal QuantityAvailable { get; set; } = 0;
        public decimal QuantityToIssue { get; set; }
        public decimal QuantityShortage { get; set; }
        public decimal QuantityReceived { get; set; }
    }
}
