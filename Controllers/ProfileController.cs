using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BTL.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        public ProfileController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult ProfileUser(int id = 2)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            Console.WriteLine($"🧩 ID nhận được từ URL: {id}");
            if (user == null)
                return NotFound();

            var reviews = (from r in _context.Review
                           join u in _context.Users on r.UserID equals u.UserID
                           join img in _context.RvImages on r.ReviewID equals img.ReviewID into imgs
                           where r.UserID == id
                           select new ReviewCardVM
                           {
                               ReviewID = r.ReviewID,
                               UserID = u.UserID,
                               UserName = u.FullName,
                               AvatarUrl = u.AvatarUrl,
                               Title = r.Title,
                               ShortContent = r.Content,
                               ImageUrl = imgs.FirstOrDefault().ImageUrl,
                               Rating = r.Rating,
                               CreateAt = r.CreateAt,
                               LikeCount = _context.Likes.Count(l => l.ReviewID == r.ReviewID && l.Type == true)
                           })
               .Take(8)
               .ToList();

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                JoinDate = user.CreateAt,
                TotalReviews = _context.Review.Count(r => r.UserID == id),
                TotalComments = _context.Comments.Count(c => c.UserID == id),
                TotalLikes = _context.Likes.Count(l => l.UserID == id && l.Type == true),
                Reviews = reviews
            };

            return View(model);
        }

    }
}
