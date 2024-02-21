using Identity_membership.web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Seeds
{
    public class PermissionSeed
    {

        public static async Task Seed(RoleManager<UserRoleApp> roleManager)
        {
            var basicRoleVal = await roleManager.RoleExistsAsync("BasicRole");

            var advancedRoleVal = await roleManager.RoleExistsAsync("AdvancedRole");

            var adminRoleVal = await roleManager.RoleExistsAsync("AdminRole");

            if (!basicRoleVal)
            {
                await roleManager.CreateAsync(new UserRoleApp() { Name = "BasicRole" });

                var basicRole = (await roleManager.FindByNameAsync("BasicRole"))!;

                await AddReadPermission(roleManager, basicRole);
            }

            if (!advancedRoleVal)
            {
                await roleManager.CreateAsync(new UserRoleApp() { Name = "AdvancedRole" });

                var advancedRole = (await roleManager.FindByNameAsync("AdvancedRole"))!;

                await AddReadPermission(roleManager, advancedRole);
                await AddUpdateAndCreatePermission(roleManager, advancedRole);
            }

            if (!adminRoleVal)
            {
                await roleManager.CreateAsync(new UserRoleApp() { Name = "AdminRole" });

                var adminRole = (await roleManager.FindByNameAsync("AdminRole"))!;

                await AddReadPermission(roleManager, adminRole);
                await AddUpdateAndCreatePermission(roleManager, adminRole);
                await AddDeletePermission(roleManager, adminRole);
            }

        }

        public static async Task AddReadPermission(RoleManager<UserRoleApp> roleManager, UserRoleApp role)
        {
            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Stock.Read));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Order.Read));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Catalog.Read));
        }

        public static async Task AddUpdateAndCreatePermission(RoleManager<UserRoleApp> roleManager, UserRoleApp role)
        {
            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Stock.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Order.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Catalog.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Stock.Update));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Order.Update));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Catalog.Update));
        }

        public static async Task AddDeletePermission(RoleManager<UserRoleApp> roleManager, UserRoleApp role)
        {
            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Stock.Delete));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Order.Delete));

            await roleManager.AddClaimAsync(role, new("Permission", Permissions.Permission.Catalog.Delete));
        }

    }
}
