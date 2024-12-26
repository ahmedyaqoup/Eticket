using Eticket.Models;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eticket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActorController : Controller
    {
        private readonly IActorRepository actorRepository;
        private readonly IMovieRepository movieRepository;

        public ActorController(IActorRepository actorRepository,IMovieRepository movieRepository)
        {
            this.actorRepository = actorRepository;
            this.movieRepository = movieRepository;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var actor=actorRepository.Get().ToList();
            return View(actor);
        }
        [AllowAnonymous]
        public IActionResult DetailsActor(int id)
        {
            var actor = actorRepository.GetOne(e => e.Id == id);

            return View(actor);

        }

        public IActionResult Create() 
        {
            return View();
        
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile file) 
        {
            ModelState.Remove("ProfilePicture");
            
            ModelState.Remove("Movies");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save in db
                    actor.ProfilePicture = fileName;
                }

                actorRepository.Create(actor);


                TempData["success"] = "Add Actor successfuly";

                return RedirectToAction("Index");
            }
            return View(actor);


        }


        public IActionResult Edit(int id)
        {
            var actor = actorRepository.GetOne(e => e.Id == id);
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile? file)
        {
            ModelState.Remove("ProfilePicture");
            ModelState.Remove("movies");
            

            var oldActor = actorRepository.GetOne(e => e.Id == actor.Id, tracked: false);

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", oldActor.ProfilePicture);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    actor.ProfilePicture = fileName;
                }
                else
                {
                    actor.ProfilePicture = oldActor.ProfilePicture;
                }

                actorRepository.Edit(actor);

                TempData["success"] = "Update actor successfuly";

                return RedirectToAction("Index");
            }

            actor.ProfilePicture = actor.ProfilePicture;

            return View(actor);
        }

        public IActionResult Delete(int id)
        {
            var actor = actorRepository.GetOne(e => e.Id == id);
            if (actor == null) return RedirectToAction("NotFoundPage", "Home");
            
            actorRepository.Delete(actor);
            TempData["success"] = "Deleted actor successfully";
            return RedirectToAction("Index");
        }



    }
}
