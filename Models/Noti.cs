using System;
using System.Collections.Generic;
namespace BTL.Models
{
    public class User
    {
        public int NotiID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        public User? User { get; set; }
    }
}