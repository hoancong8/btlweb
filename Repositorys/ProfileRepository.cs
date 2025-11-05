using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BTL.Repositories
{
    public class ProfileRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProfileRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Lấy thông tin user
        public Task<User?> GetUserByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
        }

        // Cập nhật thông tin user
        public async Task UpdateUserAsync(User user, User model, IFormFile? avatar)
        {
            user.FullName = model.FullName;
            user.Email = model.Email;

            // Nếu đổi mật khẩu
            if (!string.IsNullOrEmpty(model.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
            }

            // Nếu upload avatar mới
            if (avatar != null && avatar.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads/avatars");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = $"{Guid.NewGuid()}_{avatar.FileName}";
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    avatar.CopyTo(stream);

                user.AvatarUrl = "/uploads/avatars/" + fileName;
            }

            await _context.SaveChangesAsync();
        }

        // Lấy review + thống kê profile
        public async Task<ProfileViewModel> GetProfileAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return null;

            var reviews = await (
                from r in _context.Review
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
                }
            )
            .Take(8)
            .ToListAsync();

            return new ProfileViewModel
            {
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                JoinDate = user.CreateAt,
                TotalReviews = await _context.Review.CountAsync(r => r.UserID == id),
                TotalComments = await _context.Comments.CountAsync(c => c.UserID == id),
                TotalLikes = await _context.Likes.CountAsync(l => l.UserID == id && l.Type == true),
                Reviews = reviews
            };
        }
    }
}
