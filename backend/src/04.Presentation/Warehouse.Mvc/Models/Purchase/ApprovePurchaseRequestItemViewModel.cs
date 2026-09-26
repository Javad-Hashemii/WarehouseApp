using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Purchase
{
    public class ApprovePurchaseRequestItemViewModel
    {
        public int PurchaseRequestItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public decimal QuantityRequested { get; set; }

        [Required(ErrorMessage = "مقدار تایید شده الزامی است")]
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "مقدار تایید شده نامعتبر است")]
        public decimal QuantityApproved { get; set; }
    }
}
