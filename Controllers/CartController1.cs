using Eticket.Models;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    public class CartController1 : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController1(ICartRepository cartRepository,UserManager<ApplicationUser> userManager )
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
