using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SecurityAdd.Models;
using SecurityAdd.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace SecurityAdd.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        public readonly SignInManager<ApplicationUser> signInManager;
        public AuthenticationController(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager= _signInManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            ;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterVM registerVM=new RegisterVM();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = registerVM.UserName;
                applicationUser.Address = registerVM.Address;
                applicationUser.FName=registerVM.FName;
                applicationUser.Lname=registerVM.Lname;
                applicationUser.PasswordHash = registerVM.password;
                 IdentityResult identityResult=await userManager.CreateAsync(applicationUser,registerVM.password);
                if (identityResult.Succeeded)
                {
                 await   userManager.AddToRoleAsync(applicationUser,"Admin");
                 await  signInManager.SignInAsync(applicationUser, false);
                 return   View("Index");
                }
                else
                {
                    foreach(var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                }
            }
            return View(registerVM);
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            LoginVM loginVM = new LoginVM();
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
            ApplicationUser applicationUser= await userManager.FindByNameAsync(loginVM.UserName);
                if (applicationUser != null)
                {
                bool found= await userManager.CheckPasswordAsync(applicationUser,loginVM.Password);
                    if (found)
                    {
                        Claim cm = new Claim("ITI", "islam");
                        //User.Claims.Append(cm);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(cm);
                        await  signInManager.SignInWithClaimsAsync(applicationUser,true,claims);
                       string nn= User.Identity.Name;
                       

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("Password", "invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Name Is Invalid");
                }
                
            }
            return View(loginVM);
        }

        public async Task<IActionResult> logout()
        {
           await signInManager.SignOutAsync();
            //await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
           
            return RedirectToAction("LogIn");
        }

    }
}
