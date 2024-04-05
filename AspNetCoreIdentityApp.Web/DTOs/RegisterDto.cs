using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class RegisterDto
    {
        public RegisterDto()
        {
            
        }
        public RegisterDto(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }

        [Required(ErrorMessage ="Kullanıcı ismi boş olamaz")]
        [Display(Name ="Username:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email boş olamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon boş olamaz")]
        [Display(Name = "Phone:")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Şifre boş olamaz")]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifrenizi tekrar giriniz")]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword { get; set; }
    }
}
