using Warehouse.Mvc.Models.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace Warehouse.Mvc.Mappers.Request
{
    public static class ModifyRequestMapper
    {
        public static (
        IReadOnlyCollection<AddRequestItemDto> NewItems,
        IReadOnlyCollection<UpdateRequestItemDto> UpdatedItems,
        IReadOnlyCollection<int> DeletedItemIds)
        MapEditItems(EditRequestViewModel viewModel)
        {
            var newItems = viewModel.Items
                .Where(x => !x.Id.HasValue)
                .Select(x => new AddRequestItemDto
                {
                    ProductId = x.ProductId,
                    QuantityRequested = x.QuantityRequested
                })
                .ToList();

            var updatedItems = viewModel.Items
                .Where(x => x.Id.HasValue)
                .Select(x => new UpdateRequestItemDto
                {
                    Id = x.Id!.Value,
                    ProductId = x.ProductId,
                    QuantityRequested = x.QuantityRequested
                })
                .ToList();

            var deletedItemIds = viewModel.DeletedItemIds
                .Distinct()
                .ToList();

            return (
                newItems,
                updatedItems,
                deletedItemIds
            );
        }
    }
}
