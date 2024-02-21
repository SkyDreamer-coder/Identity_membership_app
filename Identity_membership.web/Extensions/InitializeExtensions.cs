using Identity_membership.web.CustomValidations;
using Identity_membership.web.Localizations;
using Identity_membership.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Extensions
{
    public static class InitializeExtensions
    {
        public static void AddIdentityExtensions(this IServiceCollection services) 
        {

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(15);
            });

            services.AddIdentity<UserApp, UserRoleApp>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;


                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 4;

            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
