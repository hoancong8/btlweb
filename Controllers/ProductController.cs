using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
