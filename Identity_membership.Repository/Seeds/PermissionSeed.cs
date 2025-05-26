using Identity_membership.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Identity_membership.Core.Permissions;

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
            await roleManager.AddClaimAsync(role, new("Permission", Permission.Stock.Read));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Order.Read));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Catalog.Read));
        }

        public static async Task AddUpdateAndCreatePermission(RoleManager<UserRoleApp> roleManager, UserRoleApp role)
        {
            await roleManager.AddClaimAsync(role, new("Permission", Permission.Stock.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Order.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Catalog.Create));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Stock.Update));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Order.Update));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Catalog.Update));
        }

        public static async Task AddDeletePermission(RoleManager<UserRoleApp> roleManager, UserRoleApp role)
        {
            await roleManager.AddClaimAsync(role, new("Permission", Permission.Stock.Delete));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Order.Delete));

            await roleManager.AddClaimAsync(role, new("Permission", Permission.Catalog.Delete));
        }

    }
}
