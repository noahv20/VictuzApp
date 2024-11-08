using Microsoft.AspNetCore.Identity;

namespace VictuzApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    }
}
