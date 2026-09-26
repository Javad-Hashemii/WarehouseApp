using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class GetAllRequestsForHistoryDto
    {
        public int Id { get; set; }

        public string RequestNumber { get; set; } = null!;

        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; } = null!;

        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        public string CurrentStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime LastChangedAt { get; set; }
    }
}
