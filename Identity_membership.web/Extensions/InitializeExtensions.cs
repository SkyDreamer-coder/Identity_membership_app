using Identity_membership.web.Models;

namespace Identity_membership.web.Extensions
{
    public static class InitializeExtensions
    {
        public static void AddIdentityExtensions(this IServiceCollection services) 
        {
            services.AddIdentity<UserApp, UserRoleApp>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
