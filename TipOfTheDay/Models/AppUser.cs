using Microsoft.AspNetCore.Identity;

namespace TipOfTheDay.Models
{
    public class AppUser : IdentityUser
    {
        public String? FullName { get; set; }
    }
}
