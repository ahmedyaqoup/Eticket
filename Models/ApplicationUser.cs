using Microsoft.AspNetCore.Identity;

namespace Eticket.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
