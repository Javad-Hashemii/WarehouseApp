using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.RequestHistory
{
    public class CreateRequestHistoryDto
    {
        public int RequestId { get; set; }
        public RequestStatus FromStatus { get; set; }
        public RequestStatus ToStatus { get; set; }
        public int ChangedByUserId { get; set; }
        public string? Notes { get; set; }
    }
}
