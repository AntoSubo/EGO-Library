using Microsoft.EntityFrameworkCore;
using EGO_Library.Models;
using System.IO;
using Microsoft.Extensions.Logging; 

namespace EGO_Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EgoGift> EgoGifts { get; set; }
        public DbSet<Sources> Sources { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<User> Users { get; set; }

        // Путь к БД
        public string DbPath { get; }

        public AppDbContext()
        {
            // БД будет рядом с приложением
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            DbPath = Path.Combine(appPath, "ego_library.db");

        }
        // Дополнительный конструктор с параметром (чтобы не было ошибки CS1729)
        public AppDbContext(string dbPath)
        {
            DbPath = dbPath;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var fullPath = Path.GetFullPath(DbPath);
                Console.WriteLine($"Configuring DB at: {fullPath}");
                optionsBuilder.UseSqlite($"Data Source={fullPath}");
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message),
                                   LogLevel.Information);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EgoGift>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();

                entity.HasMany(e => e.Sources)
                    .WithOne(s => s.EgoGift)
                    .HasForeignKey(s => s.EgoGiftId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.ResultGift)
                    .WithMany(g => g.ResultRecipes)
                    .HasForeignKey(r => r.ResultGiftId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.RequiredGifts)
                    .WithMany(g => g.RequiredInRecipes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RecipeRequiredGifts",
                        j => j.HasOne<EgoGift>().WithMany().HasForeignKey("EgoGiftId"),
                        j => j.HasOne<Recipes>().WithMany().HasForeignKey("RecipesId"), 
                        j => j.HasKey("RecipesId", "EgoGiftId")); 
            });
        }
    }
}