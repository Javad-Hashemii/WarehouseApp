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
    public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
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
    }
}
