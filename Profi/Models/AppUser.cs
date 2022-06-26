using Microsoft.AspNetCore.Identity;

namespace Profi.Models
{
    public class AppUser:IdentityUser
    {
        public bool IsActivated { get; set; }
    }
}
