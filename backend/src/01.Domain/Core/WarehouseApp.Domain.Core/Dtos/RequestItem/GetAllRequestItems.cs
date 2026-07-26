using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestItem
{
    public class GetAllRequestItems
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityRequested { get; set; }
        public decimal QuantityIssued { get; set; }
        public decimal? QuantityApproved { get; set; }
        public string? Notes { get; set; }
    }
}
