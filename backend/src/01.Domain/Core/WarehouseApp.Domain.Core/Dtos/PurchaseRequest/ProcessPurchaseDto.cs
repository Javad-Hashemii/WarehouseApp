using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class ProcessPurchaseDto
    {
        public int PurchaseRequestId { get; set; }
        public string Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public List<ProcessPurchaseItemDto> Items { get; set; }
    }
}
