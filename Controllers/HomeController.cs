using BTL.Models;
using BTL.Models.ViewModels;
using BTL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BTL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeRepository _reviewRepo;

        public HomeController(ILogger<HomeController> logger, HomeRepository reviewRepo)
        {
            _logger = logger;
            _reviewRepo = reviewRepo;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy UserID của người dùng hiện tại từ session
            int currentUserId = HttpContext.Session.GetInt32("UserID") ?? 0;

            // Gọi ReviewRepository để lấy các review
            var reviews = await _reviewRepo.GetReviewsAsync(currentUserId);

            // Trả về View với danh sách review
            return View(reviews);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
