using AspNetCoreIdentity.Service;
using AspNetCoreIdentityApp.Core.DTOs;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Web.Extenisons;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private string userName => User.Identity.Name; // Lamda ile tanımlamamız Get olan metodu olan bir property anlamına geliyor
        private readonly IUserService _userService;
        public UserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _userService = userService;
        }

        public async Task<IActionResult> GetUserInfo()
        {
            var result = await _userService.GetUserDtoByUserNameAsync(userName);
            return View(result);
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        } 

        public IActionResult PasswordChange()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeDto passwordChangeDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var responsePassword = await _userService.CheckPasswordAsync(userName, passwordChangeDto.PasswordOld);

            if (!responsePassword) // mevcut şifrenin doğru/yanlış durumunu kontrol ettik. yanlış ise if içi çalışacak
            {
                ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış");
                return View();
            }

            var (isSuccess, errors)=await _userService.ChangePasswordAsync(userName, passwordChangeDto.PasswordOld, passwordChangeDto.PasswordNew);
            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors);
                return View();
            }

            TempData["SuccessMessage"] = "Şifreniz başarılı bir şekilde değiştirilmiştir";

            return View();

        }


        public async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = _userService.GetGenderSelectList();

            var response = await _userService.GetUserEditDtoAsync(userName);

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditDto userEditDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess,errors) = await _userService.EditUserAsync(userEditDto,userName);

            if(!isSuccess)
            {
                ModelState.AddModelErrorList(errors);
                return View();

            }

            TempData["SuccesMessage"] = "Bilgileriniz güncellenmiştir";

            var response = await _userService.GetUserEditDtoAsync(userName);

            return View(response);
        }

        [HttpGet]
        public IActionResult Claims()
        {
            var response = _userService.GetClaims(User);

            return View(response);
        }

        [Authorize(Policy = "AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }
    }
}
