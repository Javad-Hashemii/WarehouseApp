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
using WarehouseApp.Infra.Data.Repos.Ef.Extensions;
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
                    WorkshopName=r.Workshop.Name,
                    WarehouseId = r.WarehouseId,
                    WarehouseStockLocationId= r.Warehouse != null && r.Warehouse.StockLocation != null
                    ? r.Warehouse.StockLocation.Id : null,
                    WorkshopStockLocationId = r.Workshop.StockLocation.Id,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    Items = r.RequestItems.Select(i => new RequestItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        QuantityRequested = i.QuantityRequested,
                        QuantityIssued = i.QuantityIssued,
                        QuantityReceived = i.QuantityReceived,
                        ProductName = i.Product.Name,
                        Unit = i.Product.Unit
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByStatusesAsync(IReadOnlyCollection<RequestStatus> statuses, CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Where(r => statuses.Contains(r.Status))
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
                .ToListAsync(ct);
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
        public void  RemoveRequestItems(IEnumerable<RequestItem> items) //change the return value?
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

        public async Task<bool> IsRequestIdExistAsync(int requestId, CancellationToken ct)
        {
            return await  _context.Requests.AnyAsync(r=>r.Id==requestId && !r.IsDeleted, ct);
        }


        public async Task<PaginatedResult<GetAllRequestsForHistoryDto>> GetRequestsForHistoryAsync(
            string? search,
            int? workshopId,
            int? warehouseId,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            var query = _context.Requests
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.RequestNumber.Contains(search));
            }

            if (workshopId.HasValue)
            {
                query = query.Where(r =>
                    r.WorkshopId == workshopId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(r =>
                    r.WarehouseId == warehouseId.Value);
            }

            var projectedQuery = query
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.RequestNumber,

                    r.WorkshopId,
                    WorkshopName = r.Workshop.Name,

                    r.WarehouseId,
                    WarehouseName = r.Warehouse != null
                        ? r.Warehouse.Name
                        : null,

                    RequestStatus = r.Status,

                    PurchaseStatus = _context.PurchaseRequests
                        .Where(p => p.RequestId == r.Id)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => (PurchaseRequestStatus?)p.Status)
                        .FirstOrDefault(),

                    r.CreatedAt
                });

            var pagedResult = await projectedQuery
                .ToPaginatedResultAsync(page, pageSize, ct);

            return new PaginatedResult<GetAllRequestsForHistoryDto>
            {
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,

                Items = pagedResult.Items
                    .Select(r => new GetAllRequestsForHistoryDto
                    {
                        Id = r.Id,
                        RequestNumber = r.RequestNumber,

                        WorkshopId = r.WorkshopId,
                        WorkshopName = r.WorkshopName,

                        WarehouseId = r.WarehouseId,
                        WarehouseName = r.WarehouseName,

                        CurrentStatus =
                            r.RequestStatus == RequestStatus.PendingPurchase &&
                            r.PurchaseStatus.HasValue
                                ? r.PurchaseStatus.Value.ToString()
                                : r.RequestStatus.ToString(),

                        CreatedAt = r.CreatedAt
                    })
                    .ToList()
            };
        }

        public async Task<Request?> GetByIdWithHistoryDetailsAsync(int requestId,CancellationToken ct)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(r => r.Workshop)
                .Include(r => r.Warehouse)
                .FirstOrDefaultAsync(r => r.Id == requestId, ct);
        }
    }
}


