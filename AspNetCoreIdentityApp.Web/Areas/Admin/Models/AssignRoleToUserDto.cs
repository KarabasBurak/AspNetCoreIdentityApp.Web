namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class AssignRoleToUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Exist { get; set; }
    }
}
