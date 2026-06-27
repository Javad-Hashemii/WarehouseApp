using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Mvc.Controllers.Account
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
