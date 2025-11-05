$(document).ready(function () {
    function showToast(message, type = "success") {
        // giống service-admin.js
    }

    $("#btnAddUser").click(function () {
        $.get("/Admin/UserAdmin/Create", function (res) {
            $("#userModal .modal-content").html(res);
            $("#userModal").modal("show");
        }).fail(() => showToast("Không thể tải form thêm mới!", "error"));
    });

    $(document).on("click", ".btn-edit", function () {
        const id = $(this).data("id");
        $.get(`/Admin/UserAdmin/Edit/${id}`, function (res) {
            $("#userModal .modal-content").html(res);
            $("#userModal").modal("show");
        }).fail(() => showToast("Không thể tải form chỉnh sửa!", "error"));
    });

    $(document).on("click", ".btn-delete", function () {
        const id = $(this).data("id");
        if (!confirm("Bạn có chắc chắn muốn xóa user này?")) return;
        $.post(`/Admin/UserAdmin/Delete/${id}`, function (res) {
            showToast(res.message, res.success ? "success" : "error");
            if (res.success) {
                $(`.btn-delete[data-id='${id}']`).closest("tr").fadeOut(500, () => $(this).remove());
            }
        });
    });
});
