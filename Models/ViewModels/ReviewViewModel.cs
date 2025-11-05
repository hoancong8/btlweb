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
        [Required(ErrorMessage = "Vui lòng nhập mã VerifyKey.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "VerifyKey phải có đúng 10 ký tự.")]
        [RegularExpression(@"^[0-9][A-Za-z0-9]{9}$", ErrorMessage = "VerifyKey phải bắt đầu bằng số và có 10 ký tự.")]
        public string VerifyKey { get; set; }
        public List<Service> Services { get; set; }  // Danh sách dịch vụ để chọn
    }
}
