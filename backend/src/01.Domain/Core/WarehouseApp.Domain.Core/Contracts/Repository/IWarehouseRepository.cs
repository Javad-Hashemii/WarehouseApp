using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.Warehouse;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IWarehouseRepository
    {
        Task<bool> CreateWarehouseAsync(string name, int cityId, CancellationToken ct);
        Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct);
        Task<GetWarehouseDetailDto> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct);
        Task<bool> UpdateWarehouseAsync(int warehouseId, string name, int cityId, CancellationToken ct);
        Task<bool> DeleteWarehouseAsync(int warehouseId,int deletedByUserId, CancellationToken ct);
        Task<bool> IsWarehouseExistAsync(string warehouseName, CancellationToken ct);
        Task<bool>IsWarehouseExistById(int warehouseId, CancellationToken ct);
        Task<int?> GetWarehouseIdByWorkshopId(int workshopId, CancellationToken ct);
        Task<List<InventoryLocationDto>> GetWarehouseInventoryLocationsAsync(CancellationToken ct);

    }
}
