using AspNetCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class UserEditDto
    {
        [Required(ErrorMessage = "Kullanıcı ismi boş olamaz")]
        [Display(Name = "Username:")]
        public string UserName { get; set; } = null!;


        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        [Required(ErrorMessage = "Email boş olamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Telefon boş olamaz")]
        [Display(Name = "Phone:")]
        public string Phone { get; set; } = null!;


        [Display(Name = "Birthdate:")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "City:")]
        public string City { get; set; }


        [Display(Name = "Profile Image:")]
        public IFormFile Picture { get; set; }

        [Display(Name = "Gender:")]
        public Gender? Gender { get; set; }
    }
}
