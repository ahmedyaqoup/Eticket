using System.ComponentModel.DataAnnotations;

namespace Eticket.Models.ViewModels
{
    public class ForgetPassordVM
    {
        public int Id { get; set; }
        public string Account { get; set; }
        [Required]
        [Length(7, 100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassord { get; set; }
    }
}
