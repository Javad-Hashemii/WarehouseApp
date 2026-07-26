using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Warehouse.Mvc.Models.Workshop;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.Workshop;

namespace Warehouse.Mvc.Controllers.Workshop
{
    public class WorkshopsController(IWorkshopService _workshopService, ICityService _cityService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var workshops = await _workshopService.GetWorkshopsAsync(ct);
            return View(workshops);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var viewModel = new CreateWorkshopViewModel();
            await PopulateCitiesCreate(viewModel, ct);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWorkshopViewModel viewModel, CancellationToken ct) 
        {
            if(!ModelState.IsValid)
            {
                await PopulateCitiesCreate(viewModel, ct);
                return View(viewModel);
            }

            var workshop = new CreateWorkshopDto
            {
                Name = viewModel.Name,
                CityId = viewModel.CityId
            };

            var result = await _workshopService.CreateWorkshopAsync(workshop, ct);

            if(!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await PopulateCitiesCreate(viewModel, ct);
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "کارگاه با موفقیت ایجاد شد";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCitiesCreate(CreateWorkshopViewModel viewModel, CancellationToken ct)
        {
            var cities = await _cityService.GetAllCities(ct);
            viewModel.Cities = cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = await _workshopService
                  .GetWorkshopDetailAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }

            var viewModel = new EditWorkshopViewModel
            {
                Id= result.Data.Id,
                Name = result.Data.Name,
                CityId = result.Data.CityId
            };
            await PopulateCitiesEdit(viewModel, ct);
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditWorkshopViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCitiesEdit(viewModel, ct);
                return View(viewModel);
            }
            var workshop = new UpdateWorkshopDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                CityId = viewModel.CityId
            };
            var result = await _workshopService.UpdateWorkshopAsync(workshop, ct);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await PopulateCitiesEdit(viewModel, ct);
                return View(viewModel);
            }
            TempData["SuccessMessage"] = "کارگاه با موفقیت ویرایش شد";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCitiesEdit(EditWorkshopViewModel viewModel, CancellationToken ct)
        {
            var cities = await _cityService.GetAllCities(ct);
            viewModel.Cities = cities.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
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
            var result = await _workshopService.SoftDeleteWorkshopAsync(id, userId, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message ?? "خطا در حذف کارگاه";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = result.Message ?? "کارگاه با موفقیت حذف شد";

            return RedirectToAction(nameof(Index));
        }
    }
}