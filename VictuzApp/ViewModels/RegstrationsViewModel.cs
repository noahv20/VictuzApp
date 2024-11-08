using Microsoft.AspNetCore.Identity;
using VictuzApp.Models;

namespace VictuzApp.ViewModels
{
    public class RegstrationsViewModel
    {
        public Event Event { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
