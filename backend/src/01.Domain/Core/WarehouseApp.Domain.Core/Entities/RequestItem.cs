using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class RequestItem:BaseEntity
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityRequested { get; set; }
        public decimal QuantityIssued { get; set; } // not sure about this too
        public string? Notes { get; set; } // is this even needed?

        public Request Request { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
