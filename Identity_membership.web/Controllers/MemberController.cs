using Identity_membership.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_membership.web.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<UserApp> _signInManager;

        public MemberController(SignInManager<UserApp> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
