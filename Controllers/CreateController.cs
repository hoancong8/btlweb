using Microsoft.AspNetCore.Mvc;
using BTL.Models;
using BTL.Models.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BTL.Controllers
{
    public class CreateController : Controller
    {
        private readonly AppDbContext _context;

        public CreateController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Hiển thị form để viết đánh giá
        public IActionResult Create()
        {
            var model = new ReviewViewModel
            {
                Services = _context.Services.ToList() // Lấy danh sách dịch vụ
            };
            return View(model);
        }

        // POST: Lưu đánh giá mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model, IFormFile? imageFile)
        {
            if (true)
            {
                Console.WriteLine($"SelectedServiceID: {model.SelectedServiceID}");
                try
                {
                    // Tạo đối tượng Review mới
                    var review = new Review
                    {
                        UserID = 3, // ID của người dùng đăng nhập (cần điều chỉnh theo cơ chế đăng nhập của bạn)
                        ItemID = model.SelectedServiceID, // ID dịch vụ người dùng chọn
                        Title = model.Title,
                        Content = model.Content,
                        Rating = model.Rating,
                        CreateAt = DateTime.Now,
                        Status = "đã duyệt"
                    };

                    // Nếu có hình ảnh, xử lý và lưu
                    if (imageFile != null)
                    {
                        try
                        {
                            // Lưu hình ảnh vào thư mục wwwroot/uploads/reviews/
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/reviews", imageFile.FileName);
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }
                            // Lưu thông tin ảnh vào bảng RvImage
                            review.RvImages = new List<RvImage>
                            {
                                new RvImage { ImageUrl = "/uploads/reviews/" + imageFile.FileName }
                            };
                        }
                        catch (Exception ex)
                        {
                            // Bắt lỗi nếu không thể lưu hình ảnh
                            Console.WriteLine($"Lỗi khi lưu hình ảnh: {ex.Message}");
                            ModelState.AddModelError("Image", "Có lỗi khi lưu hình ảnh. Vui lòng thử lại.");
                        }
                    }

                    // Lưu đánh giá vào cơ sở dữ liệu
                    _context.Review.Add(review);
                    await _context.SaveChangesAsync();

                    // Chuyển hướng về trang danh sách đánh giá hoặc thông báo thành công
                    return RedirectToAction("Index", "Home"); // Hoặc trang danh sách đánh giá
                }
                catch (Exception ex)
                {
                    // Bắt lỗi khi lưu review vào cơ sở dữ liệu
                    Console.WriteLine($"Lỗi khi lưu đánh giá: {ex.Message}");
                    ModelState.AddModelError("", "Có lỗi khi lưu đánh giá. Vui lòng thử lại.");
                }
            }
            else
            {
                Console.WriteLine($"SelectedServiceID: {model.SelectedServiceID}");
                Console.WriteLine($"title: {model.Title}");
                Console.WriteLine($"conten: {model.Content}");
                Console.WriteLine($"rating: {model.Rating}");
                // In các lỗi validation nếu có
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Lỗi validation: {error.ErrorMessage}");
                }
            }

            // Nếu form không hợp lệ, trả lại trang Create với thông báo lỗi
            model.Services = _context.Services.ToList(); // Lấy lại danh sách dịch vụ
            return View(model);
        }
    }
}
