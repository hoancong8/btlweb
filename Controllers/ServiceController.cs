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

        // GET: /Service?keyword=bún bò
        public IActionResult Index(string? keyword)
        {
            var query = _context.Services
                .Include(s => s.Category)
                .Include(s => s.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(s =>
                    EF.Functions.Like(s.ItemName, $"%{keyword}%") ||
                    EF.Functions.Like(s.Description, $"%{keyword}%") ||
                    EF.Functions.Like(s.Address, $"%{keyword}%"));
            }

            var results = query
                .OrderByDescending(s => s.CreateAt)
                .ToList();

            ViewBag.Keyword = keyword;
            return View(results);
        }
    }
}
