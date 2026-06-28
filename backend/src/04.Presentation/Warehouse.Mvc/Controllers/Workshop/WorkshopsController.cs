using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Mvc.Controllers.Workshop
{
    public class WorkshopsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
