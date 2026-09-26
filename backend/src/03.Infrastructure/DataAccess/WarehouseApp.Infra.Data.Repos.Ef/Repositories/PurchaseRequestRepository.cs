using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.PurchaseRequest;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class PurchaseRequestRepository(AppDbContext _context):IPurchaseRequestRepository
    {
        public async Task AddAsync(PurchaseRequest purchaseRequest, CancellationToken ct)
        {
            await _context.PurchaseRequests.AddAsync(purchaseRequest, ct);
        }

        public async Task<GetPurchaseRequestDto?> GetPurchaseRequestWithDetail(int purchaseRequestId, CancellationToken ct)
        {
            return await _context.PurchaseRequests
                .Where(pr => pr.Id == purchaseRequestId)
                .Select(pr => new GetPurchaseRequestDto
                {
                    Id = pr.Id,
                    PurchaseRequestNumber = pr.PurchaseRequestNumber,
                    RequestId = pr.RequestId,
                    WorkshopId = pr.WorkshopId,
                    WorkshopName = pr.Workshop.Name,
                    Status = pr.Status,
                    CreatedAt = pr.CreatedAt,
                    Supplier = pr.Supplier,
                    PurchaseDate = pr.PurchaseDate,
                    ExpectedDeliveryDate = pr.ExpectedDeliveryDate,
                    Notes = pr.Notes,
                    Items = pr.Items.Select(i=>new GetPurchaseRequestItemDto
                    {
                        Id=i.Id,
                        ProductId = i.ProductId,
                        ProductName=i.Product.Name,
                        QuantityApproved=i.QuantityApproved,
                        QuantityRequested=i.QuantityRequested,
                        QuantityToPurchase=i.QuantityPurchased,
                        QuantityReceived=i.QuantityReceived,
                        Unit =i.Product.Unit
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<GetAllPurchaseRequestsDto>> GetSummariesByStatusesAsync(IReadOnlyCollection<PurchaseRequestStatus> statuses,CancellationToken ct)
        {
            return await _context.PurchaseRequests
                .AsNoTracking()
                .Where(pr => statuses.Contains(pr.Status))
                .Select(pr => new GetAllPurchaseRequestsDto
                {
                    Id = pr.Id,
                    PurchaseRequestNumber = pr.PurchaseRequestNumber,
                    RequestId = pr.RequestId,
                    WorkshopId = pr.WorkshopId,
                    WorkshopName = pr.Workshop.Name,
                    Status = pr.Status,
                    CreatedAt = pr.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<PurchaseRequest?> GetPurchaseRequest(int purchaseRequestId, CancellationToken ct)
        {
            return await _context.PurchaseRequests
                .Include(pr=>pr.Items)
                .FirstOrDefaultAsync(pr => pr.Id == purchaseRequestId);
        }

        public async Task<bool>DenyPurchaseRequestAsync(int purchaseRequestId, CancellationToken ct)
        {
            var result = await _context.PurchaseRequests
                .Where(pr => pr.Id == purchaseRequestId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(pr => pr.Status, PurchaseRequestStatus.Cancelled), ct);

            return result > 0;
        }

        public async Task<bool> IsPurchaseRequestExistsAsync(int purchaseRequestId, CancellationToken ct)
        {
            return await _context.PurchaseRequests
                .AnyAsync(pr=>pr.Id == purchaseRequestId,ct);
        }

        public async Task<PurchaseRequest?> GetPurchaseRequestForWarehouseReceiptAsync(int purchaseRequestId, CancellationToken ct)
        {
            return await _context.PurchaseRequests
                .Include(pr => pr.Items)
                .Include(pr => pr.Request)
                .ThenInclude(r => r.Warehouse)
                .ThenInclude(w => w.StockLocation)
                .FirstOrDefaultAsync(pr => pr.Id == purchaseRequestId, ct);
        }
    }
}
