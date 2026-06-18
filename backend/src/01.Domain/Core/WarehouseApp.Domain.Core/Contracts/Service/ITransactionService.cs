using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.InventoryTransaction;
using WarehouseApp.Domain.Core.Dtos.Transaction;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface ITransactionService
    {
        Task<ResultDto<bool>> CreateTransactionAsync(CreateTransactionDto createTransactionDto, CancellationToken ct);
        Task<List<GetAllTransactionsDto>> GetAllTransactionsAsync(CancellationToken ct);

    }
}
