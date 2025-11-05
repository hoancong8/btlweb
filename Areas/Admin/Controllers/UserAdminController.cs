using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.Models;

namespace BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserAdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Danh sách User
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.OrderByDescending(u => u.CreateAt).ToListAsync();
            return View("UserAdminIndex", users);
        }

        // ✅ Form thêm User
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateUser");
        }

        // ✅ Xử lý thêm User
        [HttpPost]
        public async Task<IActionResult> Create(User model, IFormFile? avatar)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Email))
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin." });

            if (avatar != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string path = Path.Combine(folder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await avatar.CopyToAsync(stream);
                model.AvatarUrl = "/uploads/avatars/" + fileName;
            }
            else
            {
                model.AvatarUrl = "/uploads/avatars/default.png";
            }

            model.CreateAt = DateTime.Now;
            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm User thành công!" });
        }

        // ✅ Form sửa User
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return PartialView("_EditUser", user);
        }

        // ✅ Xử lý sửa User
        [HttpPost]
        public async Task<IActionResult> Edit(User model, IFormFile? avatar)
        {
            var existing = await _context.Users.FindAsync(model.UserID);
            if (existing == null) return Json(new { success = false, message = "Không tìm thấy User." });

            existing.UserName = model.UserName;
            existing.FullName = model.FullName;
            existing.Email = model.Email;
            existing.Role = model.Role;
            existing.IsActive = model.IsActive;

            if (avatar != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string path = Path.Combine(folder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await avatar.CopyToAsync(stream);
                existing.AvatarUrl = "/uploads/avatars/" + fileName;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật User thành công!" });
        }

        // ✅ Xóa User
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return Json(new { success = false, message = "Không tìm thấy User." });

                // Xóa avatar nếu có
                if (!string.IsNullOrEmpty(user.AvatarUrl) && !user.AvatarUrl.EndsWith("default.png"))
                {
                    string filePath = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã xóa User thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, bool isActive)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return Json(new { success = false, message = "Không tìm thấy User." });

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"User đã {(isActive ? "kích hoạt" : "tắt")} thành công!" });
        }

    }
}
