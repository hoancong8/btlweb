using System.ComponentModel.DataAnnotations;

namespace BTL.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "Dịch vụ là bắt buộc.")]
        public int SelectedServiceID { get; set; }  // ID dịch vụ được chọn
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public List<Service> Services { get; set; }  // Danh sách dịch vụ để chọn
    }
}
