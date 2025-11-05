namespace BTL.Models
{
    public class About
    {
        public int AboutID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
    }
}