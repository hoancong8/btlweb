using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Tên ðãng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Tên ðãng nhập phải có ít nhất 6 kí tự")]
        public string UserName { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hop le")]
        public string Email { get; set; }

        [Required(ErrorMessage = "M?t khẩu là bắt buộc")]
        public string PasswordHash { get; set; }

        public string? AvatarUrl { get; set; }

        public bool Role { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
