using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class PurchaseRequestHistoryRepository(AppDbContext _context): IPurchaseRequestHistoryRepository
    {
        public async Task<List<RequestTimelineItemDto>> GetRequestTimelineAsync(int requestId,CancellationToken ct)
        {
            return await _context.PurchaseRequestHistories
                .AsNoTracking()
                .Where(h => h.PurchaseRequest.RequestId == requestId)
                .Select(h => new RequestTimelineItemDto
                {
                    FromStatus = h.FromStatus.ToString(),
                    ToStatus = h.ToStatus.ToString(),
                    ChangedByUserId = h.ChangedByUserId,
                    ChangedByUserName = h.ChangedByUser.FullName,
                    ChangedAt = h.ChangedAt,
                    Notes = h.Notes,
                    Source = "Purchase"
                })
                .ToListAsync(ct);
        }
    }
}
