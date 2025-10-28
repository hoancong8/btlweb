using System;
using System.Collections.Generic;
namespace BTL.Models
{
    public class User
    {
         public int UserID { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? AvatarUrl { get; set; }
        public bool Role { get; set; } = true; // true=User, false=Admin
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Service>? Services { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Noti>? Notis { get; set; }
    }
}