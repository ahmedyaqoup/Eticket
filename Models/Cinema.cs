using System.ComponentModel.DataAnnotations;

namespace Eticket.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [Length(3, 25, ErrorMessage = "three charechters or more")]
        public string Name { get; set; }
        [Required]
        [Length(3, 25, ErrorMessage = "three charechters or more")]
        public string Description { get; set; }
        [Required]
        public string CinemaLogo { get; set; }
        [Required]
        public string Address { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
