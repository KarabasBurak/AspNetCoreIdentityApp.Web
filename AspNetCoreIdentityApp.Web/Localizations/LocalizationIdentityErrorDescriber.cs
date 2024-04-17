using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {

        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code="DuplicateUserName", Description=$"{userName} Kullanıcı İsmi Başka Bir Kullanıcı Tarafından Alınmıştır" };

            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateMail", Description = $"{email} Email Adresi Başka Bir Kullanıcı Tarafından Alınmıştır" };
            //return base.DuplicateEmail(email);
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "DuplicatePassword", Description = "Şifre en az 6 karakter içermelidir" };
            //return base.PasswordTooShort(length);
        }
    }
}
