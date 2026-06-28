using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class RequestRepository(AppDbContext _context) : IRequestRepository
    {
        public async Task<bool> CreateRequestAsync(CreateRequestDto createRequestDto, CancellationToken ct)
        {

            var request = new Request
            {
                RequestNumber = createRequestDto.RequestNumber,
                RequestedByUserId = createRequestDto.RequestedByUserId,
                WorkshopId = createRequestDto.WorkshopId,
                WarehouseId = createRequestDto.WarehouseId,
                Status = createRequestDto.RequestedStatus, //was??!
                CreatedAt = DateTime.UtcNow,

                RequestItems = createRequestDto.Items.Select(itemDto => new RequestItem
                {
                    ProductId = itemDto.ProductId,
                    QuantityRequested = itemDto.QuantityRequested
                }).ToList()
            };
            _context.Requests.Add(request);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct) //implement soft delete?
        {
            throw new NotImplementedException();
        }

        public async Task<GetRequestDto?> GetByIdWithDetailsAsync(int requestId, CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r=>r.Id == requestId)
                .Select(r => new GetRequestDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    RequestedByUserId = r.RequestedByUserId,
                    WorkshopId = r.WorkshopId,
                    WarehouseId = r.WarehouseId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status, CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r => r.Status == status)
                .Select(r => new GetAllRequestsDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    RequestedByUserId = r.RequestedByUserId,
                    WorkshopId = r.WorkshopId,
                    WarehouseId = r.WarehouseId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId, CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r => r.WarehouseId == warehouseId)
                .Select(r => new GetAllRequestsDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    RequestedByUserId = r.RequestedByUserId,
                    WorkshopId = r.WorkshopId,
                    WarehouseId = r.WarehouseId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId, CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r => r.WorkshopId == workshopId)
                .Select(r => new GetAllRequestsDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    RequestedByUserId = r.RequestedByUserId,
                    WorkshopId = r.WorkshopId,
                    WarehouseId = r.WarehouseId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateRequestAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct)
        {
            var result = _context.Requests
                .Where(r => r.Id == updateStatusDto.RequestId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(r => r.Status, updateStatusDto.NewStatus)
                .SetProperty(r => r.UpdatedAt, DateTime.UtcNow)
                .SetProperty(r => r.UpdatedByUserId, updateStatusDto.ChangedByUserId), ct); 
            return await result > 0;

        }

        public Task<bool> UpdateStatusAsync(UpdateRequestStatusDto updateRequestStatusDto, CancellationToken ct, string? notes = null)
        {
            throw new NotImplementedException();
        }


    }
}
