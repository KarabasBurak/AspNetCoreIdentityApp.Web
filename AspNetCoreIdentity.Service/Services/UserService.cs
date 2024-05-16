using AspNetCoreIdentityApp.Core.DTOs;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AppUserDto> GetUserDtoByUserNameAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var result= new AppUserDto
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
            return result;
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var currentUser = await _userManager.FindByNameAsync(userName); // Şifresini değiştirecek kullanıcıyı belirledik.

            var result = await _userManager.CheckPasswordAsync(currentUser, password); // çektiğin kullanıcının mevcut şifresini kontrol et.
            return result;
        }

        public async Task<(bool,IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var currentUser=await _userManager.FindByNameAsync(userName); // parolası değiştirilecek kişiyi veritabanından bul

            // kullanıcıyı, Eski şifresi ve yeni şifresi parametrelerine göre ChangePasswordAsync metodunu kullanarak güncelle
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword); 

            if (!resultChangePassword.Succeeded) 
            {
                return (false, resultChangePassword.Errors); // Şifre değiştirme işlemi başarısız ise hataları dön
            }

            await _userManager.UpdateSecurityStampAsync(currentUser); // UpdateSecurityStampAsync metodu ile günceli bir şekilde şifreyi değiştir
            await _signInManager.SignOutAsync(); // Çıkış yap
            await _signInManager.PasswordSignInAsync(currentUser, newPassword, true, false); // Yeni şifre ile tekrar giriş yap

            return (true, null);
        }

        public async Task<UserEditDto> GetUserEditDtoAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var userDto = new UserEditDto()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
            };

            return userDto;
        }

        public SelectList GetGenderSelectList()
        {
            return new SelectList(Enum.GetNames(typeof(Gender)));
        }

        public async Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditDto userEditDto, string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName); // Güncelleme yapacak kullanıcıyı userName'e göre çektik.

            currentUser.UserName = userEditDto.UserName; // userEditDto'daki UserName, currentUser'daki (db'deki) UserName'e kaydetme işlemidir.
            currentUser.Email = userEditDto.Email;
            currentUser.BirthDate = userEditDto.BirthDate;
            currentUser.City = userEditDto.City;
            currentUser.Gender = userEditDto.Gender;
            currentUser.PhoneNumber = userEditDto.Phone;


            if (userEditDto.Picture != null && userEditDto.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(userEditDto.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userPictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await userEditDto.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateUser = await _userManager.UpdateAsync(currentUser);

            if (!updateUser.Succeeded)
            {
                return (false, updateUser.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();

            if (userEditDto.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDate!.Value.ToString()) });
            }

            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }

            return (true, null);
        }

        public List<ClaimDto> GetClaims(ClaimsPrincipal claimsPrincipal)
        {
            var userClaimList = claimsPrincipal.Claims.Select(x => new ClaimDto()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return userClaimList;
        }
    }
}
