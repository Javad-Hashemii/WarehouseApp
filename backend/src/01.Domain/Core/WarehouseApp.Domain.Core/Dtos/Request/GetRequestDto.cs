using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class GetRequestDto
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
        public int RequestedByUserId { get; set; }
        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public int? WarehouseId { get; set; }
        public int? WarehouseStockLocationId { get; set; }
        public int? WorkshopStockLocationId { get; set; }

        public RequestStatus Status { get; set; }
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<RequestItemDto> Items { get; set; } = new();
    }
}
