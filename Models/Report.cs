using System;
namespace BTL.Models
{
    ppublic class Report
    {
        public int ReportID { get; set; }
        public int UserID { get; set; }
        public int ReviewID { get; set; }
        public string Reason { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Chờ xử lý";

        // Navigation
        public User? User { get; set; }
        public Review? Review { get; set; }
    }
}