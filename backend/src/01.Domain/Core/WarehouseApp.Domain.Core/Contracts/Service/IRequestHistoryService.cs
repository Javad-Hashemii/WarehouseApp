using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IRequestHistoryService
    {
        void CreateRequestHistoryAsync(CreateRequestHistoryDto createRequestHistoryDto);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId, CancellationToken ct);

    }
}
