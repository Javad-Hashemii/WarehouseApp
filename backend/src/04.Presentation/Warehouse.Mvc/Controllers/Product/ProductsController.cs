using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Mvc.Controllers.Product
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
