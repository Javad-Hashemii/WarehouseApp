using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class RequestHistoryRepository(AppDbContext _context) : IRequestHistoryRepository
    {
        public async Task CreateRequestHistory(CreateRequestHistoryDto createRequestHistoryDto)
        {
            var requestHistory = new RequestHistory
            {
                RequestId = createRequestHistoryDto.RequestId,
                FromStatus = createRequestHistoryDto.FromStatus,
                ToStatus = createRequestHistoryDto.ToStatus,
                ChangedByUserId = createRequestHistoryDto.ChangedByUserId,
                Notes = createRequestHistoryDto.Notes,
                ChangedAt = DateTime.UtcNow
            };  
            _context.RequestHistories.Add(requestHistory);
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .Select(rh => new GetAllRequestHistoriesDto
                {
                    Id = rh.Id,
                    RequestId = rh.RequestId,
                    FromStatus = rh.FromStatus,
                    ToStatus = rh.ToStatus,
                    ChangedByUserId = rh.ChangedByUserId,
                    ChangedAt = rh.ChangedAt,
                    Notes = rh.Notes
                })
                .ToListAsync(ct);
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId, CancellationToken ct)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .Where(rh => rh.RequestId == requestId)
                .Select(rh => new GetAllRequestHistoriesDto
                {
                    Id = rh.Id,
                    RequestId = rh.RequestId,
                    FromStatus = rh.FromStatus,
                    ToStatus = rh.ToStatus,
                    ChangedByUserId = rh.ChangedByUserId,
                    ChangedAt = rh.ChangedAt,
                    Notes = rh.Notes
                })
                .ToListAsync(ct);
        }

        public async Task<List<GetRequestHistoryDetail>> GetRequestHistoryDetailsAsync(int requestId, CancellationToken ct)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .Where(rh => rh.RequestId == requestId)
                .OrderByDescending(rh => rh.ChangedAt)
                .Select(rh => new GetRequestHistoryDetail
                {
                    Id = rh.Id,
                    RequestId = rh.RequestId,
                    FromStatus = rh.FromStatus,
                    ToStatus = rh.ToStatus,
                    ChangedByUserId = rh.ChangedByUserId,
                    ChangedByUserName = rh.ChangedByUser.FullName,
                    ChangedAt = rh.ChangedAt,
                    Notes = rh.Notes
                })
                .ToListAsync(ct);
        }


        public async Task<List<RequestTimelineItemDto>> GetRequestTimelineAsync(
            int requestId,
            CancellationToken ct)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .Where(h => h.RequestId == requestId)
                .Select(h => new RequestTimelineItemDto
                {
                    FromStatus = h.FromStatus.ToString(),
                    ToStatus = h.ToStatus.ToString(),
                    ChangedByUserId = h.ChangedByUserId,
                    ChangedByUserName = h.ChangedByUser.FullName,
                    ChangedAt = h.ChangedAt,
                    Notes = h.Notes,
                    Source = "Request"
                })
                .ToListAsync(ct);
        }
    }
}
