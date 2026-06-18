using Microsoft.EntityFrameworkCore;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.Warehouse;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class WarehouseRepository(AppDbContext _context) : IWarehouseRepository
    {
        public async Task<bool> CreateWarehouseAsync(string name, int cityId, CancellationToken ct)
        {
            var warehouse = new Warehouse
            {
                Name = name,
                CityId = cityId
            };
            _context.Warehouses.Add(warehouse);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public Task<bool> DeleteWarehouseAsync(int warehouseId, CancellationToken ct) // implement soft delete?
        {
            throw new NotImplementedException();
        }

        public async Task<GetWarehouseDetailDto?> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct) //check this
        {
            return await _context.Warehouses
                .AsNoTracking()
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
                .Select(w => new GetAllWarehousesDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    CityId = w.CityId,
                })
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateWarehouseAsync(int warehouseId, string name, int cityId, CancellationToken ct)
        {
           var result = _context.Warehouses
                .Where(w => w.Id == warehouseId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(w => w.Name, name)
                .SetProperty(w => w.CityId, cityId), ct);
            return await result > 0;
        }

        public async Task<bool> IsWarehouseExistAsync(string warehouseName, CancellationToken ct)
        {
            return await _context.Warehouses.AnyAsync(w => w.Name == warehouseName, ct);
        }
    }
}
