using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IRequestHistoryRepository
    {
        Task<bool> CreateRequestHistoryAsync(CreateRequestHistoryDto createRequestHistoryDto, CancellationToken ct);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId, CancellationToken ct);


    }
}
