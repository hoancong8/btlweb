using Microsoft.AspNetCore.Mvc;
using BTL.Models; // Đảm bảo bạn đã import mô hình Review

namespace BTL.Controllers
{
    public class CreateController : Controller
    {
        // Action để mở trang viết đánh giá
        public IActionResult Create()
        {
            return View();
        }

        // Action để xử lý gửi form đăng tải đánh giá (nếu có)
        [HttpPost]
        public IActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                // Xử lý lưu đánh giá vào database (nếu cần)
                // _context.Reviews.Add(review);
                // _context.SaveChanges();

                TempData["Success"] = "Đánh giá đã được gửi thành công!";
                return RedirectToAction("Index", "Home"); // Điều hướng về trang chủ hoặc trang cần thiết
            }

            // Nếu có lỗi, trả về trang tạo lại
            return View(review);
        }
    }
}
