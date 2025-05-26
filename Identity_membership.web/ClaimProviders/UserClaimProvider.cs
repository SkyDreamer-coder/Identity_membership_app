using Identity_membership.Repository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity_membership.web.ClaimProviders
{
    //[Authorize]
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<UserApp> _userManager;

        public UserClaimProvider(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = principal.Identity as ClaimsIdentity;

            var user = await _userManager.FindByNameAsync(identityUser!.Name!);           

            if(String.IsNullOrEmpty(user!.City)) { return principal; }

            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", user.City);

                identityUser.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
