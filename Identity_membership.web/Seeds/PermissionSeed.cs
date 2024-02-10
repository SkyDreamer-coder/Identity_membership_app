using Identity_membership.web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Seeds
{
    public class PermissionSeed
    {

        public static async Task Seed(RoleManager<UserRoleApp> roleManager)
        {
            var basicRoleVal = await roleManager.RoleExistsAsync("BasicRole");

            if (!basicRoleVal)
            {
                await roleManager.CreateAsync(new UserRoleApp() { Name = "BasicRole" });

                var basicRole = (await roleManager.FindByNameAsync("BasicRole"))!;

                await roleManager.AddClaimAsync(basicRole, new("Permission", Permissions.Permission.Stock.Read));

                await roleManager.AddClaimAsync(basicRole, new("Permission", Permissions.Permission.Order.Read));

                await roleManager.AddClaimAsync(basicRole, new("Permission", Permissions.Permission.Catalog.Read));
            }
        }
    }
}
