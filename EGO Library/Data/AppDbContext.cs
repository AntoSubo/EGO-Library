using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace EGO_Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EgoGift> EgoGifts { get; set; }
        public DbSet<Sources> Sources { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ego_library.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // конфигурация EgoGift
            modelBuilder.Entity<EgoGift>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Status).HasMaxLength(50);
                entity.HasIndex(g => g.Name);
            });

            // конфигурация Sources
            modelBuilder.Entity<Sources>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Location).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Type).HasMaxLength(50);

                entity.HasOne(s => s.EgoGift)
                      .WithMany(g => g.Sources)
                      .HasForeignKey(s => s.EgoGiftId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // конфигурация Recipe
            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).HasMaxLength(100);
                entity.Property(r => r.Description).HasMaxLength(500);
                entity.Property(r => r.Location).HasMaxLength(100);
                entity.Property(r => r.Difficulty).HasMaxLength(20);

                // связь с результирующим даром
                entity.HasOne(r => r.ResultGift)
                      .WithMany(g => g.ResultRecipes)
                      .HasForeignKey(r => r.ResultGiftId)
                      .OnDelete(DeleteBehavior.Cascade);

                // связь многие-ко-многим с требуемыми дарами
                entity.HasMany(r => r.RequiredGifts)
                      .WithMany(g => g.RequiredInRecipes)
                      .UsingEntity(j => j.ToTable("RecipeRequiredGifts"));
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Email).HasMaxLength(100);
                entity.HasIndex(u => u.Username).IsUnique();


                entity.Property(u => u.Id).ValueGeneratedOnAdd();
            });
        }
    }
}