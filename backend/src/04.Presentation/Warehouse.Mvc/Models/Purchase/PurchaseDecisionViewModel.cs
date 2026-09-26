using Microsoft.AspNetCore.Mvc.Rendering;

namespace Warehouse.Mvc.Models.Purchase
{
    public class PurchaseDecisionViewModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; } = null!;

        public List<PurchaseDecisionItemViewModel> Items { get; set; } = new();
    }
}
