using Identity_membership.web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<UserApp>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UserApp> manager, UserApp user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordCanNotContainUserName", Description = "Şifre alanı kullanıcı adı içeremez" });
            }

            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
