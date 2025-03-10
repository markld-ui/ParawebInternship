using Microsoft.AspNetCore.Identity;

namespace LandingAPI.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
