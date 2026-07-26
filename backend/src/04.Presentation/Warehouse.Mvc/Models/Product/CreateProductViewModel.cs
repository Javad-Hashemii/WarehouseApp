using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Product
{
    public class CreateProductViewModel
    {

        [Required(ErrorMessage = "نام کالا الزامی است.")]
        [StringLength(300,ErrorMessage = "نام کالا نمی‌تواند بیشتر از ۳۰۰ کاراکتر باشد.")]
        [Display(Name = "نام کالا")]
        public string ProductName { get; set; } = string.Empty;

        [Range(1,int.MaxValue,ErrorMessage = "دسته‌بندی را انتخاب کنید.")]
        [Display(Name = "دسته‌بندی")]
        public int CategoryId { get; set; }

        [Display(Name = "زیر‌دسته")]
        public int? SubCategoryId { get; set; }

        [Required(ErrorMessage = "واحد کالا الزامی است.")]
        [StringLength(50,ErrorMessage = "واحد کالا نمی‌تواند بیشتر از ۵۰ کاراکتر باشد.")]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = string.Empty;

        [Display(Name = "کالای مصرفی")]
        public bool IsConsumable { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> SubCategories { get; set; } = new();
    }
}
