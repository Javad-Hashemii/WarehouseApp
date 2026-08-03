using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Request
{
    public class EditRequestItemViewModel
    {

        public int? Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            "0.001",
            "999999999",
            ErrorMessage = "مقدار باید بیشتر از صفر باشد")]
        public decimal QuantityRequested { get; set; }
    }
}
