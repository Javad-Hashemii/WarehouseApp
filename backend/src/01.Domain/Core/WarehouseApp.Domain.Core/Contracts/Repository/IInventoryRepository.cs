using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Balance;
using WarehouseApp.Domain.Core.Dtos.Category;
using WarehouseApp.Domain.Core.Dtos.InventoryBalance;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> CreateInventoryAsync(CreateInventoryDto createInventoryDto, CancellationToken ct);
        Task<List<GetAllInventoryDto>> GetAllInventoriesAsync(CancellationToken ct);
        Task<bool> UpdateInventoryAsync(UpdateInventoryDto updateInventoryBalanceDto,CancellationToken ct);
        Task<bool> IsInventoryExistAsync(int productId, int warehouseId, CancellationToken ct);
        Task<List<GetInventoryDto>> GetInventoryBalanceForProductsAsync(List<int> productIds, int? stockLocation, CancellationToken ct);
        Task<List<InventoryBalance>> GetInventoryBalancesForUpdateAsync(IReadOnlyCollection<int> productIds, int stockLocationId, CancellationToken ct);
        Task AddInventoryBalancesAsync(IEnumerable<InventoryBalance> inventoryBalances, CancellationToken ct);
        Task<List<GetInventoryBalanceDto>> GetInventoryBalancesAsync(CancellationToken ct);
        Task<InventoryDetailsDto?> GetInventoryDetailsAsync(int stockLocationId, CancellationToken ct);
        Task<ManagerInventoryLocationsDto?> GetManagerInventoryLocationsAsync(int workshopId,CancellationToken ct);

    }
}
