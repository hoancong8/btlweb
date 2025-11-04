namespace BTL.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime JoinDate { get; set; }

        public int TotalReviews { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }

        public List<ReviewCardVM> Reviews { get; set; }
    }
}
