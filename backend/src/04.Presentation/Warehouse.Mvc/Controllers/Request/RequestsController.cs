using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Mvc.Controllers.Request
{
    public class RequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
