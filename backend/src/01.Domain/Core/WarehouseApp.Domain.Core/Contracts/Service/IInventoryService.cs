using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.InventoryBalance;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IInventoryService
    {
        Task<ResultDto<bool>> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct);
        Task<List<GetAllInventoryDto>> GetAllInventoriesAsync(CancellationToken ct);
        Task<ResultDto<bool>> UpdateInventoryAsync(UpdateInventoryDto updateInventoryBalanceDto, CancellationToken ct);
    }
}
