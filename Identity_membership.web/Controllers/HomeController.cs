using Identity_membership.web.Models;
using Identity_membership.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Identity_membership.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<UserApp> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<UserApp> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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

            foreach(IdentityError err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
