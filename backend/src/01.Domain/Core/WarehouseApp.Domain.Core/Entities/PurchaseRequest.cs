using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class PurchaseRequest:BaseEntity
    {
        public string PurchaseRequestNumber { get; set; }
        public int RequestId { get; set; }
        public int WorkshopId { get; set; }
        public  PurchaseRequestStatus Status { get; set; }

        public string? Notes { get; set; }

        public Request Request { get; set; }
        public Workshop Workshop { get; set; }

        public string? Supplier { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }

        public ICollection<PurchaseRequestItem> Items { get; set; } = new List<PurchaseRequestItem>();
        public ICollection<PurchaseRequestHistory> History { get; set; } = new List<PurchaseRequestHistory>();

    }
}
