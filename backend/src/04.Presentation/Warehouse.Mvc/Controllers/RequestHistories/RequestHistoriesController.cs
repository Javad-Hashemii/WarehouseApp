using Microsoft.AspNetCore.Mvc;
using WarehouseApp.Application.Services;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;

namespace Warehouse.Mvc.Controllers.RequestHistories
{
    public class RequestHistoriesController(IRequestHistoryService requestHistoryService,IRequestService requestService,ITransactionService transactionService):Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            int? workshopId,
            int? warehouseId,
            int page = 1,
            int pageSize = 10,
            CancellationToken ct = default)
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


        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            CancellationToken ct)
        {
            var result = await requestHistoryService
                .GetDetailsAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions(int requestId,CancellationToken ct)
        {
            var result = await transactionService
                .GetTransactionsForRequestAsync(requestId, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(
                    nameof(Details),
                    new { id = requestId });
            }

            ViewBag.RequestId = requestId;

            return View(result.Data);
        }
    }
}
