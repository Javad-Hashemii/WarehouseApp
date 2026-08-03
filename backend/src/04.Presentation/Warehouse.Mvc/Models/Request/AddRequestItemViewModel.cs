using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WarehouseApp.Domain.Core.Dtos.Product;

namespace Warehouse.Mvc.Models.Request
{
    public class AddRequestItemViewModel
    {

        [Display(Name = "جستجوی کالا")]
        public string? Search { get; set; }


        [Display(Name = "دسته‌بندی")]
        public int? CategoryId { get; set; }


        [Display(Name = "زیر دسته")]
        public int? SubCategoryId { get; set; }


        // Selected item

        [Range(1, int.MaxValue, ErrorMessage = "کالا را انتخاب کنید.")]
        [Display(Name = "کالا")]
        public int ProductId { get; set; }


        [Required(ErrorMessage = "مقدار الزامی است.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد.")]
        [Display(Name = "مقدار")]
        public decimal QuantityRequested { get; set; }


        // Dropdowns

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> SubCategories { get; set; } = new();


        // Search result

        public List<GetAllProductsDto> Products { get; set; } = new();


        // Pagination

        public int Page { get; set; } = 1;

        public int? RequestId { get; set; }

        public string ReturnTo { get; set; } = "Create";
    }
}
