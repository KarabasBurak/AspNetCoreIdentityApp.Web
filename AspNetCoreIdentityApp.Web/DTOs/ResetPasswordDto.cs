using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class ResetPasswordDto
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre boş olamaz")]
        [Display(Name = "New Password:")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre Aynı Değil")]
        [Required(ErrorMessage = "Şifrenizi tekrar giriniz")]
        [Display(Name = "New Password Confirm:")]
        public string ConfirmPassword { get; set; }=null!;
    }
}
