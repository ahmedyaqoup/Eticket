using Eticket.Models;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMovieRepository movieRepository;

        public CategoryController(ICategoryRepository categoryRepository,IMovieRepository movieRepository )
        {
            this.categoryRepository = categoryRepository;
            this.movieRepository = movieRepository;
        }
        
        public IActionResult Index()
        {
            return View( categoryRepository.Get().ToList());
        }

        public IActionResult AllMovies(int id) 
        {
            var movie = movieRepository.Get(filter: e => e.CategoryId == id, includeProps: [e => e.Cinema, e => e.Category]).ToList();
            return View( movie );
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                categoryRepository.Create(category);

                TempData["success"] = "Add Category successfuly";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Edit(int id)
        {

            var category = categoryRepository.GetOne(e => e.Id == id);

            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundSearch", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                categoryRepository.Edit(category);
                TempData["success"] = "Update Category successfuly";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {

            var category = categoryRepository.GetOne(e => e.Id == id);
            if (category != null)
            {
                categoryRepository.Delete(category);

                TempData["success"] = "Delete Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }



    }
}
