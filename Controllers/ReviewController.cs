using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.Models;
using BTL.Models.ViewModels;

namespace BTL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // Toggle Like (Bấm là Like, bấm lại là Bỏ Like)
        [HttpPost]
        public IActionResult ToggleLike(int reviewId, int userId)
        {
            var existing = _context.Likes
                .FirstOrDefault(l => l.ReviewID == reviewId && l.UserID == userId);

            if (existing != null)
            {
                _context.Likes.Remove(existing);
            }
            else
            {
                var like = new Like
                {
                    ReviewID = reviewId,
                    UserID = userId,
                    Type = false,
                    CreateAt = DateTime.Now
                };
                _context.Likes.Add(like);
            }

            _context.SaveChanges();

            int count = _context.Likes.Count(l => l.ReviewID == reviewId);

            return Json(new { success = true, likeCount = count });
        }
        public IActionResult Detail(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            var review = _context.Review
                .Include(r => r.User)
                .Include(r => r.RvImages)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .FirstOrDefault(r => r.ReviewID == id);

            if (review == null)
                return NotFound();

            var service = _context.Services
                .FirstOrDefault(s => s.ItemID == review.ItemID);

            if (service == null)
                return NotFound();

            var vm = new ReviewDetailVM
            {
                ReviewID = id,
                UserID = userId,
                UserName = review.User.FullName,
                AvatarUrl = review.User.AvatarUrl ?? "/uploads/avatars/default.png",
                CurrentUserAvatar = _context.Users.Find(userId)?.AvatarUrl ?? "/uploads/avatars/default.png",
                Title = review.Title,
                Content = review.Content,
                CreateAt = review.CreateAt,
                MainImage = review.RvImages.FirstOrDefault()?.ImageUrl ?? "/uploads/reviews/default.png",
                IsLiked = _context.Likes.Any(x => x.ReviewID == id && x.UserID == userId),
                LikeCount = _context.Likes.Count(x => x.ReviewID == id),
                ItemID = review.ItemID,
                ServiceName = service.ItemName,
                ServiceDescription = service.Description,
                ServiceAddress = service.Address,
                ServiceImageUrl = service.ImageUrl,
                Comments = review.Comments
                    .OrderByDescending(c => c.CreateAt)
                    .Select(c => new CommentVM
                    {
                        UserName = c.User.FullName,
                        Avatar = c.User.AvatarUrl,
                        Text = c.CommentText,
                        Time = c.CreateAt
                    }).ToList()
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
public IActionResult AddComment(int reviewId, string content)
{
    int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

    if (userId == 0)
        return Json(new { success = false, message = "Bạn cần đăng nhập để bình luận." });

    var comment = new Comment
    {
        ReviewID = reviewId,
        UserID = userId,
        CommentText = content,
        CreateAt = DateTime.Now
    };

    _context.Comments.Add(comment);
    _context.SaveChanges();

    var user = _context.Users.Find(userId);

    return Json(new
    {
        success = true,
        avatar = user?.AvatarUrl ?? "/uploads/avatars/default.png",
        userName = user?.FullName ?? "Người dùng",
        text = content,
        time = comment.CreateAt.ToString("HH:mm dd/MM")
    });
}

    }
}
