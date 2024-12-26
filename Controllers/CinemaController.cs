using Eticket.Data;
using Eticket.Models;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;
        private readonly IMovieRepository movieRepository;

        public CinemaController(ICinemaRepository cinemaRepository,IMovieRepository movieRepository ) 
        {
            this.cinemaRepository = cinemaRepository;
            this.movieRepository = movieRepository;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(cinemaRepository.Get().ToList());
        }
        [AllowAnonymous]
        public IActionResult AllMovies(int id)
        {
            var movie = movieRepository.Get(filter: e => e.CinemaId == id, includeProps: [e=>e.Cinema, e => e.Category]).ToList();

            return View(movie);
        }

        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid) 
            {
                
                cinemaRepository.Create(cinema);
                TempData["success"] = "Add Cinema successfuly";

                return RedirectToAction("Index");


            }
            return View(cinema);

        }

        public IActionResult Edit(int id)
        {

            var cinema = cinemaRepository.GetOne(e => e.Id==id);

            if (cinema != null)
            {
                return View(cinema);
            }

            return RedirectToAction("NotFoundSearch", "Search");


        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {

            cinemaRepository.Edit(cinema);
            TempData["success"] = "Edit Cinema successfuly";
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Delete(int id)
        {
            var cinema = cinemaRepository.GetOne( e => e.Id == id);
            if (cinema != null)
            {
                cinemaRepository.Delete(cinema);

                TempData["success"] = "Delete Cinema successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundSearch", "Home");

        }









    }
}
