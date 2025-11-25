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

                // Создаем рецепты
                await CreateRecipesAsync(context);

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
            var gifts = new List<EgoGift>();

            // Базовые статусы и иконки
            var statuses = new[] { "Burn", "Bleed", "Charge", "Poise", "Defense", "Attack", "Support", "Heal" };
            var icons = new[] { "🔥", "💧", "⚡", "🛡️", "⚔️", "❤️", "🌟", "💎" };

            var random = new Random();

            for (int i = 1; i <= 200; i++)
            {
                var status = statuses[random.Next(statuses.Length)];
                var icon = icons[random.Next(icons.Length)];
                var tier = random.Next(1, 6); // Tier от 1 до 5

                gifts.Add(new EgoGift
                {
                    Name = $"{status} Gift {i}",
                    Tier = tier,
                    Status = status,
                    Icon = icon,
                    Description = $"This is a sample description for {status} Gift {i}. It provides various benefits in combat.",
                    Sources = CreateSourcesForGift(random, tier),
                    CreatedDate = DateTime.Now.AddDays(-random.Next(365))
                });
            }

            // Добавляем несколько конкретных даров из ТЗ
            gifts.AddRange(new List<EgoGift>
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
                        new Sources { Location = "Mirror Dungeon", Type = "Dungeon", Floor = 5, DropRate = 0.15 },
                        new Sources { Location = "Fusion", Type = "Crafting", DropRate = 0.05 }
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
                        new Sources { Location = "Mirror Dungeon", Type = "Dungeon", Floor = 3, DropRate = 0.25 },
                        new Sources { Location = "Event: Fire Festival", Type = "Event", DropRate = 0.10 }
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
                        new Sources { Location = "Mirror Dungeon", Type = "Dungeon", Floor = 2, DropRate = 0.30 }
                    },
                    CreatedDate = DateTime.Now
                }
            });

            return gifts;
        }

        private static List<Sources> CreateSourcesForGift(Random random, int tier)
        {
            var sources = new List<Sources>();
            var locations = new[] { "Mirror Dungeon", "Arena", "Shop", "Event", "Fusion" };
            var types = new[] { "Dungeon", "Boss Drop", "Shop", "Event", "Crafting" };

            // Каждый дар имеет 1-3 источника
            var sourceCount = random.Next(1, 4);
            for (int j = 0; j < sourceCount; j++)
            {
                var location = locations[random.Next(locations.Length)];
                var type = types[random.Next(types.Length)];
                var floor = location == "Mirror Dungeon" ? random.Next(1, 6) : 0;
                var dropRate = random.NextDouble() * 0.5; // от 0 до 0.5

                sources.Add(new Sources
                {
                    Location = location,
                    Type = type,
                    Floor = floor,
                    DropRate = dropRate
                });
            }

            return sources;
        }

        private static async Task CreateRecipesAsync(AppDbContext context)
        {
            var gifts = await context.EgoGifts.ToListAsync();
            var random = new Random();

            var recipes = new List<Recipe>();

            // Создаем 50 рецептов
            for (int i = 0; i < 50; i++)
            {
                var resultGift = gifts[random.Next(gifts.Count)];
                var requiredGifts = gifts.OrderBy(x => random.Next()).Take(random.Next(2, 5)).ToList();

                recipes.Add(new Recipe
                {
                    Name = $"Fusion Recipe {i + 1}",
                    Description = $"Combine these gifts to create {resultGift.Name}",
                    ResultGift = resultGift,
                    RequiredGifts = requiredGifts,
                    Location = "Mirror Dungeon",
                    Difficulty = resultGift.Tier switch
                    {
                        4 or 5 => "Hard",
                        3 => "Medium",
                        _ => "Easy"
                    }
                });
            }

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }

        public static async Task CheckDatabaseStatusAsync()
        {
            try
            {
                using var context = new AppDbContext();
                var giftCount = await context.EgoGifts.CountAsync();
                var sourcesCount = await context.Sources.CountAsync();
                var recipesCount = await context.Recipes.CountAsync();

                Console.WriteLine($"Database status: {giftCount} gifts, {sourcesCount} sources, {recipesCount} recipes");

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