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
        public IActionResult Index(string keyword, int? categoryId)
        {
            var query = _context.Services
                .Include(s => s.Category)
                .Include(s => s.User)
                .AsQueryable();

            // Lọc theo TÌM KIẾM (code của bạn)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(s =>
                    EF.Functions.Like(s.ItemName, $"%{keyword}%") ||
                    EF.Functions.Like(s.Description, $"%{keyword}%") ||
                    EF.Functions.Like(s.Address, $"%{keyword}%")
                );
            }

            // Lọc theo DANH MỤC (code từ máy chủ)
            // (Tôi đã đổi tên "id" thành "categoryId" cho dễ hiểu)
            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            // Lấy kết quả cuối cùng
            var services = query.ToList();

            return View(services);
        }
    }
}
