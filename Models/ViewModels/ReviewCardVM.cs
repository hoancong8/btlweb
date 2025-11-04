namespace BTL.Models.ViewModels
{
    public class ReviewCardVM
    {
        public int ReviewID { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public DateTime CreateAt { get; set; }

        public int LikeCount { get; set; }   // ✅ tổng like
        public bool IsLiked { get; set; }    // ✅ user đã like chưa
     
    }
   
}
