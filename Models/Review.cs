using System;
using System.Collections.Generic;
namespace BTL.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Chờ duyệt";

        // Navigation
        public User? User { get; set; }
        public Service? Service { get; set; }
        public ICollection<RvImage>? RvImages { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}