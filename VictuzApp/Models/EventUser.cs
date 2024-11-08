using Microsoft.AspNetCore.Identity;

namespace VictuzApp.Models
{
    public class EventUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
