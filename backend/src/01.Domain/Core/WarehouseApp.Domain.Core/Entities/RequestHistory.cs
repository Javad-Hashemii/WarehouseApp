using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class RequestHistory:BaseEntity
    {

        public int RequestId { get; set; }
        public RequestStatus FromStatus { get; set; }
        public RequestStatus ToStatus { get; set; }
        public int ChangedByUserId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Notes { get; set; }

        public Request Request { get; set; } = null!;
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
