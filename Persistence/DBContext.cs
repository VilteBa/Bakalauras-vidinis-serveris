using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Persistence
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Shelter> Shelters { get; set; }

        public DbSet<User> Users { get; set; }

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


            //modelBuilder.Entity<LovedPets>()
            //    .HasKey(t => new { t.UserId, t.PetId });

            //modelBuilder.Entity<LovedPets>()
            //    .HasOne(u => u.Pet)
            //    .WithMany(p => p.LovedPets)
            //    .HasForeignKey(lp => lp.PetId);

            //modelBuilder.Entity<LovedPets>()
            //    .HasOne(lp => lp.User)
            //    .WithMany(u => u.LovedPets)
            //    .HasForeignKey(lp => lp.UserId);

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
