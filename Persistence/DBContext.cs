using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Shelter> Shelters { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gyvunas priklauso priglaudai
            modelBuilder.Entity<Shelter>()
                .HasMany(s => s.Pets)
                .WithOne(p => p.Shelter)
                .HasForeignKey(p => p.ShelterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Darbuotas priklauso prieglaudai
            modelBuilder.Entity<Shelter>()
                .HasMany(s => s.Users)
                .WithOne(p => p.Shelter)
                .HasForeignKey(p => p.ShelterId);

            // Prieglauda turi daug rezervaciju
            modelBuilder.Entity<Shelter>()
                .HasMany(s => s.Reservations)
                .WithOne(p => p.Shelter)
                .HasForeignKey(p => p.ShelterId);

            // Vartotojas turi daug rezervaciju
            modelBuilder.Entity<User>()
                .HasMany(s => s.Reservations)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Gyvunas turi daug paveiksliuku
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.Photos)
                .WithOne(f => f.Pet)
                .HasForeignKey(f => f.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prieglauda turi viena paveiksleli
            modelBuilder.Entity<Shelter>()
                .HasOne(s => s.ShelterPhoto)
                .WithOne(f => f.Shelter)
                .HasForeignKey<File>(f => f.ShelterId);

            // gyvunai pamegti vartotoju
            modelBuilder.Entity<User>()
            .HasMany(p => p.Pets)
            .WithMany(p => p.Users)
            .UsingEntity<LovedPets>(
                j => j
                    .HasOne(pt => pt.Pet)
                    .WithMany(t => t.LovedPets)
                    .HasForeignKey(pt => pt.PetId),
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(p => p.LovedPets)
                    .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.PetId, t.UserId });
                });

            modelBuilder.Entity<Reservation>()
                .Property(p => p.ReservationState)
                .HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(p => p.Sex)
                .HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(p => p.Sex)
                .HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(p => p.Size)
                .HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(p => p.Color)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(p => p.Role)
                .HasConversion<string>();
        }
    }
}
