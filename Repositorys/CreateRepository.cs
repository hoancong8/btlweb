using BTL.Helpers;
using BTL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BTL.Repositories
{
    public class CreateRepository
    {
        private readonly AppDbContext _context;

        public CreateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task AddReviewAsync(Review review, IFormFile? imageFile)
        {
            // Lưu ảnh nếu có
            if (imageFile != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/reviews");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await imageFile.CopyToAsync(stream);

                review.RvImages = new List<RvImage>
                {
                    new RvImage { ImageUrl = "/uploads/reviews/" + imageFile.FileName }
                };
            }

            // Thêm vào cơ sở dữ liệu
            _context.Review.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}
