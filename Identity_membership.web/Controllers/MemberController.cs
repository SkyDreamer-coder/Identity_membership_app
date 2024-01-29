using Identity_membership.web.Extensions;
using Identity_membership.web.Models;
using Identity_membership.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_membership.web.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<UserApp> _signInManager;
        private readonly UserManager<UserApp> _userManager;

        public MemberController(SignInManager<UserApp> signInManager, UserManager<UserApp> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserVM { Email = user!.Email!, PhoneNumber = user.PhoneNumber, UserName = user.UserName! };

            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeVM req)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(user!, req.PasswordEx);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Hatalı Eski Şifre");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user!, req.PasswordEx, req.PasswordNewConfirm);

            if(!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(x=>x.Description).ToList());
                return View();
            }


            await _userManager.UpdateSecurityStampAsync(user!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user!, req.PasswordNewConfirm, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir.";

            return View();
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
