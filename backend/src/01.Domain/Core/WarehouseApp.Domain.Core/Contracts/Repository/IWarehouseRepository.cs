using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Warehouse;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IWarehouseRepository
    {
        Task<bool> CreateWarehouseAsync(string name, int cityId, CancellationToken ct);
        Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct);
        Task<GetWarehouseDetailDto> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct);
        Task<bool> UpdateWarehouseAsync(int warehouseId, string name, int cityId, CancellationToken ct);
        Task<bool> DeleteWarehouseAsync(int warehouseId, CancellationToken ct);
        Task<bool> IsWarehouseExistAsync(string warehouseName, CancellationToken ct);

    }
}
