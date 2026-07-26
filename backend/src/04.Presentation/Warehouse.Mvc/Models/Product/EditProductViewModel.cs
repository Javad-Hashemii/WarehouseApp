using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Product
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کالا الزامی است")]
        [Display(Name = "نام کالا")]
        public string Name { get; set; }

        [Required(ErrorMessage = "انتخاب دسته بندی الزامی است")]
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }

        [Display(Name = "زیردسته")]
        public int? SubCategoryId { get; set; }

        [Required(ErrorMessage = "واحد کالا الزامی است")]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = string.Empty;

        [Display(Name = "کالای مصرفی")]
        public bool IsConsumable { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> SubCategories { get; set; } = new();
    }
}
