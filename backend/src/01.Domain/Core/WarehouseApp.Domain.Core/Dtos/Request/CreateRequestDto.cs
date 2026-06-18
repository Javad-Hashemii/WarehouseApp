using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class CreateRequestDto
    {
        public string RequestNumber { get; set; }
        public int RequestedByUserId { get; set; }
        public int WorkshopId { get; set; }
        public int? WarehouseId { get; set; }
        public RequestStatus RequestedStatus { get; set; }
    }
}
