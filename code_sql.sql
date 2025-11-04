-- =============================
-- 1️⃣ BẢNG NGƯỜI DÙNG (tblUser)
-- =============================
CREATE TABLE tblUser (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL UNIQUE,         -- Tên đăng nhập, >= 6 ký tự
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,           -- Mật khẩu mã hóa
    AvatarUrl NVARCHAR(255) NULL,
    Role BIT DEFAULT 1,                            -- 0-Admin, 1-User
    CreateAt DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1                         -- 1-Hoạt động, 0-Bị khóa
);
GO

-- =============================
-- 2️⃣ BẢNG DANH MỤC (tblCategory)
-- =============================
CREATE TABLE tblCategory (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- =============================
-- 3️⃣ BẢNG DỊCH VỤ / SẢN PHẨM (tblService)
-- =============================
CREATE TABLE tblService (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CategoryID INT FOREIGN KEY REFERENCES tblCategory(CategoryID),
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    Address NVARCHAR(255) NOT NULL,
    AvgRating FLOAT NULL,
    CreateAt DATETIME DEFAULT GETDATE()
);
ALTER TABLE tblService ADD ImageUrl NVARCHAR(255);

GO

-- =============================
-- 4️⃣ BẢNG ĐÁNH GIÁ (tblReview)
-- =============================
CREATE TABLE tblReview (
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    ItemID INT FOREIGN KEY REFERENCES tblService(ItemID),
    Title NVARCHAR(150) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    CreateAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT N'Chờ duyệt'       -- 'Chờ duyệt' | 'Đã duyệt' | 'Từ chối'
);
GO

-- =============================
-- 5️⃣ BẢNG ẢNH ĐÁNH GIÁ (tblRvImage)
-- =============================
CREATE TABLE tblRvImage (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ReviewID INT FOREIGN KEY REFERENCES tblReview(ReviewID),
    ImageUrl NVARCHAR(255) NOT NULL
);
GO

-- =============================
-- 6️⃣ BẢNG LƯỢT THÍCH (tblLike)
-- =============================
CREATE TABLE tblLike (
    LikeID INT IDENTITY(1,1) PRIMARY KEY,
    Type BIT NULL,                                 -- 0-Like, 1-Dislike
    ReviewID INT FOREIGN KEY REFERENCES tblReview(ReviewID),
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    CreateAt DATETIME DEFAULT GETDATE()
);
GO

-- =============================
-- 7️⃣ BẢNG BÌNH LUẬN (tblComment)
-- =============================
CREATE TABLE tblComment (
    CommentID INT IDENTITY(1,1) PRIMARY KEY,
    ReviewID INT FOREIGN KEY REFERENCES tblReview(ReviewID),
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    Comment NVARCHAR(MAX) NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE()
);
GO

-- =============================
-- 8️⃣ BẢNG BÁO CÁO (tblReport)
-- =============================
CREATE TABLE tblReport (
    ReportID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    ReviewID INT FOREIGN KEY REFERENCES tblReview(ReviewID),
    Reason NVARCHAR(255) NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT N'Chờ xử lý'       -- 'Chờ xử lý' | 'Đã xử lý' | 'Loại bỏ'
);
GO

-- =============================
-- 9️⃣ BẢNG THÔNG BÁO (tblNoti)
-- =============================
CREATE TABLE tblNoti (
    NotiID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
    Message NVARCHAR(255) NOT NULL,
    IsRead BIT DEFAULT 0,                          -- 0-Chưa đọc, 1-Đã đọc
    CreateAt DATETIME DEFAULT GETDATE()
);
GO


INSERT INTO tblCategory (CategoryName, Description)
VALUES
(N'Thời trang', N'Quần áo'),
(N'Du lịch', N'Địa điểm tham quan'),
(N'Giải trí', N'Hoạt động vui chơi');

INSERT INTO tblService (ItemName, Description, CategoryID, UserID, Address, AvgRating, ImageUrl)
VALUES
(N'Quần áo - quanao8', N'Sản phẩm thời trang quanao8', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanao8.jpg'),

(N'Quần áo - quanaoam1', N'Sản phẩm thời trang quanaoam1', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam1.jpg'),
(N'Quần áo - quanaoam2', N'Sản phẩm thời trang quanaoam2', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam2.jpg'),
(N'Quần áo - quanaoam3', N'Sản phẩm thời trang quanaoam3', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam3.jpg'),
(N'Quần áo - quanaoam4', N'Sản phẩm thời trang quanaoam4', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam4.jpg'),
(N'Quần áo - quanaoam5', N'Sản phẩm thời trang quanaoam5', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam5.jpg'),
(N'Quần áo - quanaoam6', N'Sản phẩm thời trang quanaoam6', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam6.jpg'),
(N'Quần áo - quanaoam7', N'Sản phẩm thời trang quanaoam7', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaoam7.jpg'),

(N'Quần áo - quanaonu1', N'Sản phẩm thời trang quanaonu1', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu1.png'),
(N'Quần áo - quanaonu2', N'Sản phẩm thời trang quanaonu2', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu2.jpg'),
(N'Quần áo - quanaonu3', N'Sản phẩm thời trang quanaonu3', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu3.jpg'),
(N'Quần áo - quanaonu4', N'Sản phẩm thời trang quanaonu4', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu4.jpg'),
(N'Quần áo - quanaonu5', N'Sản phẩm thời trang quanaonu5', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu5.jpg'),
(N'Quần áo - quanaonu6', N'Sản phẩm thời trang quanaonu6', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu6.jpg'),
(N'Quần áo - quanaonu7', N'Sản phẩm thời trang quanaonu7', 1, 3, N'Cửa hàng thời trang', NULL, '/uploads/reviews/quanaonu7.jpg');


-- 1
INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
VALUES
(3, 2, N'Chất liệu tốt, mặc thoải mái', 
 N'Áo quần chất liệu mềm, không bị bí. Mặc thử rất thoáng và form đẹp.', 
 5, N'Đã duyệt');

-- 2
INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
VALUES
(3, 2, N'Mặc rất ổn so với tầm giá', 
 N'Sản phẩm có độ hoàn thiện tốt, giao hàng nhanh. Màu sắc giống trên ảnh.', 
 4, N'Đã duyệt');

-- 3
INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
VALUES
(3, 3, N'Rất phù hợp để đi học và đi chơi', 
 N'Form đẹp, đường may chắc chắn. Vải mát và co giãn tốt.', 
 5, N'Đã duyệt');

-- 4
INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
VALUES
(3, 4, N'Mẫu mã đẹp, đóng gói cẩn thận', 
 N'Hàng nhận đúng như mô tả, đóng gói kỹ. Mặc thử vừa người.', 
 4, N'Đã duyệt');

-- 5
INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
VALUES
(3, 5, N'Vải dày dặn, không bị xù', 
 N'Chất lượng ổn trong tầm giá. Giặt thử không bị phai màu.', 
 5, N'Đã duyệt');

	 -- 6
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 6, N'Vải thoáng mát, dễ chịu',
	 N'Mặc cả ngày vẫn thoải mái, chất liệu không gây bí da. Đáng tiền.', 
	 5, N'Đã duyệt');

	-- 7
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 7, N'Form đẹp, đường may chắc chắn',
	 N'Cắt may cẩn thận, không bị chỉ thừa. Mặc rất ôm dáng.', 
	 4, N'Đã duyệt');

	-- 8
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 8, N'Màu sắc rất trendy',
	 N'Màu ngoài đời đẹp hơn cả trên ảnh, không bị phai hay lem.', 
	 5, N'Đã duyệt');

	-- 9
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 9, N'Chất vải mềm, không nhăn',
	 N'Vải mịn, mặc lên mát. Giặt máy không bị nhăn hay bai dão.', 
	 4, N'Đã duyệt');

	-- 10
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 10, N'Mặc đi làm rất lịch sự',
	 N'Phù hợp để mặc đi làm hoặc đi gặp khách. Nhìn rất thanh lịch.', 
	 5, N'Đã duyệt');

	-- 11
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 11, N'Không bị xù lông sau khi giặt',
	 N'Giặt thử 2 lần vẫn giữ form, không xù, không bai. Chất lượng tuyệt!', 
	 5, N'Đã duyệt');

	-- 12
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 12, N'Đúng kích thước, không bị sai size',
	 N'Đặt size L và nhận đúng form. Mặc vừa, không bị rộng hay chật.', 
	 4, N'Đã duyệt');

	-- 13
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 13, N'Hợp để mặc đi chơi cuối tuần',
	 N'Mặc đi dạo phố hay hẹn hò đều đẹp. Nhiều người hỏi mua link.', 
	 5, N'Đã duyệt');

	-- 14
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 14, N'Đóng gói chắc chắn, giao hàng nhanh',
	 N'Nhận hàng nhanh hơn dự kiến, gói hàng đẹp và cẩn thận.', 
	 4, N'Đã duyệt');

	-- 15
	INSERT INTO tblReview (UserID, ItemID, Title, Content, Rating, Status)
	VALUES
	(3, 15, N'Chất lượng vượt mong đợi',
	 N'Không nghĩ chất vải tốt như vậy trong tầm giá. Rất hài lòng.', 
	 5, N'Đã duyệt');


 INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES
(7, '/uploads/reviews/quanao8.jpg'),
(2, '/uploads/reviews/quanaonam1.jpg'),
(3, '/uploads/reviews/quanaonam7.jpg'),
(4, '/uploads/reviews/quanaonam3.jpg'),
(5, '/uploads/reviews/quanaonam4.jpg');
-- 6
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(6, '/uploads/reviews/quanaonam5.jpg');

-- 7
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(7, '/uploads/reviews/quanaonam6.jpg');

-- 8
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(8, '/uploads/reviews/quanaonam7.jpg');

-- 9
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(9, '/uploads/reviews/quanaonu1.png');

-- 10
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(10, '/uploads/reviews/quanaonu2.jpg');

-- 11
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(11, '/uploads/reviews/quanaonu3.jpg');

-- 12
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(12, '/uploads/reviews/quanaonu4.jpg');

-- 13
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(13, '/uploads/reviews/quanaonu5.jpg');

-- 14
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(14, '/uploads/reviews/quanaonu6.jpg');

-- 15
INSERT INTO tblRvImage (ReviewID, ImageUrl) VALUES 
(15, '/uploads/reviews/quanaonu7.jpg');
