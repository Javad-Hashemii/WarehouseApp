using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Mvc.Models.Request;
using Warehouse.Mvc.Services;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.Request;

namespace Warehouse.Mvc.Controllers.Request
{
    [Authorize]
    public class RequestsController(IRequestService requestService,IProductService productService,IWorkshopService workshopService,IRequestDraftService requestDraftService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await requestService
                .GetRequestsForCurrentUserAsync(ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<GetAllRequestsDto>());
            }


            return View(result.Data);
        }

        [Authorize(Roles ="WorkshopEmployee,Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var viewModel = new CreateRequestViewModel();

            await PopulateDropdownsAsync(viewModel, ct);

            return View(viewModel);
        }
        private async Task PopulateDropdownsAsync(CreateRequestViewModel viewModel,CancellationToken ct)
        {
            var workshops = await workshopService.GetWorkshopsAsync(ct);

            viewModel.Workshops = workshops
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = w.Name
                })
                .ToList();


            var products = await productService.GetAllProductsAsync(ct);

            viewModel.Products = products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Name} ({p.Unit})"
                })
                .ToList();
        }

        [HttpGet]
        public async Task<IActionResult> AddItem(CancellationToken ct)
        {
            var products = await productService.GetAllProductsAsync(ct);

            var viewModel = new AddRequestItemViewModel
            {
                Products = products
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Name} ({p.Unit})"
                    })
                    .ToList()
            };

            return View(viewModel);
        }


        [Authorize(Roles = "WorkshopEmployee,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateRequestViewModel viewModel,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }


            var requestDto = new CreateRequestDto
            {
                RequestNumber = viewModel.RequestNumber,
                WorkshopId = viewModel.WorkshopId,
                Items = viewModel.Items
            };


            var result = await requestService
                .CreateRequestAsync(requestDto, ct);


            if (!result.IsSuccess)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                await PopulateDropdownsAsync(viewModel, ct);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}


