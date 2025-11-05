using Microsoft.AspNetCore.Mvc;
using BTL.Models;
using System.Linq;

namespace BTL.Controllers
{
    public class NotiController : Controller
    {
        private readonly AppDbContext _context;

        public NotiController(AppDbContext context)
        {
            _context = context;
        }

        // Trang danh sách thông báo đầy đủ
        public IActionResult Index()
        {
            var notis = _context.Notis
                .OrderByDescending(n => n.CreateAt)
                .ToList();
            return View(notis);
        }

        // API trả về JSON cho dropdown
        [HttpGet]
        public IActionResult GetLatest()
        {
            var notis = _context.Notis
                .OrderByDescending(n => n.CreateAt)
                .Take(5)
                .Select(n => new
                {
                    n.NotiID,
                    n.Message,
                    n.IsRead,
                    CreateAt = n.CreateAt.ToString("HH:mm dd/MM/yyyy")
                })
                .ToList();

            return Json(notis);
        }
    }
}