using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.Category;
using WarehouseApp.Domain.Core.Dtos.InventoryBalance;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct);
        Task<List<GetAllInventoryDto>> GetAllInventoriesAsync(CancellationToken ct);
        Task<bool> UpdateInventoryAsync(UpdateInventoryDto updateInventoryBalanceDto,CancellationToken ct);
        Task<bool> IsInventoryExistAsync(int productId, int warehouseId, CancellationToken ct);

    }
}
