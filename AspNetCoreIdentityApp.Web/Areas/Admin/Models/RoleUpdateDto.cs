using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleUpdateDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Rol ismi boş bırakılamaz.")]
        [Display(Name = "Rol İsmi: ")]
        public string Name { get; set; }
    }
}
