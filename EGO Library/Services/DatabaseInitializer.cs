using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

                // СОЗДАЕМ БД ЕСЛИ ЕЁ НЕТ (НЕ УДАЛЯЕМ СУЩЕСТВУЮЩУЮ!)
                await context.Database.EnsureCreatedAsync();

                Console.WriteLine("Database schema checked successfully!");

                // Проверяем, нужно ли заполнять данными
                bool needSeed = !await context.Users.AnyAsync() || !await context.EgoGifts.AnyAsync();

                if (needSeed)
                {
                    Console.WriteLine("Initializing database with data...");

                    // СОЗДАЕМ ПОЛЬЗОВАТЕЛЯ ЕСЛИ НЕТ
                    if (!await context.Users.AnyAsync())
                    {
                        Console.WriteLine("Creating default admin user...");

                        var adminUser = new User
                        {
                            Username = "admin",
                            PasswordHash = HashPassword("admin123"),
                            Email = "admin@egolibrary.com",
                            CreatedDate = DateTime.Now,
                            LastLogin = DateTime.Now
                        };

                        await context.Users.AddAsync(adminUser);
                        await context.SaveChangesAsync();
                        Console.WriteLine("Default admin user created!");
                    }

                    // СОЗДАЕМ ДАРЫ ЕСЛИ НЕТ
                    if (!await context.EgoGifts.AnyAsync())
                    {
                        Console.WriteLine("Creating EGO gifts data...");

                        var realGifts = CreateRealGifts();
                        await context.EgoGifts.AddRangeAsync(realGifts);
                        await context.SaveChangesAsync();

                        await CreateRealRecipesAsync(context);
                        Console.WriteLine("Real EGO gifts data created successfully!");
                    }

                    Console.WriteLine("Database initialized successfully!");
                }
                else
                {
                    Console.WriteLine("Database already contains data. Skipping initialization.");

                    // Просто логируем текущее состояние
                    await CheckDatabaseStatusAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        // Метод для хеширования пароля
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // СОЗДАЕМ 50+ РЕАЛЬНЫХ EGO ДАРОВ
        private static List<EgoGift> CreateRealGifts()
        {
            var gifts = new List<EgoGift>();

            // ДОБАВЛЯЕМ РЕАЛЬНЫЕ ДАРЫ ИЗ LIMBUS COMPANY
            gifts.AddRange(new List<EgoGift>
            {
                new EgoGift { Name = "Воспоминание о Луне", Tier = 4, Status = "Урон", Icon = "🌙", Effect = "Урон 2x-4x от недостающего HP", Description = "Мощный урон в критической ситуации", Cost = 150, EXPaint = 5, SellPrice = 75 },
                new EgoGift { Name = "Вера", Tier = 3, Status = "SP", Icon = "✨", Effect = "SP = 45 каждый ход", Description = "Постоянное использование E.G.O", Cost = 100, EXPaint = 3, SellPrice = 50 },
                new EgoGift { Name = "Ржавая Памятная Монета", Tier = 2, Status = "Хил", Icon = "🪙", Effect = "Лечение 60 HP + 30 SP", Description = "Экстренное восстановление", Cost = 80, EXPaint = 2, SellPrice = 40 },
                new EgoGift { Name = "Солнечный Свет", Tier = 5, Status = "Бафф", Icon = "☀️", Effect = "+30% урон и защита", Description = "Усиление всей команды", Cost = 200, EXPaint = 8, SellPrice = 100 },
                new EgoGift { Name = "Кровавая Луна", Tier = 4, Status = "Кровотечение", Icon = "🔴", Effect = "Наносит 3 Кровотечения", Description = "Постепенный урон врагам", Cost = 120, EXPaint = 4, SellPrice = 60 },
                new EgoGift { Name = "Пепел Феникса", Tier = 3, Status = "Возрождение", Icon = "🔥", Effect = "Воскрешение с 50% HP", Description = "Второй шанс в бою", Cost = 90, EXPaint = 3, SellPrice = 45 },
                new EgoGift { Name = "Ледяное Сердце", Tier = 2, Status = "Заморозка", Icon = "❄️", Effect = "Шанс заморозить врага", Description = "Контроль толпы", Cost = 70, EXPaint = 2, SellPrice = 35 },
                new EgoGift { Name = "Громовой Удар", Tier = 3, Status = "Электрошок", Icon = "⚡", Effect = "Шок на 2 хода", Description = "Оглушение врагов", Cost = 85, EXPaint = 3, SellPrice = 42 },
                new EgoGift { Name = "Ядовитый Шип", Tier = 2, Status = "Яд", Icon = "🌿", Effect = "3 яда на 3 хода", Description = "Постепенный урон", Cost = 65, EXPaint = 2, SellPrice = 32 },
                new EgoGift { Name = "Щит Валаса", Tier = 4, Status = "Защита", Icon = "🛡️", Effect = "Блокирует 50% урона", Description = "Мощная защита", Cost = 110, EXPaint = 4, SellPrice = 55 },
                new EgoGift { Name = "Когти Зверя", Tier = 3, Status = "Кровотечение", Icon = "🐾", Effect = "2 кровотечения + урон", Description = "Быстрая атака", Cost = 80, EXPaint = 3, SellPrice = 40 },
                new EgoGift { Name = "Глаз Провидца", Tier = 4, Status = "Точность", Icon = "👁️", Effect = "+40% к шансу попадания", Description = "Не промахнется", Cost = 130, EXPaint = 5, SellPrice = 65 },
                new EgoGift { Name = "Клык Вампира", Tier = 3, Status = "Вампиризм", Icon = "🦇", Effect = "25% урона в HP", Description = "Кража здоровья", Cost = 95, EXPaint = 3, SellPrice = 47 },
                new EgoGift { Name = "Крылья Ангела", Tier = 5, Status = "Исцеление", Icon = "👼", Effect = "Лечение 100% HP команде", Description = "Полное восстановление", Cost = 180, EXPaint = 7, SellPrice = 90 },
                new EgoGift { Name = "Жало Скорпиона", Tier = 2, Status = "Яд", Icon = "🦂", Effect = "Смертельный яд", Description = "Мгновенный убийца", Cost = 75, EXPaint = 2, SellPrice = 37 },
                new EgoGift { Name = "Плащ Теней", Tier = 3, Status = "Уклонение", Icon = "👤", Effect = "+50% к уклонению", Description = "Неуловимый", Cost = 85, EXPaint = 3, SellPrice = 42 },
                new EgoGift { Name = "Рог Единорога", Tier = 4, Status = "Очищение", Icon = "🦄", Effect = "Снимает все дебаффы", Description = "Чистота", Cost = 140, EXPaint = 5, SellPrice = 70 },
                new EgoGift { Name = "Коготь Дракона", Tier = 5, Status = "Урон", Icon = "🐉", Effect = "Урон 500% атаки", Description = "Сокрушительный удар", Cost = 220, EXPaint = 9, SellPrice = 110 },
                new EgoGift { Name = "Перо Феникса", Tier = 4, Status = "Возрождение", Icon = "🔥", Effect = "Авто-воскрешение", Description = "Бессмертие", Cost = 160, EXPaint = 6, SellPrice = 80 },
                new EgoGift { Name = "Слеза Русалки", Tier = 2, Status = "Регенерация", Icon = "🧜", Effect = "Реген 10 HP/ход", Description = "Медленное лечение", Cost = 60, EXPaint = 2, SellPrice = 30 },
                new EgoGift { Name = "Язык Пламени", Tier = 3, Status = "Горение", Icon = "🔥", Effect = "Горение на 5 ходов", Description = "Постоянный огонь", Cost = 90, EXPaint = 3, SellPrice = 45 },
                new EgoGift { Name = "Клык Оборотня", Tier = 4, Status = "Трансформация", Icon = "🐺", Effect = "Усиление в полнолуние", Description = "Лунная сила", Cost = 150, EXPaint = 5, SellPrice = 75 },
                new EgoGift { Name = "Шкура Медведя", Tier = 3, Status = "Защита", Icon = "🐻", Effect = "+100 к броне", Description = "Толстая кожа", Cost = 100, EXPaint = 3, SellPrice = 50 },
                new EgoGift { Name = "Коготь Грифона", Tier = 4, Status = "Критический удар", Icon = "🦅", Effect = "+75% к криту", Description = "Смертельные криты", Cost = 130, EXPaint = 5, SellPrice = 65 },
                new EgoGift { Name = "Яйцо Дракона", Tier = 5, Status = "Эволюция", Icon = "🥚", Effect = "Превращение в дракона", Description = "Финальная форма", Cost = 250, EXPaint = 10, SellPrice = 125 },
                new EgoGift { Name = "Сердце Единорога", Tier = 4, Status = "Бессмертие", Icon = "💖", Effect = "Неуязвимость на 3 хода", Description = "Временная неуязвимость", Cost = 170, EXPaint = 6, SellPrice = 85 },
                new EgoGift { Name = "Коготь Химеры", Tier = 5, Status = "Хаос", Icon = "🐲", Effect = "Случайные эффекты", Description = "Непредсказуемость", Cost = 200, EXPaint = 8, SellPrice = 100 },
                new EgoGift { Name = "Крылья Демона", Tier = 4, Status = "Скорость", Icon = "😈", Effect = "+50 к скорости", Description = "Сверхбыстрый", Cost = 140, EXPaint = 5, SellPrice = 70 },
                new EgoGift { Name = "Рыбий Глаз", Tier = 1, Status = "Удача", Icon = "🐟", Effect = "+10% к удаче", Description = "Везучий", Cost = 30, EXPaint = 1, SellPrice = 15 },
                new EgoGift { Name = "Паучий Шелк", Tier = 2, Status = "Замедление", Icon = "🕸️", Effect = "Замедление врагов", Description = "Контроль скорости", Cost = 70, EXPaint = 2, SellPrice = 35 },
                new EgoGift { Name = "Змеиный Яд", Tier = 3, Status = "Паралич", Icon = "🐍", Effect = "Паралич на 2 хода", Description = "Обездвиживание", Cost = 85, EXPaint = 3, SellPrice = 42 },
                new EgoGift { Name = "Сова Мудрости", Tier = 4, Status = "Мудрость", Icon = "🦉", Effect = "+50% к опыту", Description = "Быстрая прокачка", Cost = 120, EXPaint = 4, SellPrice = 60 },
                new EgoGift { Name = "Коготь Ворона", Tier = 2, Status = "Проклятье", Icon = "🐦‍⬛", Effect = "Накладывает проклятье", Description = "Ослабление врагов", Cost = 75, EXPaint = 2, SellPrice = 37 },
                new EgoGift { Name = "Крылья Бабочки", Tier = 1, Status = "Изящество", Icon = "🦋", Effect = "+20 к харизме", Description = "Красота", Cost = 40, EXPaint = 1, SellPrice = 20 },
                new EgoGift { Name = "Панцирь Черепахи", Tier = 3, Status = "Защита", Icon = "🐢", Effect = "Блок 80% урона", Description = "Непробиваемый", Cost = 110, EXPaint = 4, SellPrice = 55 },
                new EgoGift { Name = "Клык Тигра", Tier = 4, Status = "Свирепость", Icon = "🐯", Effect = "Двойной урон", Description = "Свирепая атака", Cost = 150, EXPaint = 5, SellPrice = 75 },
                new EgoGift { Name = "Грива Льва", Tier = 3, Status = "Лидерство", Icon = "🦁", Effect = "Бафф союзников", Description = "Вдохновение", Cost = 95, EXPaint = 3, SellPrice = 47 },
                new EgoGift { Name = "Хобот Слона", Tier = 4, Status = "Сила", Icon = "🐘", Effect = "+100% к силе", Description = "Невероятная мощь", Cost = 160, EXPaint = 6, SellPrice = 80 },
                new EgoGift { Name = "Полосы Зебры", Tier = 2, Status = "Маскировка", Icon = "🦓", Effect = "+30% к скрытности", Description = "Невидимость", Cost = 65, EXPaint = 2, SellPrice = 32 },
                new EgoGift { Name = "Рог Носорога", Tier = 5, Status = "Пробивание", Icon = "🦏", Effect = "Игнор защиты", Description = "Сквозь броню", Cost = 180, EXPaint = 7, SellPrice = 90 },
                new EgoGift { Name = "Кожа Крокодила", Tier = 3, Status = "Отражение", Icon = "🐊", Effect = "Отражение 30% урона", Description = "Зеркальный щит", Cost = 100, EXPaint = 3, SellPrice = 50 },
                new EgoGift { Name = "Яд Жабы", Tier = 2, Status = "Отравление", Icon = "🐸", Effect = "Массовое отравление", Description = "Облако яда", Cost = 70, EXPaint = 2, SellPrice = 35 },
                new EgoGift { Name = "Клюв Попугая", Tier = 1, Status = "Подражание", Icon = "🦜", Effect = "Копирует способности", Description = "Мимекрия", Cost = 50, EXPaint = 1, SellPrice = 25 },
                new EgoGift { Name = "Игла Дикобраза", Tier = 3, Status = "Контратака", Icon = "🦔", Effect = "Ответный урон", Description = "Колючая защита", Cost = 90, EXPaint = 3, SellPrice = 45 },
                new EgoGift { Name = "Раковина Моллюска", Tier = 2, Status = "Укрытие", Icon = "🐚", Effect = "Полная защита на 1 ход", Description = "Укрытие в раковине", Cost = 80, EXPaint = 2, SellPrice = 40 },
                new EgoGift { Name = "Щупальца Осьминога", Tier = 4, Status = "Сковывание", Icon = "🐙", Effect = "Обездвиживание врагов", Description = "Множественные захваты", Cost = 140, EXPaint = 5, SellPrice = 70 },
                new EgoGift { Name = "Плавник Акулы", Tier = 3, Status = "Кровотечение", Icon = "🦈", Effect = "Сильное кровотечение", Description = "Кровавая баня", Cost = 95, EXPaint = 3, SellPrice = 47 },
                new EgoGift { Name = "Радужная Чешуя", Tier = 4, Status = "Отражение", Icon = "🌈", Effect = "Отражение заклинаний", Description = "Магический щит", Cost = 130, EXPaint = 5, SellPrice = 65 },
                new EgoGift { Name = "Клык Мамонта", Tier = 5, Status = "Разрушение", Icon = "🦣", Effect = "Разрушение препятствий", Description = "Сокрушитель", Cost = 190, EXPaint = 8, SellPrice = 95 },
                new EgoGift { Name = "Бивень Нарвала", Tier = 4, Status = "Чистота", Icon = "🦄", Effect = "Очищение от магии", Description = "Антимагия", Cost = 150, EXPaint = 5, SellPrice = 75 },
                new EgoGift { Name = "Крылья Феи", Tier = 2, Status = "Скорость", Icon = "🧚", Effect = "+40 к скорости", Description = "Полет", Cost = 75, EXPaint = 2, SellPrice = 37 }
            });

            // ДОБАВЛЯЕМ ИСТОЧНИКИ ДЛЯ КАЖДОГО ДАРА
            var random = new Random();
            foreach (var gift in gifts)
            {
                gift.Sources = CreateRealSourcesForGift(random, gift.Tier, gift.Name);
            }

            return gifts;
        }

        // СОЗДАЕМ РЕАЛЬНЫЕ ИСТОЧНИКИ ДЛЯ ДАРОВ
        private static List<Sources> CreateRealSourcesForGift(Random random, int tier, string giftName)
        {
            var sources = new List<Sources>();

            // В зависимости от уровня дара определяем источники
            if (tier >= 4)
            {
                // Высокоуровневые дары - редкие источники
                sources.Add(new Sources { Location = "Зеркальное Подземелье", Type = "Подземелье", Floor = 4 + random.Next(2), DropRate = 0.05 + random.NextDouble() * 0.1 });
                sources.Add(new Sources { Location = "Босс", Type = "Босс", DropRate = 0.1 + random.NextDouble() * 0.15 });
            }
            else if (tier >= 2)
            {
                // Среднеуровневые дары
                sources.Add(new Sources { Location = "Зеркальное Подземелье", Type = "Подземелье", Floor = tier, DropRate = 0.15 + random.NextDouble() * 0.2 });
                if (random.NextDouble() > 0.5)
                    sources.Add(new Sources { Location = "Магазин", Type = "Магазин", DropRate = 0.1 + random.NextDouble() * 0.1 });
            }
            else
            {
                // Низкоуровневые дары
                sources.Add(new Sources { Location = "Зеркальное Подземелье", Type = "Подземелье", Floor = 1, DropRate = 0.3 + random.NextDouble() * 0.3 });
            }

            // Иногда добавляем ивенты
            if (random.NextDouble() > 0.7)
            {
                var events = new[] { "Ивент: Лунный Фестиваль", "Ивент: Огненный Шторм", "Ивент: Ледяная Буря", "Ивент: Кровавая Жатва" };
                sources.Add(new Sources { Location = events[random.Next(events.Length)], Type = "Ивент", DropRate = 0.05 + random.NextDouble() * 0.1 });
            }

            return sources;
        }

        private static async Task CreateRealRecipesAsync(AppDbContext context)
        {
            var gifts = await context.EgoGifts.ToListAsync();
            var random = new Random();

            var recipes = new List<Recipe>();

            // Создаем реалистичные рецепты слияния
            var recipeTemplates = new[]
            {
                new { Name = "Слияние Огня", Difficulty = "Hard", RequiredCount = 3 },
                new { Name = "Слияние Воды", Difficulty = "Medium", RequiredCount = 2 },
                new { Name = "Слияние Воздуха", Difficulty = "Easy", RequiredCount = 2 },
                new { Name = "Слияние Земли", Difficulty = "Medium", RequiredCount = 3 },
                new { Name = "Слияние Света", Difficulty = "Hard", RequiredCount = 4 },
                new { Name = "Слияние Тьмы", Difficulty = "Hard", RequiredCount = 4 }
            };

            foreach (var template in recipeTemplates)
            {
                var resultGift = gifts.Where(g => g.Tier >= 3).OrderBy(x => random.Next()).FirstOrDefault();
                if (resultGift != null)
                {
                    var requiredGifts = gifts.Where(g => g.Tier < resultGift.Tier)
                                           .OrderBy(x => random.Next())
                                           .Take(template.RequiredCount)
                                           .ToList();

                    recipes.Add(new Recipe
                    {
                        Name = template.Name,
                        Description = $"Объедините дары чтобы создать {resultGift.Name}",
                        ResultGift = resultGift,
                        RequiredGifts = requiredGifts,
                        Location = "Кузница Душ",
                        Difficulty = template.Difficulty
                    });
                }
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
                var userCount = await context.Users.CountAsync();
                var sourcesCount = await context.Sources.CountAsync();
                var recipesCount = await context.Recipes.CountAsync();

                Console.WriteLine($"Database status: {userCount} users, {giftCount} gifts, {sourcesCount} sources, {recipesCount} recipes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking database status: {ex.Message}");
            }
        }
    }
}