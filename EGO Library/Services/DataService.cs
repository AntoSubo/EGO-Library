using EGO_Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EGO_Library.Services
{
    public class DataService
    {
        public async Task<List<EgoGift>> LoadGiftsAsync()
        {
            // Заглушка - вернем тестовые данные
            return await Task.FromResult(new List<EgoGift>
            {
                new EgoGift
                {
                    Id = "1",
                    Name = "Wealth",
                    Tier = 4,
                    Status = "Charge",
                    Icon = "💰",
                    Description = "Increases max Charge by 2",
                    Sources = new List<string> { "Mirror Dungeon Floor 5", "Fusion" },
                    FusionRecipes = new List<string> { "A Certain Philosophy", "Wishing Cairn" }
                }
            });
        }
    }
}