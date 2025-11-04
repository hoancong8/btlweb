using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị các dịch vụ theo danh mục
        public IActionResult Index(int id)
        {
            // Lấy danh mục và các dịch vụ thuộc danh mục đó
            var category = _context.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            var services = _context.Services
                .Where(s => s.CategoryID == id)
                .Include(s => s.Category)  // Thêm Category vào để hiển thị thông tin danh mục
                .Include(s => s.User)      // Nếu bạn cần hiển thị tên người dùng đã tạo dịch vụ
                .ToList();

            // Truyền tên danh mục vào ViewData
            ViewData["CategoryName"] = category.CategoryName;

            // Truyền danh sách các dịch vụ vào model của view
            return View(services);
        }

    }
}
