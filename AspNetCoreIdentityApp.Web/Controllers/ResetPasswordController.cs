using AspNetCoreIdentityApp.Web.DTOs;
using AspNetCoreIdentityApp.Web.Extenisons;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId; //ResetPassword sayfası açılınca linkte userId ve token kısmı görünüyor. Bunları ResetPassword metoduna tempdata ile taşıyoruz.
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if(userId==null || token==null)
            {
                throw new Exception("Bir hata oluştu");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString()!); // Client'dan gelen Id'ye göre DB'den kullanıcı çektik.
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamıştır");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token.ToString()!, resetPasswordDto.Password);
            if(result.Succeeded)
            {
                TempData["SuccessMessage"]="Şifreniz başarılı bir şekilde yenilenmiştir";
                return View();
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            }


            return View();
        }
    }
}
