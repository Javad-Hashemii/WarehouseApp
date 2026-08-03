using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Dtos.InventoryTransaction;
using WarehouseApp.Domain.Core.Dtos.Transaction;
using WarehouseApp.Domain.Core.Entities;
using WarehouseApp.Domain.Core.Enums;
using WarehouseApp.Infra.Db.SqlServer.Ef.DbContext;

namespace WarehouseApp.Infra.Data.Repos.Ef.Repositories
{
    public class TransactionRepository(AppDbContext _context) : ITransactionRepository
    {
        public async Task<bool> CreateInventoryTransactionAsync(CreateTransactionDto createTransactionDto, CancellationToken ct)
        {
            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = createTransactionDto.ProductId,
                Quantity = createTransactionDto.Quantity,
                TransactionType = createTransactionDto.TransactionType,
                TransactionDate = DateTime.Now
            };
            _context.InventoryTransactions.Add(inventoryTransaction);
            return await _context.SaveChangesAsync(ct) > 0;

        }

        public async Task<List<GetAllTransactionsDto>> GetAllTransactionsAsync(CancellationToken ct)
        {
            return await _context.InventoryTransactions
                .Select(it => new GetAllTransactionsDto
                {
                    Id = it.Id,
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    TransactionType = it.TransactionType,
                    TransactionDate = it.TransactionDate
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateInventoryTransaction(UpdateTransactionDto updateTransactionDto, CancellationToken ct)
        {
            var result = _context.InventoryTransactions
                .Where(it => it.Id == updateTransactionDto.Id)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(it => it.Quantity, updateTransactionDto.Quantity)
                    .SetProperty(it => it.TransactionType, updateTransactionDto.TransactionType)
                    .SetProperty(it => it.TransactionDate, updateTransactionDto.TransactionDate), ct);
            return await result > 0;
        }
    }
}
