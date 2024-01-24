using Identity_membership.web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.CustomValidations
{
    public class UserValidator : IUserValidator<UserApp>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UserApp> manager, UserApp user)
        {
            var errors = new List<IdentityError>();
            // the code that validates if UserName first digit contain numerical type
            var isNumeric = int.TryParse(user.UserName![0].ToString(), out _);

            if (isNumeric) 
            {
                errors.Add(new() { Code = "UserNameCanNotContainFirstLetterDigit", Description = "Kullanıcı Adının ilk karakteri sayısal ifade içeremez" });
            }

            if(errors.Any()) 
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
