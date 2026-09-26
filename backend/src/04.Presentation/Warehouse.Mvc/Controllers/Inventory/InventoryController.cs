using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Mvc.Models.InventoryBalance;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Service;

namespace Warehouse.Mvc.Controllers.Inventory
{
    public class InventoryController(IInventoryService inventoryService) : Controller
    {
        public async Task<IActionResult> AdminIndex(CancellationToken ct)
        {
            var dto = await inventoryService.GetInventoryLocationsAsync(ct);

            var viewModel = new AdminInventoryIndexViewModel
            {
                Warehouses = dto.Warehouses
                    .Select(x => new InventoryLocationViewModel
                    {
                        StockLocationId = x.StockLocationId,
                        Name = x.Name,
                        CityName = x.CityName
                    })
                    .ToList(),

                Workshops = dto.Workshops
                    .Select(x => new InventoryLocationViewModel
                    {
                        StockLocationId = x.StockLocationId,
                        Name = x.Name,
                        CityName = x.CityName
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int stockLocationId,CancellationToken ct)
        {
            var dto = await inventoryService
                .GetInventoryDetailsAsync(stockLocationId, ct);

            if (dto == null)
                return NotFound();

            var viewModel = new InventoryDetailsViewModel
            {
                StockLocationId = dto.StockLocationId,
                StockLocationName = dto.StockLocationName,
                Items = dto.Items
                    .Select(i => new InventoryBalanceItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        LastUpdatedAt = i.LastUpdatedAt
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.WarehouseManager + "," + Roles.WorkshopManager)]
        public async Task<IActionResult> ManagerIndex(CancellationToken ct)
        {
            var result = await inventoryService
                .GetManagerInventoryLocationsAsync(ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction("Index", "Dashboard");
            }

            var viewModel = new ManagerInventoryIndexViewModel
            {
                WorkshopStockLocationId =
                    result.Data.WorkshopStockLocationId,

                WarehouseStockLocationId =
                    result.Data.WarehouseStockLocationId
            };

            return View(viewModel);
        }
    }
}
