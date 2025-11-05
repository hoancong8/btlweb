using BTL.Models;
using BTL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _repo;

        public UserController(UserRepository repo)
        {
            _repo = repo;
        }

        // ✅ Register GET
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Register POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            // Kiểm tra username tồn tại
            if (await _repo.ExistsUserNameAsync(user.UserName))
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                return View(user);
            }

            // Kiểm tra email tồn tại
            if (await _repo.ExistsEmailAsync(user.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(user);
            }

            // Hash mật khẩu
            user.PasswordHash = _repo.HashPassword(user.PasswordHash);

            // Lưu user
            await _repo.AddUserAsync(user);

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Login", "User");
        }

        // ✅ Login GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View();
            }

            var user = await _repo.LoginAsync(username, password);

            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }

            // ✅ Lưu Session
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Role", user.Role ? "User" : "Admin");

            // ✅ Kiểm tra quyền
            if (user.Role == false) // false = admin
            {
                return RedirectToAction("Index", "ServiceAdmin", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        // ✅ Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ✅ Profile User hiện tại
        public async Task<IActionResult> Profile()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            var user = await _repo.GetUserByIdAsync(userId);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }
    }
}
