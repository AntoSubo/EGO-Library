using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EGO_Library.Services
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync()
        {
            using var context = new AppDbContext();

            // Создаем базу если не существует
            await context.Database.EnsureCreatedAsync();

            // Проверяем, есть ли уже данные
            if (await context.EgoGifts.AnyAsync())
                return;

            // Заполняем тестовыми данными
            var sampleGifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Wealth",
                    Tier = 4,
                    Status = "Charge",
                    Icon = "💰",
                    Description = "Increases max Charge by 2",
                    SourcesJson = JsonSerializer.Serialize(new List<string> { "Mirror Dungeon Floor 5", "Fusion" }),
                    FusionRecipesJson = JsonSerializer.Serialize(new List<string> { "A Certain Philosophy", "Wishing Cairn" })
                },
                new EgoGift
                {
                    Name = "Inferno",
                    Tier = 3,
                    Status = "Burn",
                    Icon = "🔥",
                    Description = "Applies Burn status each turn",
                    SourcesJson = JsonSerializer.Serialize(new List<string> { "Mirror Dungeon Floor 3" }),
                    FusionRecipesJson = JsonSerializer.Serialize(new List<string> { "Fire Walker", "Sun Spot" })
                }
            };

            context.EgoGifts.AddRange(sampleGifts);
            await context.SaveChangesAsync();
        }
    }
}