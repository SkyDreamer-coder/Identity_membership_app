using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = "DuplicateUserName", Description = $"{userName} başka bir kullanıcı tarafından alınmıştır" };
            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = "DuplicateEmail", Description = $"{email} başka bir kullanıcı tarafından alınmıştır" };
            //return base.DuplicateEmail(email);
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = "PasswordTooShort", Description = $"Şifre en az 6 karakter olmalıdır" };
            //return base.PasswordTooShort(length);
        }
    }
}
