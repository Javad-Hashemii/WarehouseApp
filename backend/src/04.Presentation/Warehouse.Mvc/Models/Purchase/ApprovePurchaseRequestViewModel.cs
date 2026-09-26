using WarehouseApp.Domain.Core.Enums;

namespace Warehouse.Mvc.Models.Purchase
{
    public class ApprovePurchaseRequestViewModel
    {
        public int PurchaseRequestId { get; set; }
        public string PurchaseRequestNumber { get; set; } = null!;
        public string WorkshopName { get; set; } = null!;

        public PurchaseRequestStatus Status { get; set; }
        public int RequestId { get; set; }

        public List<ApprovePurchaseRequestItemViewModel> Items { get; set; } = new();
    }
}
