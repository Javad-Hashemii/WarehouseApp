using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class PurchaseRequestHistory:HistoryBase
    {
        public int PurchaseRequestId { get; set; }

        public PurchaseRequestStatus FromStatus { get; set; }
        public PurchaseRequestStatus ToStatus { get; set; }

        public PurchaseRequest PurchaseRequest { get; set; } = null!;
    }
}
