using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BTL.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        // Navigation
        public ICollection<Service>? Services { get; set; }
    }
}