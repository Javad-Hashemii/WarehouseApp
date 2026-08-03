using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Request
{
    public class EditRequestViewModel
    {
        public int RequestId { get; set; }

        [Required(ErrorMessage = "شماره درخواست الزامی است")]
        public string RequestNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "کارگاه را انتخاب کنید")]
        public int WorkshopId { get; set; }

        public List<SelectListItem> Workshops { get; set; } = new();

        public List<EditRequestItemViewModel> Items { get; set; } = new();

        public List<int> DeletedItemIds { get; set; } = new();

    }

    }
