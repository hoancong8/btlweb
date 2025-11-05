using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTL.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng username hoặc email
                if (_context.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    return View(user);
                }

                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng");
                    return View(user);
                }

                // Mã hóa mật khẩu
                user.PasswordHash = HashPassword(user.PasswordHash);

                // Lưu vào DB
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Login", "User");
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Xem lỗi trên console
                }
            }
            return View(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // (Tuỳ chọn) Trang đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập đầy đủ thông tin.");
                return View();
            }

            // Hash mật khẩu nhập vào
            string hashedPassword = HashPassword(password);

            // Tìm user trong DB
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == username && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }

            // ✅ Lưu vào session
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Role", user.Role ? "User" : "Admin");

            // ✅ Kiểm tra Role
            if (user.Role == false)  // false = admin
            {
                return RedirectToAction("Index", "ServiceAdmin", new { area = "Admin" });
            }

            // ✅ User bình thường
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Clear();

            // Chuyển về trang chính
            return RedirectToAction("Index", "Home");
        }
    }
}
