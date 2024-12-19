using Eticket.Models;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Eticket.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;
        

        public MovieController(IMovieRepository movieRepository,ICategoryRepository categoryRepository
            ,ICinemaRepository cinemaRepository,IActorRepository actorRepository )
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
            this.actorRepository = actorRepository;
            
        }
        public IActionResult Index()
        {
            var movies=movieRepository.Get(includeProps: [e => e.Category, e => e.Cinema]).ToList();
            return View(movies);
        }
        public IActionResult Create()
        {
            var categories = categoryRepository.Get().ToList();
            var cinemas = cinemaRepository.Get().ToList();
            var actors = actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;

            return View(new Movie());
        
        }
        [HttpPost]
        public IActionResult Create(Movie movie,IFormFile? file)
        {
            ModelState.Remove("ImgUrl");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("actorsMovies");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save in db
                    movie.ImgUrl = fileName;



                }
                movieRepository.Create(movie);
                TempData["success"] = "Add Movie successfuly";
                return RedirectToAction("Index");

            }
            movieRepository.Create(movie);

            return View(movie);

        }




        public IActionResult Edit(int id)
        {
            var movie = movieRepository.GetOne(x => x.Id == id);
            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var categories = categoryRepository.Get().ToList();
            var cinemas = cinemaRepository.Get().ToList();
            var actors = actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;




            return View(movie);

        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile file)
        {

            ModelState.Remove("ImgUrl");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");

            var oldMovie = movieRepository.GetOne(e => e.Id == movie.Id);

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", oldMovie.ImgUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    movie.ImgUrl = fileName;
                }
                else
                {
                    movie.ImgUrl = oldMovie.ImgUrl;
                }

                movieRepository.Edit(movie);

                TempData["success"] = "Update Movie successfuly";

                return RedirectToAction("Index");
            }

            movie.ImgUrl = oldMovie.ImgUrl;
            var categories = categoryRepository.Get().ToList();
            ViewBag.category = categories;


            var cinemas = cinemaRepository.Get().ToList();
            ViewBag.cinema = cinemas;


            return RedirectToAction("Index");



        }

        public IActionResult Delete(int id)
        {
            var movie = movieRepository.GetOne(filter: e => e.Id == id);
            if (movie != null)
            {
                movieRepository.Delete(movie);

                TempData["success"] = "Delete Movie successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundSearch", "Home");




        }



    }
}
