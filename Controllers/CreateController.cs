using BTL.Helpers;
using BTL.Models;
using BTL.Models.ViewModels;
using BTL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

public class CreateController : Controller
{
    private readonly CreateRepository _reviewRepo;

    // Inject ReviewRepository qua constructor
    public CreateController(CreateRepository reviewRepo)
    {
        _reviewRepo = reviewRepo;
    }

    // GET: Hiển thị form viết đánh giá
    public async Task<IActionResult> Create()
    {
        var model = new ReviewViewModel
        {
            Services = await _reviewRepo.GetAllServicesAsync()
        };
        return View(model);
    }

    // POST: Lưu đánh giá mới
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewViewModel model, IFormFile? imageFile)
    {
        if (HttpContext.Session.GetInt32("UserID") == null)
            return RedirectToAction("Login", "User");

        try
        {
            int userId = (int)HttpContext.Session.GetInt32("UserID");

            var review = new Review
            {
                UserID = userId,
                ItemID = model.SelectedServiceID,
                Title = model.Title,
                Content = model.Content,
                Rating = model.Rating,
                VerifyKey = StringHelper.EncodeBase64(model.VerifyKey), // Mã hóa VerifyKey trước khi lưu
                CreateAt = DateTime.Now,
                Status = "Đã duyệt"
            };

            // Gọi ReviewRepository để thêm đánh giá
            await _reviewRepo.AddReviewAsync(review, imageFile);

            TempData["Success"] = "Đăng tải thành công!";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi khi lưu đánh giá: {ex.Message}");
            ModelState.AddModelError("", "Có lỗi khi lưu đánh giá. Vui lòng thử lại.");
            model.Services = await _reviewRepo.GetAllServicesAsync();
            return View(model);
        }
    }
}
