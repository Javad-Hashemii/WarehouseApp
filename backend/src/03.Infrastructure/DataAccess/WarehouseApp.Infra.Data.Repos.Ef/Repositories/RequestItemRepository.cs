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
        public async Task<bool> CreateRequestItemAsync(CreateRequestItemsDto createRequestItemsDto, CancellationToken ct)
        {
            var requestItem = new RequestItem
            {
                RequestId = createRequestItemsDto.RequestId,
                ProductId = createRequestItemsDto.ProductId,
                QuantityRequested = createRequestItemsDto.QuantityRequested,
                QuantityIssued = createRequestItemsDto.QuantityIssued,
                Notes = createRequestItemsDto.Notes
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
    }
}
