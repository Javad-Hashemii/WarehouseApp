using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestHistory
{
    public class RequestTimelineItemDto
    {
        public string FromStatus { get; set; } = null!;

        public string ToStatus { get; set; } = null!;

        public int ChangedByUserId { get; set; }

        public string? ChangedByUserName { get; set; }

        public DateTime ChangedAt { get; set; }

        public string? Notes { get; set; }

        public string Source { get; set; } = null!;
    }
}
