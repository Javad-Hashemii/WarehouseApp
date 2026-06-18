using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Enums
{
    public enum TransactionType
    {

        StockIn = 1,
        StockOut = 2,
        AdjustmentIncrease = 3,
        AdjustmentDecrease = 4
    }
}
