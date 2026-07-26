using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IRequestItemRepository
    {
        Task<bool> AddRequestItemAsync(AddRequestItemDto addRequestItemDto, CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct);
        Task<List<RequestItem>> GetRequestItemsByIdsAsync(int requestId, List<int> ids, CancellationToken ct);
        Task<bool> IsRequestItemExist(int requestItemId, CancellationToken cancellationToken);
        Task<bool> UpdateRequestItemAsync(UpdateRequestItemDto updateRequestItemDto, CancellationToken cancellationToken);
        Task<bool> SaveChangesAsync(CancellationToken ct);

    }
}
