using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class UpdateRequestStatusDto
    {
        public int RequestId { get; set; }
        public int UpdatedByUserId { get; set; }
        public RequestStatus NewStatus { get; set; }
        public string? Notes { get; set; }
    }
}
