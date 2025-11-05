$(document).ready(function () {
    console.log("✅ home.js loaded");

    $("#searchAllBtn").on("click", function () {
        console.log("✅ Search All clicked");

        // Hiển thị phần "Tất cả dịch vụ"
        $("#allServicesSection").removeClass("d-none").hide().fadeIn(400);

        // Đổi trạng thái active
        $(".tab-link").removeClass("active btn-success").addClass("btn-outline-success");
        $(this).removeClass("btn-outline-success").addClass("active btn-success");

        // Gọi Ajax load dữ liệu
        loadAllServices();
    });

    // Click vào các tab category khác
    $(".tab-link[data-category]").on("click", function () {
        const categoryId = $(this).data("category");
        console.log("✅ Category clicked:", categoryId);

        // Hiển thị phần "Tất cả dịch vụ"
        $("#allServicesSection").removeClass("d-none").hide().fadeIn(400);

        // Đổi trạng thái active
        $(".tab-link").removeClass("active btn-success").addClass("btn-outline-success");
        $(this).removeClass("btn-outline-success").addClass("active btn-success");

        // Load dịch vụ theo category
        loadServicesByCategory(categoryId);
    });

    // Hàm lấy tất cả dịch vụ
    function loadAllServices() {
        let container = $("#serviceList");
        container.html(`
            <div class="col-12 text-center py-5">
                <div class="spinner-border text-success" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="mt-2 text-muted">Đang tải dữ liệu...</p>
            </div>
        `);
        $.ajax({
            url: '/Home/GetAllServices',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("✅ Loaded services:", data);
                renderServices(data);
            },
            error: function (xhr, status, error) {
                console.error("❌ Ajax error:", status, error);
                container.html('<div class="col-12"><p class="text-danger text-center mt-3">Lỗi khi tải danh sách dịch vụ!</p></div>');
            }
        });
    }

    // Hàm lấy dịch vụ theo category
    function loadServicesByCategory(categoryId) {
        let container = $("#serviceList");
        container.html(`
            <div class="col-12 text-center py-5">
                <div class="spinner-border text-success" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="mt-2 text-muted">Đang tải dữ liệu...</p>
            </div>
        `);

        $.ajax({
            url: `/Home/GetServicesByCategory?categoryId=${categoryId}`,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("✅ Loaded category services:", data);
                renderServices(data);
            },
            error: function (xhr, status, error) {
                console.error("❌ Ajax error:", status, error);
                container.html('<div class="col-12"><p class="text-danger text-center mt-3">Lỗi khi tải danh sách dịch vụ!</p></div>');
            }
        });
    }

    function renderServices(services) {
        let container = $("#serviceList");
        container.empty();
        // Thêm class row nếu chưa có
        if (!container.hasClass('row')) {
            container.addClass('row g-4');
        }

        if (!services || services.length === 0) {
            container.html('<div class="col-12"><p class="text-muted text-center py-5">Không có dịch vụ nào để hiển thị.</p></div>');
            return;
        }

        services.forEach(s => {
            let html = `
                <div class="col-lg-3 col-md-4 col-sm-6 col-12">
                    <div class="card service-card shadow-sm border-0 rounded-4" 
                         data-id="${s.itemID || s.id || s.serviceId}"
                         style="cursor: pointer;">
                        <img src="/images/Data/${s.imageUrl || 'no-image.jpg'}"
                             alt="${s.itemName}"
                             onerror="this.src='/images/no-image.jpg'">
                        <div class="card-body">
                            <h5 class="card-title fw-bold text-truncate">${s.itemName}</h5>
                            <p class="card-text text-danger mb-2">
                                <i class="bi bi-geo-alt-fill"></i> ${s.address || 'Đang cập nhật'}
                            </p>
                            <p class="card-text text-warning mb-0">
                                <i class="bi bi-star-fill"></i> ${s.avgRating ? s.avgRating.toFixed(1) : 'Chưa có đánh giá'}
                            </p>
                        </div>
                    </div>
                </div>
            `;
            container.append(html);
        });
    }

    // Khi click vào 1 dịch vụ
    $(document).on('click', '.service-card', function () {
        const id = $(this).data('id');
        if (id) {
            window.location.href = `/Service/Details/${id}`;
        }
    });
});