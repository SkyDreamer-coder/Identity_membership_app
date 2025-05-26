using Identity_membership.web.Extensions;
using Identity_membership.Repository.Models;
using Identity_membership.Core.ViewModels;
using Identity_membership.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using Identity_membership.Service.Services;

namespace Identity_membership.web.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<UserApp> _signInManager;
        private readonly UserManager<UserApp> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberService _memberService;
        // get only
        private string userName => User.Identity!.Name!;

        public MemberController(SignInManager<UserApp> signInManager, UserManager<UserApp> userManager, IFileProvider fileProvider, IMemberService memberService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index() 
        {            

            return View(await _memberService.GetUserVMByUserNameAsync(userName));
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

            if (!await _memberService.CheckPasswordAsync(userName, req.PasswordEx))
            {
                ModelState.AddModelError(string.Empty, "Hatalı Eski Şifre");
                return View();
            }

            var (isSuccess, errors) = await _memberService.ChangePasswordAsync(userName, req.PasswordEx, req.PasswordNewConfirm);

            if(!isSuccess)
            {
                ModelState.AddModelErrorList(errors!);
                return View();
            }

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir.";

            return RedirectToAction("Index", "Member");
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = _memberService.GetGenderSelectList();      

            return View(await _memberService.GetUserEditVMAsync(userName));
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditVM req)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess, errors) = await _memberService.EditUserAsync(req, userName);

            if (!isSuccess) 
            {
                ModelState.AddModelErrorList(errors!);
                return View();
            }
                                                         
            TempData["SuccessMessage"] = "Profil bilgileri başarıyla değiştirilmiştir.";            

            return View(await _memberService.GetUserEditVMAsync(userName));
        }

        public async Task Logout()
        {
           await _memberService.LogoutAsync();
        }

        [HttpGet]
        public IActionResult Claims()
        {
            return View(_memberService.GetClaims(User));
        }

        [Authorize(Policy = "AfyonPolicy")]
        [HttpGet]
        public IActionResult AfyonPage()
        {
            return View();
        }

        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult ExchangePage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        [HttpGet]
        public IActionResult ViolencePage()
        {
            return View();
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticiniz ile görüşün.";

            ViewBag.message = message;

            return View();
        }
    }
}
