using Eticket.Models;
using Microsoft.EntityFrameworkCore;

namespace Eticket.Data
{
    public class ApplicationDbcontext:DbContext
    {
        public DbSet<Movie> movies { get; set; }
        public DbSet<ActorMovies> actorMovies { get; set; }
        public DbSet<Actor> actors { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
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

    }
}
