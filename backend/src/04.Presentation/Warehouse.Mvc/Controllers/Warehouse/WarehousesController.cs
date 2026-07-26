using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Warehouse.Mvc.Models.Warehouse;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.Warehouse;

namespace Warehouse.Mvc.Controllers.Warehouse
{
    public class WarehousesController(IWarehouseService _wareHouseService, ICityService _cityService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var warehouses = await _wareHouseService.GetWarehousesAsync(ct);
            return View(warehouses);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var viewModel = new CreateWarehouseViewModel();

            await PopulateCitiesAsync(viewModel, ct);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarehouseViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCitiesAsync(viewModel, ct);
                return View(viewModel);
            }
            var warehouse = new CreateWarehouseDto
            {
                Name = viewModel.Name,
                CityId = viewModel.CityId
            };
            var result = await _wareHouseService.CreateWarehouseAsync(warehouse, ct);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "خطا در ایجاد انبار");
                await PopulateCitiesAsync(viewModel, ct);
                return View(viewModel);
            }
            TempData["SuccessMessage"] = result.Message ?? "انبار با موفقیت ایجاد شد.";
            return RedirectToAction("Index");
        }

        private async Task PopulateCitiesAsync(CreateWarehouseViewModel viewModel,CancellationToken ct)
        {
            var cities = await _cityService.GetAllCities(ct);
            viewModel.Cities = cities.Select(city => new SelectListItem
            {
                Value = city.Id.ToString(),
                Text = city.Name
            }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            var result = await _wareHouseService.DeleteWarehouseAsync(id, userId, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message ?? "خطا در حذف انبار";
            }
            else
            {
                TempData["SuccessMessage"] = result.Message ?? "انبار با موفقیت حذف شد.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var warehouse = await _wareHouseService.GetWarehouseDetailAsync(id, ct);
            if (warehouse == null)
            {
                return NotFound();
            }
            var viewModel = new EditWarehouseViewModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                CityId = warehouse.CityId
            };
            await PopulateCitiesAsync(viewModel, ct);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditWarehouseViewModel viewModel, CancellationToken ct)
        {
            if(viewModel.Id <= 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                await PopulateCitiesAsync(viewModel, ct);
                return View(viewModel);
            }
            var warehouse = new UpdateWarehouseDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                CityId = viewModel.CityId
            };
            var result = await _wareHouseService.UpdateWarehouseAsync(warehouse, ct);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "خطا در ویرایش انبار");
                await PopulateCitiesAsync(viewModel, ct);
                return View(viewModel);
            }
            TempData["SuccessMessage"] = result.Message ?? "انبار با موفقیت ویرایش شد.";
            return RedirectToAction("Index");
        }

        private async Task PopulateCitiesAsync(EditWarehouseViewModel viewModel, CancellationToken ct)
        {
            var cities = await _cityService.GetAllCities(ct);
            viewModel.Cities = cities.Select(city => new SelectListItem
            {
                Value = city.Id.ToString(),
                Text = city.Name
            }).ToList();
        }
    }
}
