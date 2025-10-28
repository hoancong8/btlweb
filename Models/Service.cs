using System.Collections.Generic;
namespace BTL.Models
{
    public class Service
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public string Address { get; set; }
        public double? AvgRating { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public Category? Category { get; set; }
        public User? User { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}