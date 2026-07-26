using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
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
                Status = RequestStatus.PendingWorkshop,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = createRequestDto.RequestedByUserId,

                RequestItems = createRequestDto.Items.Select(itemDto => new RequestItem
                {
                    ProductId = itemDto.ProductId,
                    QuantityRequested = itemDto.QuantityRequested,
                    QuantityIssued=0,
                    QuantityReceived = 0
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
                .Where(r => r.Id == requestId)
                .Select(r => new GetRequestDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    RequestedByUserId = r.RequestedByUserId,
                    WorkshopId = r.WorkshopId,
                    WarehouseId = r.WarehouseId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    // Map the items here!
                    Items = r.RequestItems.Select(i => new RequestItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        QuantityRequested = i.QuantityRequested,
                        ProductName = i.Product.Name // If you have the navigation property setup
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status, CancellationToken ct)
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
                    WorkshopName = r.Workshop.Name,
                    WarehouseId = r.WarehouseId,
                    WarehouseName = r.Warehouse.Name,
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
                .SetProperty(r => r.UpdatedByUserId, updateStatusDto.UpdatedByUserId), ct);
            return await result > 0;

        }

        public async Task<bool> UpdateStatusAsync(UpdateRequestStatusDto updateRequestStatusDto, CancellationToken ct, string? notes = null)
        {
            var result = await _context.Requests
                .Where(r => r.Id == updateRequestStatusDto.RequestId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(r => r.Status, updateRequestStatusDto.NewStatus)
                .SetProperty(r => r.UpdatedAt, DateTime.UtcNow)
                .SetProperty(r => r.UpdatedByUserId, updateRequestStatusDto.UpdatedByUserId), ct);
            return result > 0;
        }

        public async Task<List<RequestItem>> GetRequestItemsByIdsAsync(int requestId, IReadOnlyCollection<int> itemIds, CancellationToken ct)
        {
            return await _context.RequestItems
                .Where(x => x.RequestId == requestId && itemIds.Contains(x.Id))
                .ToListAsync(ct);
        }
        public async Task AddRequestItemsAsync(IEnumerable<RequestItem> items, CancellationToken ct)
        {
            await _context.RequestItems.AddRangeAsync(items, ct);
        }
        public void  RemoveRequestItems(IEnumerable<RequestItem> items) //change the return value
        {
             _context.RequestItems.RemoveRange(items);
        }


        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<bool>IsRequestNumberExistAsync(string requestNumber,CancellationToken ct)
        {
            return await _context.Requests.AnyAsync(r => r.RequestNumber == requestNumber);
        }

        public async Task<Request?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
        }

        public async Task<List<GetAllRequestsDto>> GetAllSummariesAsync(CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r => !r.IsDeleted)
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
                .ToListAsync(ct);
        }
    }
}


