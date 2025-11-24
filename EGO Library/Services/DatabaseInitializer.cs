using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGO_Library.Services
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync()
        {
            try
            {
                using var context = new AppDbContext();

                // Создаем базу данных и таблицы, если их нет
                await context.Database.EnsureCreatedAsync();

                // Проверяем, есть ли уже данные
                if (await context.EgoGifts.AnyAsync())
                {
                    Console.WriteLine("Database already contains data. Skipping initialization.");
                    return;
                }

                Console.WriteLine("Initializing database with sample data...");

                // Создаем тестовые данные
                var sampleGifts = CreateSampleGifts();

                // Добавляем в контекст
                await context.EgoGifts.AddRangeAsync(sampleGifts);

                // Сохраняем в базу
                await context.SaveChangesAsync();

                Console.WriteLine("Database initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        private static List<EgoGift> CreateSampleGifts()
        {
            return new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Wealth",
                    Tier = 4,
                    Status = "Charge",
                    Icon = "💰",
                    Description = "Increases max Charge by 2",
                    Sources = new List<Sources> 
                    {
                        new Sources
                        {
                            Location = "Mirror Dungeon",
                            Type = "Dungeon",
                            Floor = 5,
                            DropRate = 0.15
                        },
                        new Sources
                        {
                            Location = "Fusion",
                            Type = "Crafting",
                            DropRate = 0.05
                        }
                    },
                    CreatedDate = DateTime.Now
                },
                new EgoGift
                {
                    Name = "Inferno",
                    Tier = 3,
                    Status = "Burn",
                    Icon = "🔥",
                    Description = "Applies Burn status each turn",
                    Sources = new List<Sources> 
                    {
                        new Sources
                        {
                            Location = "Mirror Dungeon",
                            Type = "Dungeon",
                            Floor = 3,
                            DropRate = 0.25
                        },
                        new Sources
                        {
                            Location = "Event: Fire Festival",
                            Type = "Event",
                            DropRate = 0.10
                        }
                    },
                    CreatedDate = DateTime.Now
                },
                new EgoGift
                {
                    Name = "Fortitude",
                    Tier = 2,
                    Status = "Defense",
                    Icon = "🛡️",
                    Description = "Reduces incoming damage by 15%",
                    Sources = new List<Sources> 
                    {
                        new Sources
                        {
                            Location = "Mirror Dungeon",
                            Type = "Dungeon",
                            Floor = 2,
                            DropRate = 0.30
                        }
                    },
                    CreatedDate = DateTime.Now
                }
            };
        }

        // Метод для проверки состояния базы данных
        public static async Task CheckDatabaseStatusAsync()
        {
            try
            {
                using var context = new AppDbContext();
                var giftCount = await context.EgoGifts.CountAsync();
                var sourcesCount = await context.Sources.CountAsync(); 

                Console.WriteLine($"Database status: {giftCount} gifts, {sourcesCount} sources");

                if (giftCount == 0)
                {
                    Console.WriteLine("Database is empty. Run InitializeAsync to populate.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking database status: {ex.Message}");
            }
        }
    }
}