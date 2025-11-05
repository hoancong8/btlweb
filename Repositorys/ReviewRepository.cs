using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BTL.Repositories
{
    public class ReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ LIKE / UNLIKE
        public async Task<int> ToggleLikeAsync(int reviewId, int userId)
        {
            var existing = await _context.Likes
                .FirstOrDefaultAsync(l => l.ReviewID == reviewId && l.UserID == userId);

            if (existing != null)
                _context.Likes.Remove(existing);
            else
                _context.Likes.Add(new Like
                {
                    ReviewID = reviewId,
                    UserID = userId,
                    Type = false,
                    CreateAt = DateTime.Now
                });

            await _context.SaveChangesAsync();

            return await _context.Likes.CountAsync(l => l.ReviewID == reviewId);
        }

        // ✅ Lấy chi tiết review
        public async Task<ReviewDetailVM?> GetReviewDetailAsync(int reviewId, int userId)
        {
            var review = await _context.Review
                .Include(r => r.User)
                .Include(r => r.RvImages)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.ReviewID == reviewId);

            if (review == null)
                return null;

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.ItemID == review.ItemID);

            if (service == null)
                return null;

            return new ReviewDetailVM
            {
                ReviewID = reviewId,
                UserID = userId,
                UserName = review.User.FullName,
                AvatarUrl = review.User.AvatarUrl ?? "/uploads/avatars/default.png",
                CurrentUserAvatar = _context.Users.Find(userId)?.AvatarUrl ?? "/uploads/avatars/default.png",
                Title = review.Title,
                Content = review.Content,
                CreateAt = review.CreateAt,
                MainImage = review.RvImages.FirstOrDefault()?.ImageUrl ?? "/uploads/reviews/default.png",
                IsLiked = _context.Likes.Any(x => x.ReviewID == reviewId && x.UserID == userId),
                LikeCount = _context.Likes.Count(x => x.ReviewID == reviewId),
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
        }

        // ✅ Thêm bình luận
        public async Task<CommentVM?> AddCommentAsync(int reviewId, int userId, string content)
        {
            var comment = new Comment
            {
                ReviewID = reviewId,
                UserID = userId,
                CommentText = content,
                CreateAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);

            return new CommentVM
            {
                UserName = user?.FullName ?? "Người dùng",
                Avatar = user?.AvatarUrl ?? "/uploads/avatars/default.png",
                Text = content,
                Time = comment.CreateAt
            };
        }
    }
}
