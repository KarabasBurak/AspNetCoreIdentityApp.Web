using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = principal.Identity as ClaimsIdentity;

            if (identityUser != null && identityUser.IsAuthenticated) // Kullanıcı kimliği kontrolü ve doğrulama
            {
                var currentUser = await _userManager.FindByNameAsync(identityUser.Name);
                if (currentUser != null && currentUser.City != null) // Kullanıcı ve şehir bilgisi kontrolü
                {
                    if (!principal.HasClaim(c => c.Type == "City")) // "City" claim'ini kontrol et ve ekle
                    {
                        Claim cityClaim = new Claim("City", currentUser.City);
                        identityUser.AddClaim(cityClaim);
                        principal.AddIdentity(identityUser); // Eğer ClaimsIdentity güncellenirse, principal'e eklenmeli
                    }
                }
            }
            return principal;

        }
    }
}
