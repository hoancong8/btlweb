using System;
namespace BTL.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public Review? Review { get; set; }
        public User? User { get; set; }
    }
}