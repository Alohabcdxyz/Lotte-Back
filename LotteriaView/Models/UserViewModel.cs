using System.ComponentModel.DataAnnotations;

namespace LotteriaView.Models
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
