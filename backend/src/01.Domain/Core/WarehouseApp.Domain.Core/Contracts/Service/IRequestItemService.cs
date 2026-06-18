using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IRequestItemService
    {
        Task<ResultDto<bool>> CreateRequestItemAsync(CreateRequestItemsDto createRequestItemsDto, CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct);
        Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct);
    }
}
