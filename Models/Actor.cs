using System.ComponentModel.DataAnnotations;

namespace Eticket.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        public string News { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
