using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Warehouse;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IWarehouseService
    {
        Task<ResultDto<bool>> CreateWarehouseAsync(CreateWarehouseDto createWarehouseDto, CancellationToken ct);
        Task<List<GetAllWarehousesDto>> GetWarehousesAsync(CancellationToken ct);
        Task<GetWarehouseDetailDto> GetWarehouseDetailAsync(int warehouseId, CancellationToken ct);
        Task<ResultDto<bool>> UpdateWarehouseAsync(UpdateWarehouseDto updateWarehouseDto, CancellationToken ct);
        Task<ResultDto<bool>> DeleteWarehouseAsync(int warehouseId,int deletedByUserId, CancellationToken ct);
    }
}
