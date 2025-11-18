using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EGO_Library.Services
{
    public class DataInitializer
    {
        private readonly AppDbContext _context;

        public DataInitializer(AppDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            // Создаем базу если не существует
            _context.Database.EnsureCreated();

            // Проверяем, есть ли уже данные
            if (_context.EgoGifts.Any())
                return;

            Console.WriteLine("Начало заполнения базы данных...");

            var gifts = GenerateEgoGifts();
            _context.EgoGifts.AddRange(gifts);
            _context.SaveChanges();

            Console.WriteLine($"База данных заполнена! Добавлено {gifts.Count} записей.");
        }

        private static List<EgoGift> GenerateEgoGifts()
        {
            var gifts = new List<EgoGift>();
            var random = new Random();

            // Базовые данные для генерации
            var statusEffects = new[]
            {
                "Burn", "Bleed", "Charge", "Poise", "Rupture", "Sinking", "Tremor", "Fragment",
                "Attack Power Up", "Defense Power Up", "Speed Up", "HP Recovery", "SP Recovery",
                "Critical Rate Up", "Damage Reduction", "Counter", "Pierce", "Blunt", "Slash"
            };

            var locations = new[]
            {
                "Mirror Dungeon", "Thread Spin", "Shop", "Event", "Boss Drop", "Fusion",
                "Exchange", "Season Pass", "Achievement", "Limited Time"
            };

            var icons = new[]
            {
                "💰", "🔥", "🛡️", "🎯", "❤️", "⚡", "🌪️", "💧", "🌑", "☀️", "⭐", "🎭",
                "⚔️", "🏹", "🔮", "💎", "🎲", "📜", "🔑", "🏆"
            };

            var namePrefixes = new[]
            {
                "Abyssal", "Ancient", "Arcane", "Blazing", "Celestial", "Chaotic", "Crimson", "Cursed",
                "Divine", "Eternal", "Frozen", "Golden", "Hallowed", "Infernal", "Lunar", "Mystic",
                "Radiant", "Shadow", "Solar", "Thundering", "Void", "Wailing", "Whispering"
            };

            var nameSuffixes = new[]
            {
                "Ambition", "Anguish", "Ashes", "Blade", "Blood", "Boon", "Breath", "Chains",
                "Claw", "Crown", "Desire", "Dream", "Ember", "Embrace", "End", "Flame",
                "Gaze", "Heart", "Hope", "Memory", "Oath", "Pain", "Promise", "Regret",
                "Scream", "Shadow", "Sigh", "Soul", "Tear", "Thorn", "Truth", "Will",
                "Wisdom", "Wound", "Yearning"
            };

            // Генерируем 600+ EGO gifts
            for (int i = 1; i <= 650; i++)
            {
                var name = $"{namePrefixes[random.Next(namePrefixes.Length)]} {nameSuffixes[random.Next(nameSuffixes.Length)]}";
                var tier = GetWeightedTier(random);
                var status = statusEffects[random.Next(statusEffects.Length)];
                var icon = icons[random.Next(icons.Length)];

                var gift = new EgoGift(name, tier, status, icon, GenerateDescription(name, tier, status, random))
                {
                    Sources = GenerateSources(locations, random, tier)
                };

                gifts.Add(gift);
            }

            return gifts;
        }

        private static int GetWeightedTier(Random random)
        {
            // Веса для tier: 1-40%, 2-30%, 3-15%, 4-10%, 5-5%
            var roll = random.Next(100);
            return roll switch
            {
                < 40 => 1,
                < 70 => 2,
                < 85 => 3,
                < 95 => 4,
                _ => 5
            };
        }

        private static string GenerateDescription(string name, int tier, string status, Random random)
        {
            var descriptions = new[]
            {
                $"The {name} grants its wielder enhanced capabilities in combat.",
                $"A manifestation of {name.ToLower()} that empowers the user.",
                $"This EGO gift channels the essence of {name.ToLower()}.",
                $"{name} embodies the concept of {status.ToLower()} in physical form.",
                $"Wielders of {name} find themselves transformed by its power.",
                $"An enigmatic artifact known as {name} with mysterious properties.",
                $"{name} resonates with those who understand its true nature.",
                $"The power of {name} can turn the tide of any battle."
            };

            var effects = new[]
            {
                $"Applies {status} status to enemies.",
                $"Increases {status} potency by {tier * 10}%.",
                $"Grants resistance to {status} effects.",
                $"Converts damage to {status} type.",
                $"Amplifies {status} related abilities.",
                $"Creates {status} fields around the user.",
                $"Stores {status} energy for powerful attacks."
            };

            return $"{descriptions[random.Next(descriptions.Length)]} {effects[random.Next(effects.Length)]}";
        }

        private static ObservableCollection<Sources> GenerateSources(string[] locations, Random random, int tier)
        {
            var sources = new ObservableCollection<Sources>();
            var sourceCount = random.Next(1, 4); // 1-3 источника

            for (int i = 0; i < sourceCount; i++)
            {
                var location = locations[random.Next(locations.Length)];
                var floor = location == "Mirror Dungeon" ? random.Next(1, 6) : 0;
                var dropRate = location == "Boss Drop" ? random.NextDouble() * 0.3 : 0;

                sources.Add(new Sources(location, GetSourceType(location), floor, dropRate));
            }

            return sources;
        }

        private static string GetSourceType(string location)
        {
            return location switch
            {
                "Mirror Dungeon" => "Dungeon",
                "Thread Spin" => "Crafting",
                "Shop" => "Purchase",
                "Event" => "Event",
                "Boss Drop" => "Drop",
                "Fusion" => "Crafting",
                "Exchange" => "Exchange",
                "Season Pass" => "Reward",
                "Achievement" => "Achievement",
                "Limited Time" => "Limited",
                _ => "Other"
            };
        }
    }
}