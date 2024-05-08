using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class ForgetPasswordDto
    {
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        [Required(ErrorMessage = "Email boş olamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; } = null;
    }
}
