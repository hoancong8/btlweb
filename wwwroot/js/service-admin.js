$(document).ready(function () {

    console.log("✅ service-admin.js loaded");

    // ===================== 🧭 Hàm hiển thị toast thông báo =====================
    function showToast(message, type = "success") {
        const toastId = "toast-" + Date.now();
        const bg =
            type === "success"
                ? "bg-success"
                : type === "error"
                ? "bg-danger"
                : "bg-warning text-dark";

        const toastHtml = `
            <div id="${toastId}" 
                 class="toast align-items-center text-white ${bg} border-0 position-fixed bottom-0 end-0 m-3" 
                 role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body fw-semibold">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        $("body").append(toastHtml);
        const toast = new bootstrap.Toast(document.getElementById(toastId), { delay: 3000 });
        toast.show();
        setTimeout(() => $(`#${toastId}`).remove(), 3500);
    }

    // ===================== 🧩 Mở modal thêm dịch vụ =====================
    $("#btnAddService").click(function () {
        $.get("/Admin/ServiceAdmin/Create", function (res) {
            $("#serviceModal .modal-content").html(res);
            $("#serviceModal").modal("show");
        }).fail(() => showToast("Không thể tải form thêm mới!", "error"));
    });

    // ===================== 🧩 Mở modal sửa =====================
    $(document).on("click", ".btn-edit", function () {
        const id = $(this).data("id");
        $.get(`/Admin/ServiceAdmin/Edit/${id}`, function (res) {
            $("#serviceModal .modal-content").html(res);
            $("#serviceModal").modal("show");
        }).fail(() => showToast("Không thể tải form chỉnh sửa!", "error"));
    });

    // ===================== 🧩 Mở modal xóa =====================
    $(document).on("click", ".btn-delete", function () {
        const id = $(this).data("id");
        $.get(`/Admin/ServiceAdmin/Delete/${id}`, function (res) {
            $("#serviceModal .modal-content").html(res);
            $("#serviceModal").modal("show");
        }).fail(() => showToast("Không thể tải form xác nhận xóa!", "error"));
    });

    // ===================== 🧩 Validate form trước khi gửi =====================
    function validateForm(form) {
        let isValid = true;
        form.find("[required]").each(function () {
            if (!$(this).val().trim()) {
                $(this).addClass("is-invalid");
                isValid = false;
            } else {
                $(this).removeClass("is-invalid");
            }
        });
        return isValid;
    }

    // ===================== 🧩 Xử lý submit form Ajax (Create / Edit / Delete) =====================
    $(document).on("submit", "form", function (e) {
        e.preventDefault();
        const form = $(this);
        const formData = new FormData(this);
        const submitBtn = form.find("button[type=submit]");
        const oldText = submitBtn.html();

        // Validate trước khi gửi
        if (!validateForm(form)) {
            showToast("⚠️ Vui lòng điền đầy đủ thông tin bắt buộc!", "warning");
            return;
        }

        // Loading button
        submitBtn.prop("disabled", true).html(`<span class="spinner-border spinner-border-sm"></span> Đang xử lý...`);

        $.ajax({
            url: form.attr("action"),
            type: form.attr("method") || "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    showToast(res.message, "success");

                    // Đóng modal
                    $("#serviceModal").modal("hide");

                    // Nếu là form xóa → xóa hàng khỏi bảng
                    if (form.attr("id") === "formDeleteService") {
                        const deletedId = form.find("input[name='ItemID']").val();
                        const row = $(`.btn-delete[data-id='${deletedId}']`).closest("tr");
                        row.fadeOut(500, () => row.remove());
                    } else {
                        // Nếu là thêm / sửa → reload trang
                        setTimeout(() => location.reload(), 800);
                    }
                } else {
                    showToast(res.message || "Có lỗi xảy ra!", "error");
                }
            },
            error: function (xhr) {
                console.error(xhr.responseText);
                showToast("❌ Lỗi khi gửi yêu cầu đến máy chủ!", "error");
            },
            complete: function () {
                submitBtn.prop("disabled", false).html(oldText);
            }
        });
    });
});
