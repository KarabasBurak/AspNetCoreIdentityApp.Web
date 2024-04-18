using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Validations.CustomValidations;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ProgramCsCoding
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";

                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;

                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); // Kullanıcı başarısız giriş yaptığında 3 dakika kilitlenecek. yani giriş yapamayacak
                opts.Lockout.MaxFailedAccessAttempts = 3; // Kullanıcı 3 kez başarısız giriş yaptığında kilitlenecek
            })
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddDefaultTokenProviders();
        }
    }
}

// Program.cs'de bulunan Identity özelleştirmeleri için extension oluşturduk. Program.cs tarafını kirletmemek için extension oluşturmak best practices