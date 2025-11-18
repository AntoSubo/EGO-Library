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

        // ДОБАВЬТЕ этот метод
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}