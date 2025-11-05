using BTL.Models;
using BTL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileRepository _repo;

        public ProfileController(ProfileRepository repo)
        {
            _repo = repo;
        }

        // GET: /Profile
        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "User");

            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return View("Profile", user);
        }

        // POST: /Profile
        [HttpPost]
        public async Task<IActionResult> Index(User model, IFormFile AvatarUrl)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "User");

            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            await _repo.UpdateUserAsync(user, model, AvatarUrl);

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Index");
        }

        // GET: /Profile/ProfileUser/{id}
        public async Task<IActionResult> ProfileUser(int id)
        {
            var model = await _repo.GetProfileAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }
    }
}
