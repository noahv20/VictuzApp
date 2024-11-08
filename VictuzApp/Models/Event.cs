using Microsoft.AspNetCore.Identity;
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

        // bool waarde waar zetten in controle
        // alleen voor de member geen andere rol
        // In index alleen bool false laten zien
        public bool IsSuggestion { get; set; } = false;
        public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    }
}
