using AspNetCoreIdentityApp.Web.DTOs;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgetPasswordController(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var user= await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Girmiş olduğunuz email adresine sahip kullanıcı bulunamadı");
                return View();
            }

            string passwordResetToken=await _userManager.GeneratePasswordResetTokenAsync(user); //Identity içinde GeneratePasswordResetTokenAsync metodu ile token oluşturduk.Program.cs'de var

            var passwordResetLink=Url.Action("ResetPassword", "ResetPassword", new { userId = user.Id, Token=passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink!, user.Email!);

            TempData["SuccessMessage"] = "Şifre yenileme linki, e-posta adresinize gönderilmiştir.";
            return RedirectToAction("ForgetPassword","ForgetPassword");
            
        }
    }
}
