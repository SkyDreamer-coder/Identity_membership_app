using Identity_membership.web.Models;
using Identity_membership.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Identity_membership.web.Extensions;

namespace Identity_membership.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<UserApp> _userManager;

        private readonly SignInManager<UserApp> _signInManager; // user login, logout, cookie operations

        public HomeController(ILogger<HomeController> logger, UserManager<UserApp> userManager, SignInManager<UserApp> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
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
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var userVal = await _userManager.FindByEmailAsync(req.Email);

            if (userVal == null) 
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(userVal, req.Password, req.RememberMe, true);

            if(result.Succeeded) 
            {
                return Redirect(returnUrl);
            }

            if(result.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Ard arda 3 kere hatalý giriþ yaptýnýz. Hesabýnýz geçici olarak kilitlendi." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { $"Email veya þifre yanlýþ", $"Baþarýsýz giriþ sayýsý = {await _userManager.GetAccessFailedCountAsync(userVal)} / {_userManager.Options.Lockout.MaxFailedAccessAttempts}." });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM req)
        {

            if(!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userManager.CreateAsync(new() { UserName = req.UserName, PhoneNumber = req.Phone, Email = req.Email }, req.PasswordConfirm);

            if (result.Succeeded) 
            {
                TempData["SuccessMessage"] = "Üyelik iþlemi baþarýyla gerçekleþti.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }


            ModelState.AddModelErrorList(result.Errors.Select(x=>x.Description).ToList());
   

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
