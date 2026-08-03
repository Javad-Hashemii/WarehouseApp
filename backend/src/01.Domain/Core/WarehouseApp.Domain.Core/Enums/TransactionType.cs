using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Enums
{
    public enum TransactionType
    {

        StockIn = 0,
        StockOut = 1,
        AdjustmentIncrease = 2,
        AdjustmentDecrease = 3
    }
}
