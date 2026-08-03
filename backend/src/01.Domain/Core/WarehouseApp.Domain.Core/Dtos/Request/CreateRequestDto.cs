using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class CreateRequestDto
    {
        public string RequestNumber { get; set; } = null!;
        public int WorkshopId { get; set; }
        public int WarehouseId { get; set; }
        public int RequestedByUserId { get; set; }
        public List<CreateRequestItemsDto> Items { get; set; } = new();
    }
}
