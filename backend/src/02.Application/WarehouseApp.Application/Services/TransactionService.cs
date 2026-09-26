using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.InventoryTransaction;
using WarehouseApp.Domain.Core.Dtos.Transaction;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class TransactionService(ITransactionRepository transactionRepository,IRequestRepository requestRepository,ICurrentUserService currentUserService,IWarehouseRepository warehouseRepository) : ITransactionService
    {
        public async Task<ResultDto<bool>> CreateTransactionAsync(CreateTransactionDto createTransactionDto, CancellationToken ct)
        {
            var result = await transactionRepository.CreateInventoryTransactionAsync(createTransactionDto, ct);
            if(result)
            {
                return ResultDto<bool>.Success("صورت جلسه با موفقیت ایجاد شد", true); // صورت جلسه؟؟
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در ایجاد صورت جلسه", false);
            }

        }

        public async Task<List<GetAllTransactionsDto>> GetAllTransactionsAsync(CancellationToken ct)
        {
            return await transactionRepository.GetAllTransactionsAsync(ct);
        }

        public async Task<ResultDto<List<GetRequestTransactionDto>>> GetTransactionsForRequestAsync(int requestId,CancellationToken ct)
        {
            if (requestId <= 0)
            {
                return ResultDto<List<GetRequestTransactionDto>>
                    .Failure("شناسه درخواست نامعتبر است");
            }

            var request = await requestRepository.GetByIdWithDetailsAsync(requestId, ct);

            if (request == null)
            {
                return ResultDto<List<GetRequestTransactionDto>>
                    .Failure("درخواست یافت نشد");
            }

            switch (currentUserService.Role)
            {
                case "Admin":
                case "CentralManager":
                    break;

                case "WorkshopEmployee":
                case "WorkshopManager":
                    if (currentUserService.WorkshopId != request.WorkshopId)
                    {
                        return ResultDto<List<GetRequestTransactionDto>>.Failure("شما مجاز به مشاهده تراکنش‌های این درخواست نیستید");
                    }
                    break;

                case "WarehouseManager":
                    if (currentUserService.WorkshopId == null)
                    {
                        return ResultDto<List<GetRequestTransactionDto>>.Failure("انبار مرتبط با کاربر مشخص نیست");
                    }

                    var warehouseId =
                        await warehouseRepository.GetWarehouseIdByWorkshopId(currentUserService.WorkshopId.Value,ct);

                    if (warehouseId == null || warehouseId != request.WarehouseId)
                    {
                        return ResultDto<List<GetRequestTransactionDto>>.Failure("شما مجاز به مشاهده تراکنش‌های این درخواست نیستید");
                    }

                    break;

                default:
                    return ResultDto<List<GetRequestTransactionDto>>.Failure("شما مجاز به مشاهده تراکنش‌های این درخواست نیستید");
            }

            var transactions = await transactionRepository.GetTransactionsForRequest(requestId, ct);

            return ResultDto<List<GetRequestTransactionDto>>.Success("تراکنش‌های درخواست با موفقیت دریافت شد", transactions);
        }

    }
}
