using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WarehouseApp.Domain.Core.Entities;

namespace Warehouse.Mvc.Models.Warehouse
{
    public class EditWarehouseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="نام انبار الزامی است")]
        [StringLength(50)]
        [Display(Name="نام انبار")]
        public string Name { get; set; } = string.Empty;

        [Range(1,int.MaxValue,ErrorMessage ="شهر را انتخواب کنید")]
        public int CityId { get; set; }

        public List<SelectListItem> Cities { get; set; } = new();
    }
}
