using Microsoft.AspNetCore.Mvc;
using BTL.Models;

namespace BTL.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Contact
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(model);
                _context.SaveChanges();
                ViewBag.Success = "Cảm ơn bạn! Liên hệ của bạn đã được gửi thành công.";
                ModelState.Clear();
                return View();
            }

            return View(model);
        }

        // Trang quản trị: Danh sách liên hệ
        public IActionResult List()
        {
            var contacts = _context.Contacts
                .OrderByDescending(c => c.CreateAt)
                .ToList();
            return View(contacts);
        }
    }
}