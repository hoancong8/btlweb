using BTL.Helpers;
using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BTL.Repositories
{
    public class HomeRepository
    {
        private readonly AppDbContext _context;

        public HomeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewCardVM>> GetReviewsAsync(int currentUserId)
        {
            var reviews = await _context.Review
                .Include(r => r.User)
                .Include(r => r.RvImages)
                .OrderByDescending(r => r.CreateAt)
                .Select(r => new ReviewCardVM
                {
                    ReviewID = r.ReviewID,
                    UserID = r.UserID,  // Thêm UserID vào đối tượng ReviewCardVM
                    UserName = r.User.FullName,
                    VerifyKey = StringHelper.DecodeBase64(r.VerifyKey),
                    AvatarUrl = string.IsNullOrEmpty(r.User.AvatarUrl)
                                    ? "/uploads/avatars/default.png"
                                    : r.User.AvatarUrl,
                    Title = r.Title,
                    ShortContent = r.Content.Length > 80
                                    ? r.Content.Substring(0, 80) + "..."
                                    : r.Content,
                    ImageUrl = r.RvImages.FirstOrDefault() != null
                                    ? r.RvImages.First().ImageUrl
                                    : "/uploads/reviews/default.png",
                    Rating = r.Rating,
                    CreateAt = r.CreateAt,
                    LikeCount = _context.Likes.Count(l => l.ReviewID == r.ReviewID),
                    IsLiked = _context.Likes.Any(l => l.ReviewID == r.ReviewID && l.UserID == currentUserId)
                })
                .ToListAsync();

            return reviews;
        }
    }
}
