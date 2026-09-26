using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestHistory
{
    public class RequestHistoryDetailsDto
    {
        public int RequestId { get; set; }

        public string RequestNumber { get; set; } = null!;

        public string WorkshopName { get; set; } = null!;

        public string? WarehouseName { get; set; }

        public string CurrentStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<RequestTimelineItemDto> Timeline { get; set; } = new();
    }
}
