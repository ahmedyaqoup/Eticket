using Eticket.Models;
using Eticket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    public class CheckoutController1 : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public CheckoutController1(IEmailSender emailSender,UserManager<ApplicationUser> userManager)
        {
            this.emailSender = emailSender;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Success()
        {
            var user = await userManager.GetUserAsync(User);
            await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), "Success pay", "تم الدفع بنجاح");
            return RedirectToAction("Index","Home");
        }
    }
}
