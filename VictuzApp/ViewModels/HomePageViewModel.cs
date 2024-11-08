using VictuzApp.Models;
using VictuzApp.Services;

namespace VictuzApp.ViewModels
{
    public class HomePageViewModel
    {
        public ICollection<BestActivity> BestActivities { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
