using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTL.Models
{
    [Table("tblReview")]
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Chờ duyệt";
        [Required(ErrorMessage = "Vui lòng nhập mã VerifyKey.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "VerifyKey phải có đúng 10 ký tự.")]
        [RegularExpression(@"^[0-9][A-Za-z0-9]{9}$", ErrorMessage = "VerifyKey phải bắt đầu bằng số và có 10 ký tự.")]
        public string VerifyKey { get; set; }
        // Navigation
        [ForeignKey("UserID")]
        public User? User { get; set; }
        //public Service? Service { get; set; }
        public ICollection<RvImage>? RvImages { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}