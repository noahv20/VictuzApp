using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VictuzApp.Models;

namespace VictuzApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relaties
            modelBuilder.Entity<Event>()
                .HasMany(a => a.Participants)
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

            //Participants Data Seed (Dummy Data)
            Participant p = new Participant()
            {
                Id = 1,
                Name = "Rob Cilissen",
                Email = "Rob.Cilissen@zuyd.nl",
                IsMember = true
            };

            //BoardMember Data Seed (Dummy Data)
            BoardMember bm = new BoardMember()
            {
                Id = 2,
                Name = "Miel Noelanders",
                Email = "Miel.Noelanders@zuyd.be",
                IsMember = true
            };

            //Administrator Data Seed (Dummy Data)
            Administrator admin = new Administrator()
            {
                Id = 3,
                Name = "admin",
                Email = "admin@admin.nl",
                IsMember = true
            };

            // Add Data To Database
            modelBuilder.Entity<Event>()
                .HasData(e);

            modelBuilder.Entity<Participant>()
                .HasData(p);

            modelBuilder.Entity<BoardMember>()
                .HasData(bm);

            modelBuilder.Entity<Administrator>()
                .HasData(admin);
        }
    }
}
