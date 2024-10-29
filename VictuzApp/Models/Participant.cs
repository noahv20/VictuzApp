using System.ComponentModel.DataAnnotations;

namespace VictuzApp.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(251)]
        public string Email { get; set; }
        [Required]
        public bool IsMember { get; set; }
        public ICollection<Event>? Events { get; set; }

    }
}
