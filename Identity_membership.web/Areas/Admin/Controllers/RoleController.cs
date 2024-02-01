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

        public async Task<IActionResult> EditRole(string id)
        {
            var roleToEdit = await _roleManager.FindByIdAsync(id);

            if (roleToEdit == null)
            {
                throw new ArgumentNullException(nameof(roleToEdit), "İlgili rol bulunamadı");
            }

            return View(new EditRoleVM() { Id = roleToEdit.Id, Name = roleToEdit!.Name! });
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);

            if (roleToDelete == null)
            {
                throw new ArgumentNullException(nameof(roleToDelete), "İlgili rol bulunamadı");
            }
            
            var result = await _roleManager.DeleteAsync(roleToDelete);

            if(!result.Succeeded) 
            {
                throw new Exception(result.Errors.Select(x=>x.Description).First());
            }

            TempData["SuccessMessage"] = "İlgili role silindi";

            return RedirectToAction(nameof(RoleController.Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM req)
        {

            var roleToEdit = await _roleManager.FindByIdAsync(req.Id);

            if (roleToEdit == null)
            {
                throw new ArgumentNullException(nameof(roleToEdit), "İlgili rol bulunamadı");
            }

            roleToEdit.Name = req.Name;

            await _roleManager.UpdateAsync(roleToEdit);

            ViewData["SuccessMessage"] = "Role bilgisi güncellendi";

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

            TempData["SuccessMessage"] = "Role oluşturma işlemi başarıyla gerçekleşti";

            return RedirectToAction(nameof(RoleController.Index));
        }
    }
}
