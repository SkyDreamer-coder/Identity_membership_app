using Identity_membership.web.Areas.Admin.Models;
using Identity_membership.web.Extensions;
using Identity_membership.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_membership.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {

        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<UserRoleApp> _roleManager;

        public RoleController(UserManager<UserApp> userManager, RoleManager<UserRoleApp> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var roles = await _roleManager.Roles.AsNoTracking().Select(x => new RoleVM()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();            

            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM req)  
        {

            var result = await _roleManager.CreateAsync(new UserRoleApp() { Name = req.Name });

            if(!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            return RedirectToAction(nameof(RoleController.Index));
        }
    }
}
