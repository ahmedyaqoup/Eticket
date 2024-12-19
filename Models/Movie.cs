using Eticket.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Eticket.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus { get; set; }
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public Category Category { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();

        public List<ActorMovies> actorsMovies { get; set; } = new List<ActorMovies>();
    }
}
