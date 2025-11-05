using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("tblContact")]
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(150)]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung liên hệ")]
        public string Message { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}