using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace Warehouse.Mvc.Models.Request
{
    public class ModifyRequestViewModel
    {
        public int RequestId { get; set; }
        public List<AddRequestItemDto> NewItems { get; set; } = new();
        public List<UpdateRequestItemDto> UpdatedItems { get; set; } = new();
        public List<int> DeletedItemIds { get; set; } = new();
    }
}
