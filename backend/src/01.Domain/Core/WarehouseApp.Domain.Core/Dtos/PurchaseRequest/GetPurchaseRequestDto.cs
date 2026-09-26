using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class GetPurchaseRequestDto
    {
        public int Id { get; set; }
        public string PurchaseRequestNumber { get; set; } = null!;
        public int RequestId { get; set; }
        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public PurchaseRequestStatus Status { get; set; }

        public string? Supplier { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Notes { get; set; }

        public List<GetPurchaseRequestItemDto> Items { get; set; } = new();
    }
}
