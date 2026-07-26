using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WarehouseApp.Domain.Core.Dtos.RequestItem;

namespace Warehouse.Mvc.Models.Request
{
    public class CreateRequestViewModel
    {
        [Required(ErrorMessage = "شماره درخواست الزامی است.")]
        [StringLength(100, ErrorMessage = "شماره درخواست نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        [Display(Name = "شماره درخواست")]
        public string RequestNumber { get; set; } = string.Empty;


        [Range(1, int.MaxValue, ErrorMessage = "کارگاه را انتخاب کنید.")]
        [Display(Name = "کارگاه")]
        public int WorkshopId { get; set; }


        public List<SelectListItem> Workshops { get; set; } = new();
        public List<SelectListItem> Products { get; set; } = new();

        public List<CreateRequestItemsDto> Items { get; set; } = new();
    }
}
