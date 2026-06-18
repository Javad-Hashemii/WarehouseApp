using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Enums
{
    public enum RequestStatus
    {
        PendingWorkshop,
        PendingDepartment,
        PendingCentral,
        PendingProcurement,
        Fulfilled,
        DeniedByDepartment,
        DeniedByCentral
    }
}
