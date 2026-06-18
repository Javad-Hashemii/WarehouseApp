using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos.Category;
using WarehouseApp.Domain.Core.Dtos.InventoryTransaction;
using WarehouseApp.Domain.Core.Dtos.Transaction;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Contracts.Repository
{
    public interface ITransactionRepository
    {
        Task<bool> CreateInventoryTransactionAsync(CreateTransactionDto createTransactionDto, CancellationToken ct);
        Task<List<GetAllTransactionsDto>> GetAllTransactionsAsync(CancellationToken ct);
        //get by id warehouse id workshop id
        Task<bool> UpdateInventoryTransaction(UpdateTransactionDto updateTransactionDto, CancellationToken ct);  // should It exist?


    }
}
