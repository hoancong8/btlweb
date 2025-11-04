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

        [Required(ErrorMessage = "Tên ðãng nh?p là bat buoc")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Tên ðãng nhap phai có ít nhat 6 ki tu")]
        public string UserName { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bat buoc")]
        [EmailAddress(ErrorMessage = "Email không hop le")]
        public string Email { get; set; }

        [Required(ErrorMessage = "M?t kh?u là bat buoc")]
        public string PasswordHash { get; set; }

        public string? AvatarUrl { get; set; }

        public bool Role { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
