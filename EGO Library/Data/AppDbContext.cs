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
            // Конфигурация для EgoGift
            modelBuilder.Entity<EgoGift>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<EgoGift>()
                .HasMany(g => g.Sources)
                .WithOne()
                .HasForeignKey(s => s.EgoGiftId);

            // Конфигурация для Sources
            modelBuilder.Entity<Sources>()
                .HasKey(s => s.Id);
        }
    }
}