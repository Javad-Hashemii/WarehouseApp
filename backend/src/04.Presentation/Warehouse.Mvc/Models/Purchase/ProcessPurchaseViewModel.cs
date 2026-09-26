using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Purchase
{
    public class ProcessPurchaseViewModel
    {
        public int PurchaseRequestId { get; set; }

        public string PurchaseRequestNumber { get; set; } = null!;

        public string WorkshopName { get; set; } = null!;

        [Required(ErrorMessage = "نام تامین‌کننده الزامی است")]
        public string? Supplier { get; set; }

        [Required(ErrorMessage = "تاریخ خرید الزامی است")]
        public DateTime? PurchaseDate { get; set; }

        [Required(ErrorMessage = "تاریخ خرید الزامی است")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        public string? Notes { get; set; }

        public List<ProcessPurchaseItemViewModel> Items { get; set; } = new();
    }
}
