using Eticket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eticket.Models.ViewModels;

namespace Eticket.Data
{
    public class ApplicationDbcontext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> movies { get; set; }
        public DbSet<ActorMovies> actorMovies { get; set; }
        public DbSet<Actor> actors { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<Cart> carts { get; set; }

        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options)
           : base(options)
        {
        }
        public ApplicationDbcontext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Eticket500;Integrated Security=True;TrustServerCertificate=True");
        }
        public DbSet<Eticket.Models.ViewModels.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<Eticket.Models.ViewModels.LoginVM> LoginVM { get; set; } = default!;
        public DbSet<Eticket.Models.ViewModels.AddUserToRoleVM> AddUserToRoleVM { get; set; } = default!;

    }
}
