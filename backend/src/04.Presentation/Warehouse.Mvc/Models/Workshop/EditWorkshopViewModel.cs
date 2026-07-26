using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Workshop
{
    public class EditWorkshopViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "نام کارگاه الزامی است")]
        [StringLength(200, ErrorMessage = "نام کارگاه نمی‌تواند بیش از 200 کاراکتر باشد")]
        [Display(Name = "نام کارگاه")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شهر را انتخاب کنید")]
        [Display(Name = "شهر")]
        public int CityId { get; set; }

        public List<SelectListItem> Cities { get; set; } = new();
    }
}
