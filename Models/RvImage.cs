using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("tblRvImage")]
    public class RvImage
    {
        [Key] // ✅ Bắt buộc có khóa chính
        public int ImageID { get; set; }

        [Required]
        public int ReviewID { get; set; }

        [ForeignKey("ReviewID")]
        public Review Review { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }
    }
}
