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
                    Quantity = ib.Quantity,
                    LastUpdatedAt = ib.UpdatedAt ?? ib.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<List<GetAllInventoryDto>> GetInventoriesWithWarehouseId(int warehouseId, CancellationToken ct)
        {
            return await _context.InventoryBalances
                .AsNoTracking()
                .Select(ib => new GetAllInventoryDto
                {
                    Id = ib.Id,
                    ProductId = ib.ProductId,
                    ProductName = ib.Product.Name,
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
            return await _context.InventoryBalances.AnyAsync(ib => ib.ProductId == productId, ct);
        }

        public async Task<List<GetInventoryDto>> GetInventoryBalanceForProductsAsync(List<int> productIds, int? stockLocationId, CancellationToken ct )
        {
            return await _context.InventoryBalances
                .AsNoTracking()
                .Where(ib => productIds.Contains(ib.ProductId) && ib.StockLocationId == stockLocationId)
                .Select(ib => new GetInventoryDto
                {
                    ProductId = ib.ProductId,
                    Quantity = ib.Quantity
                })
                .ToListAsync(ct);
        }
        public async Task<List<InventoryBalance>> GetInventoryBalancesForUpdateAsync(IReadOnlyCollection<int> productIds, int stockLocationId, CancellationToken ct)
        {
            return await _context.InventoryBalances
                .Where(ib => ib.StockLocationId == stockLocationId && productIds.Contains(ib.ProductId))
                .ToListAsync(ct);
        }

        public async Task AddInventoryBalancesAsync(IEnumerable<InventoryBalance> inventoryBalances, CancellationToken ct)
        {
            await _context.InventoryBalances.AddRangeAsync(inventoryBalances, ct);
        }

        public async Task<List<GetInventoryBalanceDto>> GetInventoryBalancesAsync(CancellationToken ct)
        {
            return await _context.InventoryBalances
                .AsNoTracking()
                .Select(i => new GetInventoryBalanceDto
                {
                    Id = i.Id,

                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,

                    StockLocationId = i.StockLocationId,
                    StockLocationName = i.StockLocation.Name,

                    LocationType = i.StockLocation.WarehouseId != null
                        ? "Warehouse"
                        : "Workshop",

                    Quantity = i.Quantity,
                    LastUpdatedAt = i.LastUpdatedAt
                })
                .OrderBy(i => i.StockLocationName)
                .ThenBy(i => i.ProductName)
                .ToListAsync(ct);
        }

        public async Task<InventoryDetailsDto?> GetInventoryDetailsAsync(int stockLocationId,CancellationToken ct)
        {
            var stockLocation = await _context.StockLocations
                .AsNoTracking()
                .Where(sl => sl.Id == stockLocationId && !sl.IsDeleted)
                .Select(sl => new
                {
                    sl.Id,
                    sl.Name
                })
                .FirstOrDefaultAsync(ct);

            if (stockLocation == null)
                return null;

            var items = await _context.InventoryBalances
                .AsNoTracking()
                .Where(i => i.StockLocationId == stockLocationId && !i.IsDeleted)
                .Select(i => new InventoryBalanceItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    LastUpdatedAt = i.LastUpdatedAt
                })
                .OrderBy(i => i.ProductName)
                .ToListAsync(ct);

            return new InventoryDetailsDto
            {
                StockLocationId = stockLocation.Id,
                StockLocationName = stockLocation.Name,
                Items = items
            };
        }
        public async Task<ManagerInventoryLocationsDto?> GetManagerInventoryLocationsAsync(int workshopId,CancellationToken ct)
        {
            return await _context.Workshops
                .AsNoTracking()
                .Where(w => w.Id == workshopId && !w.IsDeleted)
                .Select(w => new ManagerInventoryLocationsDto
                {
                    WorkshopStockLocationId = w.StockLocation.Id,

                    WarehouseStockLocationId = w.Warehouse != null
                        ? w.Warehouse.StockLocation.Id
                        : null
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
