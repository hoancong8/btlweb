namespace BTL.Models.ViewModels
{
    public class ReviewDetailVM
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }

        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string CurrentUserAvatar { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string MainImage { get; set; }

        public DateTime CreateAt { get; set; }

        public bool IsLiked { get; set; }
        public int LikeCount { get; set; }

        public List<CommentVM> Comments { get; set; }
    }

    public class CommentVM
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }

}
