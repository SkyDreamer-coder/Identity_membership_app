using Identity_membership.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity_membership.Service.Services
{
    public interface IMemberService
    {
        Task<UserVM> GetUserVMByUserNameAsync(string userName);

        Task LogoutAsync();

        Task<bool> CheckPasswordAsync(string userName, string password);

        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string exPassword, string newPassword);

        Task<UserEditVM> GetUserEditVMAsync(string userName);

        SelectList GetGenderSelectList();

        Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditVM req, string userName);

        List<ClaimVM> GetClaims(ClaimsPrincipal principal);
    }
}
