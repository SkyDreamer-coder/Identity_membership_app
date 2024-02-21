using Identity_membership.Core.Models;
using Identity_membership.Core.ViewModels;
using Identity_membership.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity_membership.Service.Services
{
    public class MemberService:IMemberService
    {

        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IFileProvider _fileProvider;

        public MemberService(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<UserVM> GetUserVMByUserNameAsync(string userName)
        {
            var user = (await _userManager.FindByNameAsync(userName))!;
            return new UserVM { Email = user.Email, PhoneNumber = user.PhoneNumber, UserName = user.UserName, PictureUrl = user.Picture };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return await _userManager.CheckPasswordAsync(user!, password);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string exPassword, string newPassword)
        {
            var user = (await _userManager.FindByNameAsync(userName))!;

            var result = await _userManager.ChangePasswordAsync(user, exPassword, newPassword);

            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(user!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user!, newPassword, true, false);

            return (true, null);
        }

        public async Task<UserEditVM> GetUserEditVMAsync(string userName)
        {
            var user = (await _userManager.FindByNameAsync(userName))!;

            return new UserEditVM()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                City = user.City,
                BirtDate = user.BirthDate,
                Gender = user.Gender,
            };
        }

        public SelectList GetGenderSelectList()
        {
           return new SelectList(Enum.GetNames(typeof(Gender)));
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditVM req, string userName)
        {
            var user = (await _userManager.FindByNameAsync(userName))!;

            user.UserName = req.UserName;
            user.Email = req.Email;
            user.PhoneNumber = req.Phone;
            user.BirthDate = req.BirtDate;
            user.City = req.City;
            user.Gender = req.Gender;

            if (req.Picture != null && req.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(req.Picture.FileName)}";

                string newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await req.Picture.CopyToAsync(stream);

                user.Picture = randomFileName;
            }
            else
            {
                user.Picture = null;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return (false, updateResult.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();

            if (req.BirtDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(user, true, new[] { new Claim("birthdate", user.BirthDate!.Value.ToString()) });
            }
            else
            {
                await _signInManager.SignInAsync(user, true);
            }

            return (true, null);
        }

        public List<ClaimVM> GetClaims(ClaimsPrincipal principal)
        {
            return principal.Claims.Select(x => new ClaimVM()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();
        }
    }
}
