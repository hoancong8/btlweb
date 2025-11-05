$(document).ready(function () {
    //like dislike
    $(document).on("click", ".btn-like, .btn-dislike", function () {
        const button = $(this);
        const reviewId = button.data("review-id");
        const type = !button.hasClass("btn-like"); // true = like, false = dislike

        $.ajax({
            url: "/Interaction/ToggleLike", // ✅ khớp với [Route("interaction")] + [HttpPost("like")]
            type: "POST",
            data: { reviewId: reviewId, isLike: type }, // ✅ đổi isLike -> type
            success: function (res) {
                console.log("✅ Response từ server:", res);
                if (res.success) {
                    // Cập nhật lại số lượng
                    $(`#like-count-${reviewId}`).text(res.likeCount);
                    $(`#dislike-count-${reviewId}`).text(res.dislikeCount);

                    // Reset trạng thái nút
                    $(`#review-${reviewId} .btn-like, #review-${reviewId} .btn-dislike`).removeClass("active");

                    if (res.userAction === "like") {
                        $(`#review-${reviewId} .btn-like`).addClass("active");
                    } else if (res.userAction === "dislike") {
                        $(`#review-${reviewId} .btn-dislike`).addClass("active");
                    }
                } else {
                    alert(res.message || "Vui lòng đăng nhập để thực hiện hành động này.");
                }
            },
            error: function (xhr) {
                console.log("❌ AJAX Error:", xhr.responseText);
                alert("Đã xảy ra lỗi khi gửi yêu cầu Like/Dislike.");
            }
        });
    });


    // ======================= GỬI BÌNH LUẬN ============================
    $(document).on("submit", ".comment-form", function (e) {
        e.preventDefault();
        const form = $(this);
        const reviewId = form.data("review-id");
        const content = form.find(".comment-input").val().trim();

        if (content === "") {
            alert("Vui lòng nhập nội dung bình luận!");
            return;
        }

        $.ajax({
            url: "/Interaction/AddComment",
            type: "POST",
            data: { reviewId: reviewId, commentText: content },
            success: function (res) {
                if (res.success) {
                    const commentHtml = `
                        <div class="comment-item border-bottom py-2">
                            <strong>${res.comment.userName}</strong>
                            <small class="text-muted ms-2">${res.comment.createAt}</small>
                            <p class="mb-1">${res.comment.commentText}</p>
                        </div>
                    `;
                    $(`#comments-${reviewId}`).prepend(commentHtml);
                    form.find(".comment-input").val("");
                } else {
                    alert(res.message || "Vui lòng đăng nhập để bình luận.");
                }
            },
            error: function () {
                alert("Lỗi khi gửi bình luận.");
            }
        });
    });

    // ======================= BÁO CÁO ĐÁNH GIÁ ============================
    $(document).on("click", ".btn-report", function () {
        const reviewId = $(this).data("review-id");
        const reason = prompt("Nhập lý do bạn muốn báo cáo bài đánh giá này:");

        if (!reason || reason.trim() === "") return;

        $.ajax({
            url: "/Interaction/ReportReview",
            type: "POST",
            data: { reviewId: reviewId, reason: reason },
            success: function (res) {
                if (res.success) {
                    alert("Cảm ơn bạn! Báo cáo của bạn đã được gửi.");
                } else {
                    alert(res.message || "Vui lòng đăng nhập để gửi báo cáo.");
                }
            },
            error: function () {
                alert("Lỗi khi gửi báo cáo.");
            }
        });
    });

});
