using Identity_membership.web.Extensions;
using Identity_membership.web.Models;
using Identity_membership.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace Identity_membership.web.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<UserApp> _signInManager;
        private readonly UserManager<UserApp> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<UserApp> signInManager, UserManager<UserApp> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index() 
        {
            var user = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            var userViewModel = new UserVM { Email = user.Email, PhoneNumber = user.PhoneNumber, UserName = user.UserName, PictureUrl = user.Picture };

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
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }


            await _userManager.UpdateSecurityStampAsync(user!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user!, req.PasswordNewConfirm, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir.";

            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var user = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var userEditViewModel = new UserEditVM()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                City = user.City,
                BirtDate = user.BirthDate,
                Gender = user.Gender,
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditVM req)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            user.UserName = req.UserName;
            user.Email = req.Email;
            user.PhoneNumber = req.Phone; 
            user.BirthDate = req.BirtDate;
            user.City = req.City;
            user.Gender = req.Gender;    

            if (req.Picture != null && req.Picture.Length > 0)  
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(req.Picture.FileName)}";

                string newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await req.Picture.CopyToAsync(stream);

                user.Picture = randomFileName;
            }
            else
            {
                user.Picture = null;
            }

            var updateResult =  await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded) 
            {
                ModelState.AddModelErrorList(updateResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);

            TempData["SuccessMessage"] = "Profil bilgileri başarıyla değiştirilmiştir.";

            var userEditViewModel = new UserEditVM()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                City = user.City,
                BirtDate = user.BirthDate,
                Gender = user.Gender,              
            };

            return View(userEditViewModel);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
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
