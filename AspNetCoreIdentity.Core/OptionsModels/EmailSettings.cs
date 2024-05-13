namespace AspNetCoreIdentityApp.Core.OptionsModels
{
    public class EmailSettings
    {
        public string Host { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set;} = null!;
    }
}
// appsettings.json içinde tanımladığımız mail bilgilerini tip güvenli bir sınıf üzerinden okumak için OptionsModels sınıfı oluşturuldu