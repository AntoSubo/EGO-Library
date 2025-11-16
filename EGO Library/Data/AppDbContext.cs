using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace EGO_Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EgoGift> EgoGifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=egolibrary.db");
        }
    }
}