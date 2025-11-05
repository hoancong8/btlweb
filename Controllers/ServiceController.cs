using BTL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServiceRepository _repo;

        public ServiceController(ServiceRepository repo)
        {
            _repo = repo;
        }

        // ✅ Trang danh sách dịch vụ
        public async Task<IActionResult> Index(string keyword, int? categoryId)
        {
            var services = await _repo.GetServicesAsync(keyword, categoryId);
            return View(services);
        }
    }
}
