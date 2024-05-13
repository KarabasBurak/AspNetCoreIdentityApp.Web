using AspNetCoreIdentityApp.Core.DTOs;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result=await _userManager.CreateAsync(new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber=registerDto.Phone,
                
            }, registerDto.Password);


            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi gerçekleşti";
                return RedirectToAction(nameof(RegisterController.SignUp));
            }

            foreach(IdentityError item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
            return View();

            
        }
    }
}
