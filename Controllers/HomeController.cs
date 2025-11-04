using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BTL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Lấy UserID của người dùng hiện tại từ session
        int currentUserId = HttpContext.Session.GetInt32("UserID") ?? 0;

        // Truy vấn dữ liệu từ cơ sở dữ liệu và chuyển thành danh sách các ReviewCardVM
        var reviews = _context.Review
            .Include(r => r.User)
            .Include(r => r.RvImages)
            .OrderByDescending(r => r.CreateAt)
            .Select(r => new ReviewCardVM
            {
                ReviewID = r.ReviewID,
                UserID = r.UserID,  // Thêm UserID vào đối tượng ReviewCardVM
                UserName = r.User.FullName,

                // AvatarUrl (nếu null → avatar default)
                AvatarUrl = string.IsNullOrEmpty(r.User.AvatarUrl)
                                ? "/uploads/avatars/default.png"
                                : r.User.AvatarUrl,

                Title = r.Title,
                ShortContent = r.Content.Length > 80
                                ? r.Content.Substring(0, 80) + "..."
                                : r.Content,

                // Ảnh review (nếu không có → ảnh mặc định)
                ImageUrl = r.RvImages.FirstOrDefault() != null
                                ? r.RvImages.First().ImageUrl
                                : "/uploads/reviews/default.png",

                Rating = r.Rating,
                CreateAt = r.CreateAt,

                // Tổng số Like
                LikeCount = _context.Likes.Count(l => l.ReviewID == r.ReviewID),

                //Người dùng hiện tại đã like chưa?
                IsLiked = _context.Likes.Any(l => l.ReviewID == r.ReviewID && l.UserID == currentUserId)
            })
            .ToList();

        // Trả về View với danh sách review
        return View(reviews);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
