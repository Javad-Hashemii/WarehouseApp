using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IRequestHistoryRepository
    {
        Task CreateRequestHistory(CreateRequestHistoryDto createRequestHistoryDto);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct);
        Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId, CancellationToken ct);

        Task<List<GetRequestHistoryDetail>> GetRequestHistoryDetailsAsync(int requestId,CancellationToken ct);
        Task<List<RequestTimelineItemDto>> GetRequestTimelineAsync(int requestId,CancellationToken ct);
    }
}
