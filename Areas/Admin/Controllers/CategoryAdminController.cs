using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.Models;

namespace BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryAdminController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryAdminController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Danh sách danh mục
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            return View("CategoryAdminIndex", categories);
        }

        // ✅ Form thêm danh mục
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateCategory");
        }

        // ✅ Xử lý thêm danh mục
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                return Json(new { success = false, message = "Vui lòng nhập tên danh mục." });

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã thêm danh mục thành công!" });
        }

        // ✅ Form sửa danh mục
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return PartialView("_EditCategory", category);
        }

        // ✅ Xử lý sửa danh mục
        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var existing = await _context.Categories.FindAsync(model.CategoryID);
            if (existing == null) return Json(new { success = false, message = "Không tìm thấy danh mục." });

            existing.CategoryName = model.CategoryName;
            existing.Description = model.Description;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật danh mục thành công!" });
        }

        // ✅ Xóa danh mục
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) return Json(new { success = false, message = "Không tìm thấy danh mục." });

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}
