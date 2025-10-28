using System.Collections.Generic;
namespace BTL.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        // Navigation
        public ICollection<Service>? Services { get; set; }
    }
}