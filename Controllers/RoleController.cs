using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SecurityAdd.Models;
using SecurityAdd.ViewModels;

namespace SecurityAdd.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager { get; }
        public UserManager<ApplicationUser> userManager { get; }

        public RoleController(RoleManager<IdentityRole> _roleManager,UserManager<ApplicationUser> userManager)
        {
            roleManager = _roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult createRole()
        {
            RoleVM roleVM = new RoleVM();
            return View(roleVM);
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> createRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleVM.RoleName;
           IdentityResult result=   await  roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                 RoleVM roleVM1 = new RoleVM();
                    return View(roleVM1);
                }
                else
                {
                    foreach(var merr in result.Errors)
                    {
                        ModelState.AddModelError("", merr.Description);
                    }
                }
            }

            return View(roleVM);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            userRoleVM userRoleVM = new userRoleVM();
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddRole(userRoleVM userRoleVM)
        {
            if (ModelState.IsValid) {
             ApplicationUser app =await  userManager.FindByNameAsync(userRoleVM.UserName);
                if (app != null)
                {
                   await userManager.AddToRoleAsync(app, userRoleVM.RoleName);
                }
                else
                {
                    ModelState.AddModelError("UserName", "invalid User Name");
                }
            
            }
            return View(userRoleVM);
        }
    }
}
