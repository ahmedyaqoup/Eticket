using Eticket.Models;
using Eticket.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
                    if (await userManager.IsLockedOutAsync(appUserWithEmail ?? appUserWithUserName))
                    {
                        ModelState.AddModelError(string.Empty, "This account has been blocked.");
                        return View(loginVM);
                    }
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
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(EmailVM userEmail)
        {
            if (ModelState.IsValid)
            {
                var appUserithEmail = await userManager.FindByEmailAsync(userEmail.Account);
                var appUserithName = await userManager.FindByNameAsync(userEmail.Account);
                if (appUserithEmail != null || appUserithName != null)
                {

                    return RedirectToAction("ChangeForgottenPassword", "Account", new { acc = userEmail.Account });
                }
                else
                {
                    ModelState.AddModelError("Account", "This Account Dosn't Exist");
                }

            }

            return View(userEmail);

        }
        [HttpGet]
        public IActionResult ChangeForgottenPassword(string acc)
        {
            ViewBag.Email = acc;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeForgottenPassword(ForgetPassordVM ForgetPassordVM)
        {
            if (ModelState.IsValid)
            {
                var appUserithEmail = await userManager.FindByEmailAsync(ForgetPassordVM.Account);
                var appUserithName = await userManager.FindByNameAsync(ForgetPassordVM.Account);

                await userManager.RemovePasswordAsync(appUserithEmail ?? appUserithName);
                await userManager.AddPasswordAsync(appUserithEmail ?? appUserithName, ForgetPassordVM.Password);
                TempData["success"] = "Password has been changed successfully";
                return RedirectToAction("Index", "Home");
            }
            return View(ForgetPassordVM);
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                var appUser = await userManager.FindByIdAsync(userManager.GetUserId(User));

                var result = await userManager.ChangePasswordAsync(appUser, changePasswordVM.OldPassword, changePasswordVM.NewPassword);
                if (result.Succeeded)
                {
                    TempData["success"] = "The password has changed correctlly";
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "there is some thing wrong ");
            }
            return View(changePasswordVM);


        }































        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



    }
}
