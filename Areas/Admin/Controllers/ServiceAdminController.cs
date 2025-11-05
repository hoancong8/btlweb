using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.Models;
using System.Threading.Tasks;

namespace BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceAdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Danh sách dịch vụ
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.Category)
                .OrderByDescending(s => s.CreateAt)
                .ToListAsync();
            return View("ServiceAdminIndex", services);
        }

        // ✅ Form thêm dịch vụ
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return PartialView("_Create");
        }

        // ✅ Xử lý thêm mới
        [HttpPost]
        public async Task<IActionResult> Create(Service model, IFormFile? image)
        {
            if (string.IsNullOrEmpty(model.ItemName) || string.IsNullOrEmpty(model.Address))
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ tên và địa chỉ." });

            // Lấy user hiện tại
            model.UserID = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (image != null)
            {
                // Đường dẫn mới: wwwroot/uploads/reviews
                string folder = Path.Combine(_env.WebRootPath, "uploads", "reviews");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string path = Path.Combine(folder, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                model.ImageUrl = "/uploads/reviews/" + fileName; // Lưu đường dẫn để hiển thị trên web
            }
            else
            {
                model.ImageUrl = "/uploads/reviews/default.png"; // fallback
            }

            model.CreateAt = DateTime.Now;
            _context.Services.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm dịch vụ thành công!" });
        }

        // ✅ Form sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return PartialView("_Edit", service);
        }

        // ✅ Xử lý sửa
        [HttpPost]
        public async Task<IActionResult> Edit(Service model, IFormFile? image)
        {
            var existing = await _context.Services.FindAsync(model.ItemID);
            if (existing == null)
                return Json(new { success = false, message = "Không tìm thấy dịch vụ cần sửa." });

            if (string.IsNullOrWhiteSpace(model.ItemName))
                return Json(new { success = false, message = "Vui lòng nhập tên dịch vụ." });

            if (string.IsNullOrWhiteSpace(model.Address))
                return Json(new { success = false, message = "Vui lòng nhập địa chỉ dịch vụ." });

            existing.ItemName = model.ItemName;
            existing.Description = model.Description;
            existing.CategoryID = model.CategoryID;
            existing.Address = model.Address;

            if (image != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "reviews");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                existing.ImageUrl = "/uploads/reviews/" + fileName;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật dịch vụ thành công!" });
        }

        // ✅ Form xóa
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.ItemID == id);

            if (service == null) return NotFound();

            return PartialView("_Delete", service);
        }

        // ✅ Xử lý xóa
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                    return Json(new { success = false, message = "Không tìm thấy dịch vụ cần xóa." });

                // Xóa file ảnh vật lý
                if (!string.IsNullOrEmpty(service.ImageUrl) && !service.ImageUrl.EndsWith("default.png"))
                {
                    string filePath = Path.Combine(_env.WebRootPath, service.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                Console.WriteLine("Lỗi DeleteConfirmed: ");
                return Json(new { success = true, message = "Đã xóa dịch vụ thành công!" });
            }
            catch (Exception ex)
            {
                // In lỗi ra console
                Console.WriteLine("Lỗi DeleteConfirmed: " + ex.ToString());

                // Hoặc dùng ILogger nếu có
                //_logger.LogError(ex, "Lỗi xóa dịch vụ");

                return Json(new { success = false, message = "Xảy ra lỗi khi xóa dịch vụ. Chi tiết: " + ex.Message });
            }
        }

    }
}
