using Microsoft.AspNetCore.Identity;

namespace Domain.Model
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
