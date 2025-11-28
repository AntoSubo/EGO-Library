using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace EGO_Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Настройте строку подключения здесь
            optionsBuilder.UseSqlite("Data Source=ego_library.db");
        }
    }
}