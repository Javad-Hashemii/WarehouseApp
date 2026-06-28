using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Mvc.Controllers.Warehouse
{
    public class WarehousesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
