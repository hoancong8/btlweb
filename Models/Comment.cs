using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTL.Models
{
    [Table("tblComment")]
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int ReviewID { get; set; }
        public int UserID { get; set; }

        [Column("Comment")]
        public string CommentText { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public Review? Review { get; set; }
        public User? User { get; set; }
    }
}