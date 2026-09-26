using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class HistoryBase:BaseEntity
    {
        public int ChangedByUserId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Notes { get; set; }

        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
