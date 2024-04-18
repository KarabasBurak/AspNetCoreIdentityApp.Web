using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class LoginDto
    {
        public LoginDto()
        {
            
        }
        public LoginDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage = "Email boş olamaz")]
        [Display(Name = "Email:")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz")]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
