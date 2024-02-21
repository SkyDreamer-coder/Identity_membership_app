using Identity_membership.web.Models;
using Identity_membership.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Identity_membership.web.Extensions;
using Microsoft.AspNetCore.SignalR;
using Identity_membership.web.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Identity_membership.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager; // user login, logout, cookie operations
        private readonly IEmailService _emailService;


        public HomeController(ILogger<HomeController> logger, UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM req, string? returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl ??= Url.Action("Index", "Home");

            var userVal = await _userManager.FindByEmailAsync(req.Email);

            if (userVal == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(userVal, req.Password, req.RememberMe, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Ard arda 3 kere hatalý giriþ yaptýnýz. Hesabýnýz geçici olarak kilitlendi." });
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { $"Email veya þifre yanlýþ", $"Baþarýsýz giriþ sayýsý = {await _userManager.GetAccessFailedCountAsync(userVal)} / {_userManager.Options.Lockout.MaxFailedAccessAttempts}." });
                return View();
            }

            if (userVal.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(userVal, req.RememberMe, new[] { new Claim("birthdate", userVal.BirthDate.Value.ToString()) });
            }
            return Redirect(returnUrl!);
          
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM req)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var signUpResult = await _userManager.CreateAsync(new() { UserName = req.UserName, PhoneNumber = req.Phone, Email = req.Email }, req.PasswordConfirm);

            if (!signUpResult.Succeeded)
            {
                ModelState.AddModelErrorList(signUpResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            //Generating Claim.
            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var user = (await _userManager.FindByNameAsync(req.UserName))!;

            var claimResult = await _userManager.AddClaimAsync(user, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(claimResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            TempData["SuccessMessage"] = "Üyelik iþlemi baþarýyla gerçekleþti.";
            return RedirectToAction(nameof(HomeController.SignIn));
;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM req)
        {

            var userVal = await _userManager.FindByEmailAsync(req.Email);

            if (userVal == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanýcý bulunamamýþtýr.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(userVal);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = userVal.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            // exp link https://localhost:7048?userId=vvhc&token=sbhsbvvzkcvnkzjjdjkgkjdhsg

            // email service

            await _emailService.SendResetPasswordEmail(passwordResetLink!, userVal.Email!);

            TempData["SuccessMessage"] = "Þifre sýfýrlama linki e-posta adresinize gönderilmiþtir";

            return RedirectToAction(nameof(ForgotPassword));
        }

        public IActionResult ResetPassword(string userId, string token) 
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM req)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var userVal = await _userManager.FindByIdAsync(userId.ToString()!);

            if (userVal == null) 
            {
                ModelState.AddModelError(string.Empty, "kullanýcý bulunamamýþtýr.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(userVal, token.ToString()!, req.PasswordConfirm);

            if(result.Succeeded) 
            {
                TempData["SuccessMessage"] = "Þifreniz baþarýyla yenilenmiþtir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x=>x.Description).ToList());
                return View();
            }

            return RedirectToAction("SignIn", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
