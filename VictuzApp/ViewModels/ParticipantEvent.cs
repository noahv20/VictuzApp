using VictuzApp.Models;

namespace VictuzApp.ViewModels
{
    public class ParticipantEvent
    {
        public Event Event { get; set; }
        public List<Participant> Participants { get; set; }
        public int SelectedParticipantId { get; set; }
    }
}
