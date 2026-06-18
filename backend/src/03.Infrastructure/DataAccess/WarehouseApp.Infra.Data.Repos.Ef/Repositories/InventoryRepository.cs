using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.InventoryBalance;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    internal class InventoryRepository(AppDbContext _context) : IInventoryRepository
    {
        public async Task<bool> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct)
        {
            var inventory = new InventoryBalance
            {
                ProductId = createInventoryDto.ProductId,
                WarehouseId = createInventoryDto.WarehouseId,
                Quantity = createInventoryDto.Quantity,
                CreatedAt = DateTime.Now,
                CreatedByUserId = createInventoryDto.UserId,
            };
            _context.InventoryBalances.Add(inventory);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<GetAllInventoryDto>> GetAllInventoriesAsync(CancellationToken ct)
        {
            return await _context.InventoryBalances
                .AsNoTracking()
                .Select(ib => new GetAllInventoryDto
                {
                    Id = ib.Id,
                    ProductId = ib.ProductId,
                    ProductName = ib.Product.Name,
                    WarehouseId = ib.WarehouseId,
                    Quantity = ib.Quantity,
                    LastUpdatedAt = ib.UpdatedAt ?? ib.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<List<GetAllInventoryDto>> GetInventoriesWithWarehouseId(int warehouseId, CancellationToken ct)
        {
            return await _context.InventoryBalances
                .AsNoTracking()
                .Where(w=>w.WarehouseId==warehouseId)
                .Select(ib => new GetAllInventoryDto
                {
                    Id = ib.Id,
                    ProductId = ib.ProductId,
                    ProductName = ib.Product.Name,
                    WarehouseId = ib.WarehouseId,
                    Quantity = ib.Quantity,
                    LastUpdatedAt = ib.UpdatedAt ?? ib.CreatedAt
                })
                .ToListAsync(ct);
        }


        public async Task<bool> UpdateInventoryAsync(UpdateInventoryDto updateInventoryBalanceDto,CancellationToken ct)
        {
            var result = _context.InventoryBalances
                .Where(ib => ib.Id == updateInventoryBalanceDto.InventoryId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(ib => ib.Quantity, updateInventoryBalanceDto.Quantity)
                .SetProperty(ib => ib.UpdatedAt, DateTime.Now)
                .SetProperty(ib => ib.UpdatedByUserId, updateInventoryBalanceDto.UpdatedByUserId), ct);
            return await result > 0;
        }


        public async Task<bool> IsInventoryExistAsync(int productId,int warehouseId, CancellationToken ct)
        {
            return await _context.InventoryBalances.AnyAsync(ib => ib.ProductId == productId && ib.WarehouseId == warehouseId, ct);
        }
    }
}
