using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using System.Linq;

namespace BTL.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Inject DbContext qua constructor
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                // Lấy danh sách lỗi chi tiết
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Error"] = "Dữ liệu không hợp lệ: " + string.Join(", ", errors);
                return View(model);
            }

            var existEmail = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            var existUser = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);

            if (existEmail != null)
            {
                TempData["Error"] = "Email đã tồn tại!";
                return View(model);
            }

            if (existUser != null)
            {
                TempData["Error"] = "Tên đăng nhập đã tồn tại!";
                return View(model);
            }

            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
            model.CreateAt = DateTime.Now;
            model.Role = true;
            model.IsActive = true;

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login", "Account");
        }


        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }
    }
}
