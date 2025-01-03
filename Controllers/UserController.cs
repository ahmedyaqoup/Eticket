using Eticket.Models;
using Eticket.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index(string? Account)
        {
            if (Account == null)
            {
                return View(userManager.Users.ToList());
            }

            var Users = userManager.Users.Where(e =>
                 e.Email.Contains(Account) || e.UserName.Contains(Account)).ToList();
            if (Users.Any())
            {

                return View( Users);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchForUser(string account)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { Account = account });

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> blockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.SetLockoutEnabledAsync(user, true);
                var result = await userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
                //await userManager.SetLockoutEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    user.ISBlocked = true;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user has been blocked succefully";
                    var allUser = userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "The user has not been blocked";
                var allUser2 = userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("NotFoundSearh");
        }

        public async Task<IActionResult> unBlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //await userManager.SetLockoutEnabledAsync(user, true);
                var result = await userManager.SetLockoutEndDateAsync(user, null);
                await userManager.SetLockoutEnabledAsync(user, false);

                if (result.Succeeded)
                {
                    user.ISBlocked = false;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user is not blocked from thd site";
                    var allUser = userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "something error has happened";

                var allUser2 = userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("NotFoundPage", "home");
        }
        [HttpGet]
        public async Task<IActionResult> AddToRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = await userManager.GetRolesAsync(user);
            var roles = roleManager.Roles.ToList();
            return View(new AddUserToRoleVM()
            {
                Id = user.Id,
                Name = user.Name,
                Role = string.Join("", role),
                Roles = roles
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleVM userfrmVM)
        {

            var user = await userManager.FindByIdAsync(userfrmVM.Id);
            if (user != null)
            {
                var oldRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, oldRoles);
                var result = await userManager.AddToRoleAsync(user, userfrmVM.Role);
                if (result.Succeeded)
                {
                    TempData["success"] = "The User Role Has Been Changed";
                }
                return View("Index", userManager.Users.ToList());
            }
            return RedirectToAction("NotFoundPage", "home");
        }
    }
}
