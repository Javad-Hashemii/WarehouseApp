using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace WarehouseApp.Domain.Core.Dtos.Request
{
    public class ModifyRequestDto
    {
        public int RequestId { get; set; }
        public List<AddRequestItemDto> NewItems { get; set; } = new();
        public List<UpdateRequestItemDto> UpdatedItems { get; set; } = new();
        public List<int> RemovedItemIds { get; set; } = new();    
    }
}
