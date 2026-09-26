using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Enums
{
    public enum PurchaseRequestStatus
    {
        PendingCentralOffice = 1,
        RejectedByCentralOffice = 2,
        PendingProcurement = 3,
        PendingWarehouseReceipt = 4,
        Completed = 5,
        Cancelled = 6
    }
}
