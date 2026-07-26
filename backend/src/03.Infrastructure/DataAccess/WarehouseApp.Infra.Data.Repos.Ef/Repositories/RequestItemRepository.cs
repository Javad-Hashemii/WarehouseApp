using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class RequestItemRepository(AppDbContext _context) : IRequestItemRepository
    {
        public async Task<bool> AddRequestItemAsync(AddRequestItemDto addRequestItemDto, CancellationToken ct)
        {
            var requestItem = new RequestItem
            {
                RequestId = addRequestItemDto.RequestId,
                ProductId = addRequestItemDto.ProductId,
                QuantityRequested = addRequestItemDto.QuantityRequested,
            };  
            _context.RequestItems.Add(requestItem);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct)
        {
            return await _context.RequestItems
                .AsNoTracking()
                .Select(ri => new GetAllRequestItems
                {
                    Id = ri.Id,
                    RequestId = ri.RequestId,
                    ProductId = ri.ProductId,
                    QuantityRequested = ri.QuantityRequested,
                    QuantityIssued = ri.QuantityIssued,
                    QuantityApproved=ri.QuantityApproved,
                    Notes = ri.Notes
                })
                .ToListAsync(ct);   
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct)
        {
            return await _context.RequestItems
                .AsNoTracking()
                .Where(ri => ri.RequestId == requestId)
                .Select(ri => new GetAllRequestItems
                {
                    Id = ri.Id,
                    RequestId = ri.RequestId,
                    ProductId = ri.ProductId,
                    QuantityRequested = ri.QuantityRequested,
                    QuantityIssued = ri.QuantityIssued,
                    QuantityApproved=ri.QuantityApproved,
                    Notes = ri.Notes
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateRequestItemAsync(UpdateRequestItemDto updateRequestItemDto, CancellationToken cancellationToken)
        {
            var result = _context.RequestItems
                .Where(ri => ri.Id == updateRequestItemDto.Id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(ri => ri.ProductId, updateRequestItemDto.ProductId)
                .SetProperty(ri => ri.QuantityRequested, updateRequestItemDto.QuantityRequested),cancellationToken);
            return await result > 0;
        }

        public async Task<bool> IsRequestItemExist(int requestItemId,CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(ri => ri.Id == requestItemId, cancellationToken);

        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<List<RequestItem>> GetRequestItemsByIdsAsync(int requestId, List<int> ids, CancellationToken ct)
        {
            return await _context.RequestItems
                .Where(ri => ri.RequestId == requestId && ids.Contains(ri.Id))
                .ToListAsync(ct);
        }

    }
}
