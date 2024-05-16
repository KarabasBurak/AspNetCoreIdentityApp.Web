using AspNetCoreIdentityApp.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AspNetCoreIdentity.Service
{
    public interface IUserService
    {
        Task<AppUserDto> GetUserDtoByUserNameAsync(string userName);
        Task LogoutAsync();
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<UserEditDto> GetUserEditDtoAsync(string userName);
        SelectList GetGenderSelectList();
        Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditDto userEditDto, string userName);
        List<ClaimDto> GetClaims(ClaimsPrincipal claimsPrincipal);
    }
}
