using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
