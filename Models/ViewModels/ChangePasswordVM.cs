using System.ComponentModel.DataAnnotations;

namespace Eticket.Models.ViewModels
{
    public class ChangePasswordVM
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassord { get; set; }
    }
}
