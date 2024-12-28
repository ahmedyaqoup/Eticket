using Eticket.Models;
using Eticket.Models.ViewModels;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Stripe.Checkout;

namespace Eticket.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartRepository cartRepository,UserManager<ApplicationUser> userManager )
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var cart = cartRepository.Get(includeProps: [e=>e.Movie], filter: e => e.ApplicationUserId == userManager.GetUserId(User));
            CartWithTotalPriceVM cartWithTotal = new CartWithTotalPriceVM()
            {
                Carts = cart.ToList(),
                TotalPrice = (double)cart.Sum(e => e.Movie.Price * e.Count)
            };

            return View(cartWithTotal);
        }
        public IActionResult Increment(int movieId)
        {
            var cart = cartRepository.GetOne(filter: e => e.ApplicationUserId == userManager.GetUserId(User) && e.MovieId == movieId);

            cart.Count++;
            cartRepository.Commit();

            return RedirectToAction("Index");
        }
        public IActionResult Decrement(int movieId)
        {
            var cart = cartRepository.GetOne(filter: e => e.ApplicationUserId == userManager.GetUserId(User) && e.MovieId == movieId);
            if (cart.Count > 1)
            {
                cart.Count--;
                cartRepository.Commit();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int movieId)
        {
            var movie = cartRepository.GetOne(filter: e => e.MovieId == movieId);
            cartRepository.Delete(movie);

            return RedirectToAction("Index", "Cart");
        }



        [HttpPost]
        public IActionResult AddToCart(int movieId, int Count=1  )
        {
            var userId = userManager.GetUserId(User);

            if (userId != null)
            {
                var cartInBD = cartRepository.GetOne(filter: e => e.ApplicationUserId == userId && e.MovieId == movieId);

                if (cartInBD != null)
                {
                    cartInBD.Count += Count;
                    cartRepository.Commit();
                }

                else
                {
                    cartRepository.Create(new()
                    {
                        ApplicationUserId = userId,
                        Count=Count,
                        MovieId = movieId,
                        Time = DateTime.Now
                    });

                }

                TempData["success"] = "تم اضافه الفلم الى السله بنجااح";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");


        }
        public IActionResult Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Checkout/Success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/Cancel",
            };

            var cart = cartRepository.Get(includeProps: [e => e.Movie], filter: e => e.ApplicationUserId == userManager.GetUserId(User)).ToList();

            foreach (var item in cart)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                        },
                        UnitAmount = (long)item.Movie.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
