using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IRequestItemRepository
    {
        Task<bool> AddRequestItemAsync(AddRequestItemDto createRequestItemsDto, CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct);
        Task<bool> IsRequestItemExist(int requestItemId, CancellationToken cancellationToken);
        Task<bool> UpdateRequestItemAsync(UpdateRequestItemDto updateRequestItemDto, CancellationToken cancellationToken);


    }
}
