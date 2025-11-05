using BTL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewRepository _repo;

        public ReviewController(ReviewRepository repo)
        {
            _repo = repo;
        }

        // LIKE / UNLIKE
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int reviewId, int userId)
        {
            int newCount = await _repo.ToggleLikeAsync(reviewId, userId);

            return Json(new { success = true, likeCount = newCount });
        }

        //REVIEW DETAIL
        public async Task<IActionResult> Detail(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            var vm = await _repo.GetReviewDetailAsync(id, userId);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        //ADD COMMENT (AJAX)
        [HttpPost]
        public async Task<IActionResult> AddComment(int reviewId, string content)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (userId == 0)
                return Json(new { success = false, message = "Bạn cần đăng nhập để bình luận." });

            var comment = await _repo.AddCommentAsync(reviewId, userId, content);

            return Json(new
            {
                success = true,
                avatar = comment.Avatar,
                userName = comment.UserName,
                text = comment.Text,
                time = comment.Time.ToString("HH:mm dd/MM")
            });
        }

        // (Nếu bạn muốn tạo review ở đây thì để lại)
        public IActionResult Create()
        {
            return View();
        }
    }
}
