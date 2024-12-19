using System.ComponentModel.DataAnnotations;

namespace Eticket.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Length(3, 50, ErrorMessage = "Name must be in 3 charechters to 50")]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
