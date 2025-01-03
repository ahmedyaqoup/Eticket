using System.ComponentModel.DataAnnotations;

namespace Eticket.Models.ViewModels
{
    public class EmailVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Name Or Email")]
        
        public string Account { get; set; }
    }
}
