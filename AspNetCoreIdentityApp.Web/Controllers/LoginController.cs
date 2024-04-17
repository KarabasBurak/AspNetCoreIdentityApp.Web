using AspNetCoreIdentityApp.Web.DTOs;
using AspNetCoreIdentityApp.Web.Extenisons;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto, string returnUrl=null)
        {
            // returnUrl; kullanıcı gitmek istediği sayfaya ulaşmak için Login olması gerekiyorsa önce login olacak sonra otomatik olarak o sayfaya yönlendirilecek. Kullanıcı için pratik olacak
            // Bu kullanım aslında "if" bloğunu temsil ediyor. returnUrl boş ise Url.Action("Index","Home") kısmı çalışacak. Ama returnUrl boş değilse ise returnUrl kendi değeri çalışacak

            returnUrl = returnUrl ?? Url.Action("Privacy", "Home"); 
            var hasUser=await _userManager.FindByEmailAsync(loginDto.Email); // Kullanıcının girdiği emaile göre AppUser tablosunda kişiyi bulduk hasUser nesnesine atadık

            if(hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre Bilgilerini Kontrol Ediniz");
                return View();
            }
            // Kullanıcıdan gelen hasUser, Password, RememberMe(CheckBox tıklamışsa), LockOut bilgilerini result nesnesine atıyoruz.
            var loginResult = await _signInManager.PasswordSignInAsync(hasUser,loginDto.Password,loginDto.RememberMe,true);

            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Dakika sonra tekrar deneyiniz" });
                return View();
            }

            if (loginResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            ModelState.AddModelErrorList(new List<string>() { $"Email veya Şifre Yanlış",$"Başarısız Giriş Sayısı: {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
            

            return View();

        }
    }
}
