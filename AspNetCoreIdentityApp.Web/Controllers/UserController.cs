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
        public UserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> GetUserInfo()
        {
            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
            }

            var userDto = new AppUserDto
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl=currentUser.Picture
            };
            return View(userDto);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        public IActionResult PasswordChange()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeDto passwordChangeDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            
            var user=(await _userManager.FindByNameAsync(User.Identity!.Name!))!; // Şifresini değiştirecek kullanıcıyı belirledik.

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, passwordChangeDto.PasswordOld); // belirlenen kullanıcının mevcut şifresini aldık

            if(!checkOldPassword) // mevcut şifrenin doğru/yanlış durumunu kontrol ettik.
            {
                ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış");
                return View();
            }

            var resultChangePassword=await _userManager.ChangePasswordAsync(user,passwordChangeDto.PasswordOld,passwordChangeDto.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, passwordChangeDto.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarılı bir şekilde değiştirilmiştir";


            return View();

        }

        
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList=new SelectList(Enum.GetNames(typeof(Gender)));

            var user = (await _userManager.FindByNameAsync(User.Identity!.Name))!;
            var userDto = new UserEditDto()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Phone=user.PhoneNumber!,
                BirthDate=user.BirthDate,
                City=user.City,
                Gender=user.Gender,
            };

            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditDto userEditDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            user.UserName= userEditDto.UserName;
            user.Email= userEditDto.Email;
            user.BirthDate= userEditDto.BirthDate;
            user.City= userEditDto.City;
            user.Gender= userEditDto.Gender;
            user.PhoneNumber=userEditDto.Phone;

            if(userEditDto.Picture != null && userEditDto.Picture.Length>0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(userEditDto.Picture.FileName)}";

                var newPicturePath=Path.Combine(wwwrootFolder.First(x=>x.Name=="userPictures").PhysicalPath!,randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await userEditDto.Picture.CopyToAsync(stream);

                user.Picture = randomFileName;
            }

            var updateUser=await _userManager.UpdateAsync(user);
            if (!updateUser.Succeeded)
            {
                ModelState.AddModelErrorList(updateUser.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();

            if (userEditDto.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(user, true, new[] { new Claim("birthdate", user.BirthDate!.Value.ToString()) });
            }

            else
            {
                await _signInManager.SignInAsync(user, true);
            }


            TempData["SuccesMessage"] = "Bilgileriniz güncellenmiştir";

            var userDto = new UserEditDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                BirthDate = user.BirthDate,
                City = user.City,
                Gender = user.Gender,
            };

            return View(userDto);
        }

        [HttpGet]
        public IActionResult Claims()
        {
            var userClaimList = User.Claims.Select(x => new ClaimDto()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(userClaimList);
        }

        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }
    }
}
