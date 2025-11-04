using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTL.Models
{
    [Table("tblService")] 
    public class Service
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string? Description { get; set; }

        public int CategoryID { get; set; }

        public int UserID { get; set; }

        public string Address { get; set; }

        public double? AvgRating { get; set; }

        public string? ImageUrl { get; set; } 

        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public Category? Category { get; set; }
        public User? User { get; set; }
    }
}