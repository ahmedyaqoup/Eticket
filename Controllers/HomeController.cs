using System.Diagnostics;
using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository movieRepository;
        

        public HomeController(ILogger<HomeController> logger,IMovieRepository movieRepository )
        {
            _logger = logger;
            this.movieRepository = movieRepository;
           
        }

        public IActionResult Index(int page=1)
        {
            var movie = movieRepository.Get(includeProps: [e=>e.Category,e=>e.Cinema]).ToList();
            movie = movie.Skip((page - 1) * 10).Take(10).ToList();

            return View(movie);
        }

        public IActionResult Details(int id)
        {

            var movies=movieRepository.Get(filter:e=>e.Id==id, includeProps: [e => e.Category, e => e.Cinema,e=>e.Actors]).FirstOrDefault();
           // var actormovies=actorMovieRepository.Get(includeProps:[e=>e.ActorsId]).ToList();
            //ViewBag.actormovies=actormovies;
            return View(movies);
        }

       
        public IActionResult Search(string Name)
        {
            var movies= movieRepository.Get(filter: e => e.Name.Contains(Name), includeProps: [e => e.Category, e => e.Cinema]).ToList();
            if (movies != null && movies.Count > 0)
            {
                return View(movies);
            }
            return RedirectToAction("NotFoundSearch");

        }

        public IActionResult NotFoundSearch()
        {
            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
