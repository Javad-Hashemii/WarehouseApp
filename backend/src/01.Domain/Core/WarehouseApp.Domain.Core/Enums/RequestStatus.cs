using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Enums
{
    public enum RequestStatus
    {
        PendingWorkshop,
        PendingWarehouse,
        PendingCentralOffice,
        PendingProcurement,
        Fulfilled,
        RejectByWorkshopManager,
        RejectByCentralOffice,
        Cancelled,
        PendingWorkshopReceipt,
        PendingPurchaseDecision,
        PendingPurchase
    }
}
