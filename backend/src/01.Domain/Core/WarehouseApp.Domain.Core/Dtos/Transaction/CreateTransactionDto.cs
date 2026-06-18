using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.Transaction
{
    public class CreateTransactionDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
