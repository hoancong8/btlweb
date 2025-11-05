using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            // Có thể truyền nội dung bằng ViewBag hoặc tạo model tĩnh trong code
            ViewBag.Title = "Giới thiệu HaNoiShare";
            ViewBag.Description = "HaNoiShare là nền tảng chia sẻ dịch vụ, sản phẩm và công nghệ tại Hà Nội. "
                                + "Mục tiêu là kết nối người dùng, doanh nghiệp và cộng đồng thông qua đánh giá, chia sẻ và trải nghiệm thực tế.";

            ViewBag.Team = "Nhóm phát triển: Nhóm 21 – BTL Web 2025";
            ViewBag.ContactEmail = "support@hanoishare.vn";

            return View();
        }
    }
}