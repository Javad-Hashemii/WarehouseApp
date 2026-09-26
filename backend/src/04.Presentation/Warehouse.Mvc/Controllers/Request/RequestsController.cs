using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Mvc.Models.Purchase;
using Warehouse.Mvc.Models.Request;
using Warehouse.Mvc.Services;
using WarehouseApp.Domain.Core.Consts;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace Warehouse.Mvc.Controllers.Request
{
    [Authorize]
    public class RequestsController(IRequestService requestService, IProductService productService, ICategoryService categoryService, ISubCategoryService subCategoryService,
        IWorkshopService workshopService, IRequestDraftService requestDraftService, IEditRequestDraftService editRequestDraftService, IPurchaseDecisionDraftService purchaseDecisionDraftService) : Controller
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var viewModel = new CreateRequestViewModel();

            await PopulateDropdownsAsync(viewModel, ct);

            var draft = requestDraftService.GetDraft();

            viewModel.Items = draft.Items
                .Select(i => new CreateRequestItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Unit = i.Unit,
                    Quantity = i.QuantityRequested
                })
                .ToList();
            return View(viewModel);
        }
        private async Task PopulateDropdownsAsync(CreateRequestViewModel viewModel, CancellationToken ct)
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
        public async Task<IActionResult> AddItem(
            string? search,
            int? categoryId,
            int? subCategoryId,
            int page,
            int? requestId,
            string? returnTo,
            CancellationToken ct)
        {
            var viewModel = new AddRequestItemViewModel
            {
                Search = search,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Page = page <= 0 ? 1 : page,
                RequestId = requestId,
                ReturnTo = returnTo ?? "Create"
            };

            await PopulateAddItemDropdownsAsync(viewModel, ct);

            var result = await productService.SearchProductsAsync(
                viewModel.Search,
                viewModel.CategoryId,
                viewModel.SubCategoryId,
                viewModel.Page,
                ct);

            viewModel.Products = result.Items;

            // CHANGED: keep the normalized page returned by pagination
            viewModel.Page = result.Page;

            // CHANGED: send total page count to the view
            viewModel.TotalPages = result.TotalPages;

            return View(viewModel);
        }
        private async Task PopulateAddItemDropdownsAsync(AddRequestItemViewModel viewModel, CancellationToken ct)
        {
            var categories = await categoryService
                .GetCategoriesAsync(ct);


            viewModel.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();


            if (viewModel.CategoryId.HasValue)
            {
                var subCategories =
                    await subCategoryService
                        .GetAllSubCategoriesByCategoryId(
                            viewModel.CategoryId.Value,
                            ct);


                viewModel.SubCategories = subCategories
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToList();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddRequestItemViewModel viewModel,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAddItemDropdownsAsync(viewModel, ct);

                // CHANGED: reload products because POST validation returned the same view
                var result = await productService.SearchProductsAsync(
                    viewModel.Search,
                    viewModel.CategoryId,
                    viewModel.SubCategoryId,
                    viewModel.Page,
                    ct);

                // CHANGED
                viewModel.Products = result.Items;

                // CHANGED
                viewModel.Page = result.Page;

                // CHANGED
                viewModel.TotalPages = result.TotalPages;

                return View(viewModel);
            }

            var product = await productService
                .GetProductDetailAsync(viewModel.ProductId, ct);

            if (viewModel.ReturnTo == "Edit" &&
                viewModel.RequestId.HasValue)
            {
                var editItem = new EditRequestDraftItem
                {
                    RequestItemId = null,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Unit = product.Unit,
                    QuantityRequested = viewModel.QuantityRequested
                };

                editRequestDraftService.AddItem(
                    viewModel.RequestId.Value,
                    editItem);

                return RedirectToAction(
                    nameof(Edit),
                    new { id = viewModel.RequestId.Value });
            }

            if (viewModel.ReturnTo == "PurchaseDecision" &&
                viewModel.RequestId.HasValue)
            {
                var purchaseItem = new PurchaseDecisionDraftItem
                {
                    RequestItemId = null,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Unit = product.Unit,
                    QuantityRequested = 0,
                    QuantityReceived = 0,
                    QuantityShortage = 0,
                    QuantityToPurchase = viewModel.QuantityRequested
                };

                purchaseDecisionDraftService.AddItem(
                    viewModel.RequestId.Value,
                    purchaseItem);

                return RedirectToAction(
                    nameof(PurchaseDecision),
                    new { id = viewModel.RequestId.Value });
            }

            var createItem = new CreateRequestDraftItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Unit = product.Unit,
                QuantityRequested = viewModel.QuantityRequested
            };

            requestDraftService.AddItem(createItem);

            return RedirectToAction(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(int productId)
        {
            requestDraftService.RemoveItem(productId);
            return RedirectToAction(nameof(Create));
        }



        [Authorize(Roles = "WorkshopEmployee,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }

            var draft = requestDraftService.GetDraft();

            var requestDto = new CreateRequestDto
            {
                RequestNumber = viewModel.RequestNumber,
                WorkshopId = viewModel.WorkshopId,
                Items = draft.Items.Select(i => new CreateRequestItemsDto
                {
                    ProductId = i.ProductId,
                    QuantityRequested = i.QuantityRequested
                }).ToList()
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

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await requestService.GetByIdWithDetailsAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id, CancellationToken ct)
        {
            var result = await requestService.ApproveRequestAsync(id, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, CancellationToken ct)
        {
            var result = await requestService.RejectRequestAsync(id, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "WorkshopEmployee,WorkshopManager,CentralManager,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var draft = editRequestDraftService.GetEditRequestDraft(id);

            if (draft is null)
            {
                var result = await requestService.GetByIdWithDetailsAsync(id, ct);

                if (!result.IsSuccess || result.Data is null)
                {
                    TempData["ErrorMessage"] = result.Message;

                    return RedirectToAction(nameof(Index));
                }

                var request = result.Data;

                draft = new EditRequestDraft
                {
                    RequestId = request.Id,
                    RequestNumber = request.RequestNumber,
                    WorkshopId = request.WorkshopId,

                    Items = request.Items
                        .Select(item => new EditRequestDraftItem
                        {
                            RequestItemId = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Unit = item.Unit,
                            QuantityRequested = item.QuantityRequested
                        })
                        .ToList(),

                    DeletedItemsIds = new List<int>()
                };

                editRequestDraftService.SaveDraft(draft);
            }

            var viewModel = new EditRequestViewModel
            {
                RequestId = draft.RequestId,
                RequestNumber = draft.RequestNumber,
                WorkshopId = draft.WorkshopId,

                DeletedItemIds = draft.DeletedItemsIds.ToList(),

                Items = draft.Items
                    .Select(item => new EditRequestItemViewModel
                    {
                        Id = item.RequestItemId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Unit = item.Unit,
                        QuantityRequested = item.QuantityRequested
                    })
                    .ToList()
            };

            await PopulateEditDropdownsAsync(viewModel, ct);

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRequestViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateEditDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }

            var newItems = viewModel.Items
                .Where(item => !item.Id.HasValue)
                .Select(item => new AddRequestItemDto
                {
                    ProductId = item.ProductId,
                    QuantityRequested = item.QuantityRequested
                })
                .ToList();

            var updatedItems = viewModel.Items
                .Where(item => item.Id.HasValue)
                .Select(item => new UpdateRequestItemDto
                {
                    Id = item.Id!.Value,
                    ProductId = item.ProductId,
                    QuantityRequested = item.QuantityRequested
                })
                .ToList();

            var deletedItemIds = viewModel.DeletedItemIds
                .Distinct()
                .ToList();

            var result = await requestService.ModifyRequestItemsAsync(viewModel.RequestId, newItems, updatedItems, deletedItemIds, ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                await PopulateEditDropdownsAsync(viewModel, ct);

                return View(viewModel);
            }

            editRequestDraftService.Clear(viewModel.RequestId);

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Details), new { id = viewModel.RequestId });
        }

        private async Task PopulateEditDropdownsAsync(EditRequestViewModel viewModel, CancellationToken ct)
        {
            var workshops = await workshopService.GetWorkshopsAsync(ct);

            viewModel.Workshops = workshops
                .Select(workshop => new SelectListItem
                {
                    Value = workshop.Id.ToString(),
                    Text = workshop.Name,
                    Selected = workshop.Id == viewModel.WorkshopId
                })
                .ToList();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveEditItem(int requestId, int productId)
        {
            editRequestDraftService.RemoveItem(requestId, productId);

            return RedirectToAction(nameof(Edit), new { id = requestId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelEdit(int requestId)
        {
            editRequestDraftService.Clear(requestId);

            return RedirectToAction(
                nameof(Details),
                new { id = requestId });
        }

        [HttpGet]
        public async Task<IActionResult> ProcessByWarehouse(int id, CancellationToken ct)
        {
            var result = await requestService.GetRequestInformationForWarehouseAsync(id, ct);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        [HttpPost]
        [ActionName("ProcessByWarehouse")]
        [Authorize(Roles = Roles.WarehouseManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmWarehouseProcessing(int id, CancellationToken ct)
        {
            var result = await requestService.ProcessWarehouseRequestAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(ProcessByWarehouse), new { id });
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ReceiveByWorkshop(int id, CancellationToken ct)
        {
            var result = await requestService.GetRequestDetailForWorkshop(id, ct);
            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = Roles.WorkshopManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceiveByWorkshop(ConfirmWorkshopReceiptViewModel model, CancellationToken ct)
        {
            var dto = new ConfirmWorkshopReceiptDto
            {
                RequestId = model.RequestId,
                Items = model.Items.Select(i => new ConfirmWorkshopReceiptItemDto
                {
                    RequestItemId = i.RequestItemId,
                    QuantityReceived = i.QuantityReceived
                }).ToList()
            };

            var result = await requestService.ConfirmWorkshopReceiptAsync(dto, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(ReceiveByWorkshop), new { id = model.RequestId });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = Roles.WorkshopManager)]
        public async Task<IActionResult> PurchaseDecision(int id, CancellationToken ct)
        {
            var result = await requestService.GetRequestForPurchaseDecisionAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var draft = purchaseDecisionDraftService.GetDraft(id);

            if (draft == null)
            {
                draft = new PurchaseDecisionDraft
                {
                    RequestId = id,
                    Items = result.Data.Items
                        .Where(i => i.QuantityShortage > 0)
                        .Select(i => new PurchaseDecisionDraftItem
                        {
                            RequestItemId = i.Id,
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            Unit = i.Unit,
                            QuantityRequested = i.QuantityRequested,
                            QuantityReceived = i.QuantityReceived,
                            QuantityShortage = i.QuantityShortage,
                            QuantityToPurchase = i.QuantityShortage
                        })
                        .ToList()
                };

                purchaseDecisionDraftService.SaveDraft(draft);
            }

            var viewModel = new PurchaseDecisionViewModel
            {
                RequestId = result.Data.Id,
                RequestNumber = result.Data.RequestNumber,

                Items = draft.Items.Select(i => new PurchaseDecisionItemViewModel
                {
                    RequestItemId = i.RequestItemId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Unit = i.Unit,
                    QuantityRequested = i.QuantityRequested,
                    QuantityReceived = i.QuantityReceived,
                    QuantityShortage = i.QuantityShortage,
                    QuantityToPurchase = i.QuantityToPurchase
                }).ToList()
            };

            return View("~/Views/Purchase/PurchaseDecision.cshtml",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePurchaseDecisionItem(int requestId, int productId)
        {
            purchaseDecisionDraftService.RemoveItem(requestId, productId);

            return RedirectToAction(
                nameof(PurchaseDecision),
                new { id = requestId });
        }

        [HttpPost]
        [Authorize(Roles = Roles.WorkshopManager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseDecision(PurchaseDecisionViewModel viewModel, CancellationToken ct)
        {
            var dto = new ConfirmPurchaseDecisionDto
            {
                RequestId = viewModel.RequestId,
                Items = viewModel.Items
                    .Where(i => i.QuantityToPurchase > 0)
                    .Select(i => new ConfirmPurchaseDecisionItemDto
                    {
                        RequestItemId = i.RequestItemId,
                        ProductId = i.ProductId,
                        QuantityToPurchase = i.QuantityToPurchase
                    })
                    .ToList()
            };

            var result = await requestService.ConfirmPurchaseDecisionAsync(dto, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(
                    nameof(PurchaseDecision),
                    new { id = viewModel.RequestId });
            }

            purchaseDecisionDraftService.Clear(viewModel.RequestId);

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> History(string? search,int? workshopId,int? warehouseId,int page = 1,int pageSize = 10,CancellationToken ct = default)
        {
            var result = await requestService.GetRequestsForHistoryAsync(
                search,
                workshopId,
                warehouseId,
                page,
                pageSize,
                ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;

                return View(new PaginatedResult<GetAllRequestsForHistoryDto>());
            }

            return View(result.Data);
        }

    }

}


