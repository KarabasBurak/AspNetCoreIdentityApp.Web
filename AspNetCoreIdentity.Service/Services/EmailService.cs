using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using AspNetCoreIdentityApp.Core.OptionsModels;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient
            {
                Host = _emailSettings.Host,
                DeliveryFormat = SmtpDeliveryFormat.International, // Mail gönderme formatı belirlendi
                UseDefaultCredentials = false, //SMTP protokolünün sağladığı host,password,email bilgilerini değil bizim belirlediğimiz kimlik bilgilerini kullanmak için yazdık ve "false"
                Port = 2525,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password), // Kimlik bilgilerimizi tanımladık
                EnableSsl = true //SSL kullanımını etkinleştirdik
            };

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email,"REPKONYanında");
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
            mailMessage.Body = $@"<h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4> 
                                    <p><a href='{resetPasswordEmailLink}' >Şifre yenileme link</a></p> ";
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
