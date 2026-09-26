using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IPurchaseRequestHistoryRepository
    {
        Task<List<RequestTimelineItemDto>> GetRequestTimelineAsync(int requestId,CancellationToken ct);
    }
}
