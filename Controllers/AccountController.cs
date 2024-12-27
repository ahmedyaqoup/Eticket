using Eticket.Models;
using Eticket.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //private ApplicationUser user;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Register()
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new("Admin"));
                await roleManager.CreateAsync(new("Cinema"));
                await roleManager.CreateAsync(new("Customer"));
            }
            //await userManager.AddToRoleAsync(user, "Admin");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser user = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    Address = userVM.Address,
                    Name = userVM.Name
                };

                  var result=await userManager.CreateAsync(user,userVM.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("Password", "يرجى ادخال كلمه سر تحتوي على احرف كبيره وصغيره وارقام وحروف مميزه");
                }
                


            }
            return View(userVM);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var appUserWithEmail = await userManager.FindByEmailAsync(loginVM.Account);
                var appUserWithUserName = await userManager.FindByNameAsync(loginVM.Account);

                if (appUserWithEmail != null || appUserWithUserName != null)
                {
                    var result = await userManager.CheckPasswordAsync(appUserWithEmail ?? appUserWithUserName, loginVM.Password);

                    if (result)
                    {
                        await signInManager.SignInAsync(appUserWithEmail ?? appUserWithUserName, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "هناك خطأ في الحساب او في كلمه السر");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "هناك خطأ في الحساب او في كلمه السر");
                }
            }

            return View(loginVM);
        }


        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            return View(model: new ApplicationUserVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Address = user.Address
            });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ApplicationUserVM userVM)
        {
            var user = await userManager.GetUserAsync(User);
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.Name = userVM.Name;
            user.Address = userVM.Address;

            await userManager.UpdateAsync(user);
            TempData["success"] = "تم تحديث البيانات بنجاح";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> updatePhoto(IFormFile photo)
        {
            var user = await userManager.GetUserAsync(User);

            if (photo != null && photo.Length > 0)
            {
                // Genereate name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);

                // Save in wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    photo.CopyTo(stream);
                }

                // Save in db
                user.photo = fileName;
                await userManager.UpdateAsync(user);
            }

            TempData["success"] = "تم تحديث البيانات بنجاح";

            return RedirectToAction("Index", "Home");
        }































        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



    }
}
