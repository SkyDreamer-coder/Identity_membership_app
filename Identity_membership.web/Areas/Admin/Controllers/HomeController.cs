using Identity_membership.web.Areas.Admin.Models;
using Identity_membership.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_membership.web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Master-role-action")]
    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly UserManager<UserApp> _userManager;

        public HomeController(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.AsNoTracking().Select(x => new UserVM()
            {
                Id = x.Id,
                Email = x.Email!,
                Name = x.UserName!
            }).ToListAsync();        

            return View(users);
        }

    }
}
