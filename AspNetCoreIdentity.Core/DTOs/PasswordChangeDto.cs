using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.DTOs
{
    public class PasswordChangeDto
    {
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mevcut şifrenizi giriniz")]
        [Display(Name = "Password:")]
        public string PasswordOld { get; set; }

        [MinLength(6, ErrorMessage = "Yeni Şifreniz en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifrenizi giriniz")]
        [Display(Name = "New Password:")]
        public string PasswordNew { get; set; }

        [MinLength(6, ErrorMessage = "Yeni Şifreniz en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifre Aynı Değil")]
        [Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz")]
        [Display(Name = "New Password Confirm:")]
        public string PasswordConfirm { get; set; }
    }
}
