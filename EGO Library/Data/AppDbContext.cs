// Data/AppDbContext.cs
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace EGO_Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EgoGift> EgoGifts { get; set; }
        public DbSet<Sources> Sources { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ego_library.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация EgoGift
            modelBuilder.Entity<EgoGift>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Status).HasMaxLength(50);
                entity.Property(g => g.Icon).HasMaxLength(10);
                entity.HasIndex(g => g.Name);
            });

            // Конфигурация Sources
            modelBuilder.Entity<Sources>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Location).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Type).HasMaxLength(50);

                // Связь с EgoGift
                entity.HasOne(s => s.EgoGift)
                      .WithMany(g => g.Sources)
                      .HasForeignKey(s => s.EgoGiftId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}