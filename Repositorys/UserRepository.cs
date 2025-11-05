using BTL.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BTL.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Hash mật khẩu SHA256
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // ✅ Kiểm tra username tồn tại
        public Task<bool> ExistsUserNameAsync(string username)
        {
            return _context.Users.AnyAsync(u => u.UserName == username);
        }

        // ✅ Kiểm tra email tồn tại
        public Task<bool> ExistsEmailAsync(string email)
        {
            return _context.Users.AnyAsync(u => u.Email == email);
        }

        // ✅ Tạo user mới
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // ✅ Lấy user theo username + password (đăng nhập)
        public async Task<User?> LoginAsync(string username, string password)
        {
            string hashed = HashPassword(password);

            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == hashed);
        }

        // ✅ Lấy user theo ID
        public Task<User?> GetUserByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
        }
    }
}
