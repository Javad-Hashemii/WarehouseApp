using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Warehouse.Mvc.Models.Purchase;
using Warehouse.Mvc.Models.Request;
using WarehouseApp.Application.Services;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.PurchaseRequest;
using WarehouseApp.Domain.Core.Dtos.Request;

namespace Warehouse.Mvc.Controllers.Purchase
{
    [Authorize]
    public class PurchaseController(IPurchaseRequestService purchaseRequestService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await purchaseRequestService.GetPurchaseRequestsForCurrentUserAsync(ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<GetAllPurchaseRequestsDto>());
            }

            return View(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = Roles.CentralManager)]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await purchaseRequestService
                .GetPurchaseRequestDetailForCentralManagerAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var request = result.Data;

            var viewModel = new ApprovePurchaseRequestViewModel
            {
                PurchaseRequestId = request.Id,
                PurchaseRequestNumber = request.PurchaseRequestNumber,
                WorkshopName = request.WorkshopName,
                Status = request.Status,

                Items = request.Items.Select(i => new ApprovePurchaseRequestItemViewModel
                {
                    PurchaseRequestItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Unit = i.Unit,
                    QuantityRequested = i.QuantityRequested,
                    QuantityApproved = i.QuantityApproved
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.CentralManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve (ApprovePurchaseRequestViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "اطلاعات وارد شده نامعتبر است";
                return RedirectToAction(nameof(Details), new { id = viewModel.PurchaseRequestId });
            }

            var dto = new ApprovePurchaseRequestDto
            {
                PurchaseRequestId = viewModel.PurchaseRequestId,
                Items = viewModel.Items.Select(i => new ApprovePurchaseRequestItemDto
                {
                    PurchaseRequestItemId = i.PurchaseRequestItemId,
                    QuantityApproved = i.QuantityApproved
                }).ToList()
            };

            var result = await purchaseRequestService.ApprovePurchaseRequestAsync(dto, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Details), new { id = viewModel.PurchaseRequestId });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = Roles.CentralManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id, CancellationToken ct)
        {
            var result = await purchaseRequestService.DenyPurchaseRequestAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = Roles.ProcurementOfficer)]
        public async Task<IActionResult> ProcessPurchase(int id, CancellationToken ct)
        {
            var result = await purchaseRequestService
                .GetPurchaseRequestDetailForProcurementAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var request = result.Data;

            var viewModel = new ProcessPurchaseViewModel
            {
                PurchaseRequestId = request.Id,
                PurchaseRequestNumber = request.PurchaseRequestNumber,
                WorkshopName = request.WorkshopName,

                Supplier = request.Supplier,
                PurchaseDate = request.PurchaseDate ?? DateTime.Today,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate,
                Notes = request.Notes,

                Items = request.Items
                    .Where(i => i.QuantityApproved > 0)
                    .Select(i => new ProcessPurchaseItemViewModel
                    {
                        PurchaseRequestItemId = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Unit = i.Unit,
                        QuantityApproved = i.QuantityApproved,
                        QuantityPurchased = i.QuantityToPurchase
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ProcurementOfficer)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPurchase(ProcessPurchaseViewModel viewModel,CancellationToken ct)
        {

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dto = new ProcessPurchaseDto
            {
                PurchaseRequestId = viewModel.PurchaseRequestId,
                Supplier = viewModel.Supplier!,
                PurchaseDate = viewModel.PurchaseDate!.Value,
                ExpectedDeliveryDate = viewModel.ExpectedDeliveryDate!.Value,
                Notes = viewModel.Notes,

                Items = viewModel.Items.Select(i => new ProcessPurchaseItemDto
                {
                    PurchaseRequestItemId = i.PurchaseRequestItemId,
                    QuantityPurchased = i.QuantityPurchased
                }).ToList()
            };

            var result = await purchaseRequestService.ProcessPurchaseAsync(dto, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(
                    nameof(ProcessPurchase),
                    new { id = viewModel.PurchaseRequestId });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = Roles.WarehouseManager)]
        public async Task<IActionResult> Receive(int id, CancellationToken ct)
        {
            var result = await purchaseRequestService.GetPurchaseRequestDetailForWarehouseReceiptAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var request = result.Data;

            var viewModel = new ConfirmPurchaseReceiptViewModel
            {
                PurchaseRequestId = request.Id,
                PurchaseRequestNumber = request.PurchaseRequestNumber,
                WorkshopName = request.WorkshopName,

                Items = request.Items.Select(i => new ConfirmPurchaseReceiptItemViewModel
                {
                    PurchaseRequestItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Unit = i.Unit,
                    QuantityPurchased = i.QuantityToPurchase,
                    QuantityReceived = i.QuantityReceived,
                    QuantityReceivedNow = 0
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.WarehouseManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(ConfirmPurchaseReceiptViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "اطلاعات وارد شده نامعتبر است";
                return RedirectToAction(nameof(Receive), new { id = model.PurchaseRequestId });
            }

            var dto = new ConfirmPurchaseReceiptDto
            {
                PurchaseRequestId = model.PurchaseRequestId,

                Items = model.Items.Select(i => new ConfirmPurchaseReceiptItemDto
                {
                    PurchaseRequestItemId = i.PurchaseRequestItemId,
                    QuantityReceivedNow = i.QuantityReceivedNow
                }).ToList()
            };

            var result = await purchaseRequestService.ConfirmWarehouseReceiptAsync(dto, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Receive), new { id = model.PurchaseRequestId });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
