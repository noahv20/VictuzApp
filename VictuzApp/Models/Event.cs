using System.ComponentModel.DataAnnotations;

namespace VictuzApp.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        public ICollection<Participant>? Participants { get; set; }
    }
}
