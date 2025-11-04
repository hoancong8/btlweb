using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTL.Models
{
    [Table("tblLike")]
    public class Like
    {
        [Key]
        public int LikeID { get; set; }
        public bool? Type { get; set; }  // false = Like, true = Dislike
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public Review? Review { get; set; }
        public User? User { get; set; }
    }
}