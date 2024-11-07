using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VictuzApp.Models;

namespace VictuzApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Users)
                .WithMany(p => p.Events);

            //Activity Data Seed (Dummy Data)
            Event e = new Event()
            {
                Id = 1,
                Title = "Stickers maken",
                Description = "Tijdens de Stickers Maken workshop op Hogeschool Zuyd leren studenten creatieve en praktische ontwerpvaardigheden. Ze ontwerpen hun eigen stickers met grafische software, ontdekken de basisprincipes van kleur, vorm en compositie, en maken vervolgens hun ontwerpen werkelijkheid met printtechnieken en een snijplotter. Deze workshop biedt een leuke, hands-on ervaring waarbij studenten hun creativiteit kunnen uiten en unieke, zelfgemaakte stickers mee naar huis nemen.",
                Date = new DateTime(2024, 10, 18),
                MaxParticipants = 30
            };

           

            // Add Data To Database
            modelBuilder.Entity<Event>()
                .HasData(e);
        }
    }
}
