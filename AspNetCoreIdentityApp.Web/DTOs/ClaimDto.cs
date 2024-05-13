namespace AspNetCoreIdentityApp.Web.DTOs
{
    public class ClaimDto
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Issuer { get; set; } = null!;

    }
}
