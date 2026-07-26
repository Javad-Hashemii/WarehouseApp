using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Request
{
    public class AddRequestItemViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "کالا را انتخاب کنید.")]
        [Display(Name = "کالا")]
        public int ProductId { get; set; }


        [Required(ErrorMessage = "مقدار الزامی است.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد.")]
        [Display(Name = "مقدار")]
        public decimal QuantityRequested { get; set; }


        public List<SelectListItem> Products { get; set; } = new();
    }
}
