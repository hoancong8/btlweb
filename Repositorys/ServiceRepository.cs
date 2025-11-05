using BTL.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL.Repositories
{
    public class ServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Lấy danh sách Service theo keyword + category filter
        public async Task<List<Service>> GetServicesAsync(string? keyword, int? categoryId)
        {
            var query = _context.Services
                .Include(s => s.Category)
                .Include(s => s.User)
                .AsQueryable();

            // ✅ Lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(s =>
                    EF.Functions.Like(s.ItemName, $"%{keyword}%") ||
                    EF.Functions.Like(s.Description, $"%{keyword}%") ||
                    EF.Functions.Like(s.Address, $"%{keyword}%")
                );
            }

            // ✅ Lọc theo category
            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryID == categoryId.Value);
            }

            return await query.ToListAsync();
        }
    }
}
