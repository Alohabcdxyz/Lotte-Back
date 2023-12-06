using System.ComponentModel.DataAnnotations;

namespace LotteriaView.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        public int Role { get; set; }
    }
}
