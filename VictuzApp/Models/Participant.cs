using Microsoft.AspNetCore.Identity;

namespace VictuzApp.Models
{
    public class Participant : IdentityUser
    {
        public ICollection<Event>? Events { get; set; }
    }
}
