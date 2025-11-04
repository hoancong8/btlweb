namespace BTL.Models
{
    public class Noti
    {
        public int NotiID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation (n?u b?n ð?nh liên k?t ð?n 1 Noti khác)
        public Noti? ParentNoti { get; set; }   // ? ð?i tên
    }
}
