using Microsoft.EntityFrameworkCore;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.Warehouse;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class WarehouseRepository(AppDbContext _context) :   IWarehouseRepository
    {
        public async Task<bool> CreateWarehouseAsync(string name, int cityId, CancellationToken ct)
        {
            var warehouse = new Warehouse
            {
                Name = name,
                CityId = cityId,
                StockLocation = new StockLocation
                {
                    Name = name
                }
            };
            _context.Warehouses.Add(warehouse);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteWarehouseAsync(int warehouseId,int deletedByUserId, CancellationToken ct) // implement soft delete?
        {
            var result = await _context.Warehouses
                .Where(w => w.Id == warehouseId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(w => w.IsDeleted, true)
                .SetProperty(w=>w.DeletedAt,DateTime.UtcNow)
                .SetProperty(w=>w.DeletedByUserId,deletedByUserId)
                );
            return result > 0;
        }

        public async Task<GetWarehouseDetailDto?> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct) //check this
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(w=>!w.IsDeleted)
                .Select(w => new GetWarehouseDetailDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                })
                .FirstOrDefaultAsync(w => w.Id == warehouseId, ct);
        }

        public async Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(w=>!w.IsDeleted)
                .Select(w => new GetAllWarehousesDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                    CityName= w.City.Name
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateWarehouseAsync(int warehouseId,string name,int cityId,CancellationToken ct)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.StockLocation)
                .FirstAsync(
                    w => w.Id == warehouseId && !w.IsDeleted,
                    ct);

            warehouse.Name = name;
            warehouse.CityId = cityId;

            warehouse.StockLocation.Name = name;

            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> IsWarehouseExistAsync(string warehouseName, CancellationToken ct)
        {
            return await _context.Warehouses.AnyAsync(w => w.Name == warehouseName && !w.IsDeleted, ct);
        }

        public async Task<bool> IsWarehouseExistById(int warehouseId, CancellationToken ct)
        {
            return await _context.Warehouses.AnyAsync(w=>w.Id==warehouseId && !w.IsDeleted, ct);
        }

        public async Task<int?> GetWarehouseIdByWorkshopId(int workshopId, CancellationToken ct)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.WorkshopId == workshopId && !w.IsDeleted)
                .Select(w => (int?)w.Id)
                .SingleOrDefaultAsync(ct);
        }

        public async Task<List<InventoryLocationDto>> GetWarehouseInventoryLocationsAsync(CancellationToken ct)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(w => !w.IsDeleted)
                .Select(w => new InventoryLocationDto
                {
                    StockLocationId = w.StockLocation.Id,
                    Name = w.Name,
                    CityName = w.City.Name
                })
                .OrderBy(w => w.Name)
                .ToListAsync(ct);
        }
    }
}
