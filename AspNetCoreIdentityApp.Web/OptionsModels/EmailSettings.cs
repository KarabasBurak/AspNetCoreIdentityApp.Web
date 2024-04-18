namespace AspNetCoreIdentityApp.Web.OptionsModels
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public string Password { get; set; }
        public string Email { get; set;}
    }
}
// appsettings.json içinde tanımladığımız mail bilgilerini tip güvenli bir sınıf üzerinden okumak için OptionsModels sınıfı oluşturuldu