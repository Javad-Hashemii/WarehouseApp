using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Dtos.PurchaseRequest
{
    public class GetAllPurchaseRequestsDto
    {
        public int Id { get; set; }

        public string PurchaseRequestNumber { get; set; } = null!;

        public int RequestId { get; set; }

        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; } = null!;

        public PurchaseRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
