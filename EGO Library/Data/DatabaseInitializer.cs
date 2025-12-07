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

                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                var hasUsers = await context.Users.AnyAsync();
                var hasGifts = await context.EgoGifts.AnyAsync();
                var hasSources = await context.Sources.AnyAsync();
                var hasRecipes = await context.Recipes.AnyAsync();

                if (!hasUsers)
                {
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
                }

                if (!hasGifts)
                {
                    var gifts = CreateGifts();
                    await context.EgoGifts.AddRangeAsync(gifts);
                    await context.SaveChangesAsync();
                }

                if (!hasRecipes)
                {
                    await CreateRecipesAsync(context);
                }

                if (!hasSources)
                {
                    var giftsCount = await context.EgoGifts.CountAsync();
                    var sourcesCount = await context.Sources.CountAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static List<EgoGift> CreateGifts()
        {
            var gifts = new List<EgoGift>();

            gifts.AddRange(CreateTier1Gifts());
            gifts.AddRange(CreateTier2Gifts());
            gifts.AddRange(CreateTier3Gifts());
            gifts.AddRange(CreateTier4Gifts());
            gifts.AddRange(CreateTier5Gifts());

            return gifts;
        }

        private static async Task CreateRecipesAsync(AppDbContext context)
        {
            var recipesData = GetRecipesData();

            var allGifts = await context.EgoGifts.ToListAsync();
            var giftDictionary = allGifts.ToDictionary(g => g.Name, g => g);

            var recipesToAdd = new List<Recipes>();

            foreach (var recipeData in recipesData)
            {
                if (!giftDictionary.TryGetValue(recipeData.ResultGiftName, out var resultGift))
                {
                    continue;
                }

                var requiredGifts = new List<EgoGift>();
                bool allRequiredGiftsFound = true;

                foreach (var requiredGiftName in recipeData.RequiredGiftNames)
                {
                    if (giftDictionary.TryGetValue(requiredGiftName, out var requiredGift))
                    {
                        requiredGifts.Add(requiredGift);
                    }
                    else
                    {
                        allRequiredGiftsFound = false;
                        break;
                    }
                }

                if (!allRequiredGiftsFound)
                {
                    continue;
                }

                var recipe = new Recipes(
                    name: recipeData.Name,
                    description: recipeData.Description,
                    resultGift: resultGift,
                    requiredGifts: requiredGifts,
                    location: recipeData.Location
                );

                recipesToAdd.Add(recipe);
            }

            await context.Recipes.AddRangeAsync(recipesToAdd);
            await context.SaveChangesAsync();
        }

        private class RecipeData
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string ResultGiftName { get; set; } = string.Empty;
            public List<string> RequiredGiftNames { get; set; } = new List<string>();
        }

        private static List<RecipeData> GetRecipesData()
        {
            return new List<RecipeData>
            {
                new RecipeData
                {
                    Name = "Рецепт: Память о луне",
                    Description = "Создание легендарного EGO дара Память о луне из фрагментов памяти",
                    Location = "Рецепт создания",
                    ResultGiftName = "Память о луне",
                    RequiredGiftNames = new List<string>
                    {
                        "Разрубленная память",
                        "Проколотая память",
                        "Раздробленная память",
                        "Фрагмент греха",
                        "Фрагмент греха"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Оперативное снаряжение высокого риска",
                    Description = "Создание оперативного снаряжения высокого риска для экстремальных операций",
                    Location = "Рецепт создания",
                    ResultGiftName = "Оперативное снаряжение высокого риска",
                    RequiredGiftNames = new List<string>
                    {
                        "Стандартное оперативное снаряжение",
                        "Операционная пропускная карта"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Подарок",
                    Description = "Создание подарка с множеством случайных эффектов",
                    Location = "Рецепт создания",
                    ResultGiftName = "Подарок",
                    RequiredGiftNames = new List<string>
                    {
                        "Упаковочная коробка",
                        "Упаковочный бант"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Непреклонность",
                    Description = "Создание Непреклонности для команд с Родословной Клинка",
                    Location = "Рецепт создания",
                    ResultGiftName = "Непреклонность",
                    RequiredGiftNames = new List<string>
                    {
                        "Ржавая рукоять",
                        "Сломанный клинок"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Величественность",
                    Description = "Создание Величественности для команд с кланом Курокумо",
                    Location = "Рецепт создания",
                    ResultGiftName = "Величественность",
                    RequiredGiftNames = new List<string>
                    {
                        "Ржавая рукоять",
                        "Поврежденный клинок"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Культивация",
                    Description = "Создание Культивации для команд с Родословной Клинка",
                    Location = "Рецепт создания",
                    ResultGiftName = "Культивация",
                    RequiredGiftNames = new List<string>
                    {
                        "Изношенная рукоять",
                        "Сломанный клинок"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Вечные цепи связи",
                    Description = "Создание Вечных цепей связи для Среднего пальца",
                    Location = "Рецепт создания",
                    ResultGiftName = "Вечные цепи связи",
                    RequiredGiftNames = new List<string>
                    {
                        "Улуч. татуировки - средний палец",
                        "Цепи лояльности"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Сет снаряжения агента зачистки: тип C",
                    Description = "Создание полного сета снаряжения агента зачистки тип C для Корпорации W",
                    Location = "Рецепт создания",
                    ResultGiftName = "Сет снаряжения агента зачистки: тип C",
                    RequiredGiftNames = new List<string>
                    {
                        "Стандартная кепка корпорации W",
                        "Пространственный кинжал типа E",
                        "Портативная защитная батарея"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Сервоприводы вечного генератора",
                    Description = "Создание Сервоприводов вечного генератора для зарядных построек",
                    Location = "Рецепт создания",
                    ResultGiftName = "Сервоприводы вечного генератора",
                    RequiredGiftNames = new List<string>
                    {
                        "Кардиоваскулярный реактивный модуль",
                        "Сервоприводы протезных суставов"
                    }
                },
                new RecipeData
                {
                    Name = "Рецепт: Весёлая игрушка",
                    Description = "Создание Весёлой игрушки с комплексными эффектами",
                    Location = "Рецепт создания",
                    ResultGiftName = "Весёлая игрушка",
                    RequiredGiftNames = new List<string>
                    {
                        "Шапка с помпоном",
                        "Большой мешок с подарками",
                        "Грустная игрушка"
                    }
                }
            };
        }

        private static List<EgoGift> CreateTier1Gifts()
        {
            var gifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Фиксатор запястья",
                    Tier = 1,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier1/Фиксатор запястья.png",
                    Effect = "[Начало Хода] Союзника с 5+ (Счётчиками charge + 'Уникальным charge') получают 1 damageUp.",
                    Description = "Фиксатор запястья, который усиливает атакующие возможности при наличии заряда.",
                    Cost = 161,
                    SellPrice = 81,
                    Acquisition = "Магазин и Событие",
                    Keywords = "charge, damage, гордыня, атака, поддержка, усилитель, запястье",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Событие", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Кукла вуду",
                    Tier = 1,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier1/Кукла вуду.png",
                    Effect = "[Начало Хода] Наносит 3 фиксированного урона Завистью всем врагам.",
                    Description = "Мистическая кукла, наносящая урон и ослабляющая ослабленных врагов",
                    Cost = 149,
                    SellPrice = 75,
                    Acquisition = "Магазин и Событие: Ты будешь играть?",
                    Keywords = "зависть, powerDown, фиксированный урон",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Ты будешь играть?", Type = "Event", DropRate = 0.06 }
                    }
                },

                new EgoGift
                {
                    Name = "Источник бесперебойного питания",
                    Tier = 1,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier1/Источник бесперебойного питания.png",
                    Effect = "[Конец Хода] Если 3- Счётчика charge, получает +3 Счётчика charge в следующем Начале Хода.",
                    Description = "Стабилизирует энергопоток, усиливая заряд и урон в зависимости от текущего уровня заряда",
                    Cost = 156,
                    SellPrice = 78,
                    Acquisition = "Магазин и Событие: kqe-1j-23",
                    Keywords = "charge, damageUp, энергия",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 },
                        new Sources { Location = "Событие: kqe-1j-23", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Неразряжаемый дефибриллятор",
                    Tier = 1,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier1/Неразряжаемый дефибриллятор.png",
                    Effect = "[Конец Хода] Если 3- Счётчика charge, получает +3 Счётчика charge в следующем Начале Хода.",
                    Description = "Неразряжаемый дефибриллятор, который управляет зарядом и усиливает урон.",
                    Cost = 156,
                    SellPrice = 78,
                    Acquisition = "Магазин и Событие",
                    Keywords = "charge, damageUp, изолятор",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Событие", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Гадание на завтра",
                    Tier = 1,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier1/Гадание на завтра.png",
                    Effect = "Улучшает первую наградную карту Боя до самого высокого Ранга, который может быть на этаже.",
                    Description = "Гадание, которое предсказывает удачу и улучшает боевые награды.",
                    Cost = 140,
                    SellPrice = 70,
                    Acquisition = "Магазин и Событие: Звёздный светоч",
                    Keywords = "support, улучшение карт, награды",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Звёздный светоч", Type = "Event", DropRate = 0.06 }
                    }
                },

                new EgoGift
                {
                    Name = "Тернистый путь",
                    Tier = 1,
                    Status = "Утопание",
                    ImagePath = "Resources/Images/EGO/Tier1/Тернистый путь.png",
                    Effect = "При попадании и нанесении урона по HP врага Скиллом, что накладывает sinking или 'Уникальное sinking', накладывает 3 sinking и +2 Счётчика sinking в Конце Хода.",
                    Description = "Тернистый путь, усиливающий эффекты sinking и взаимодействующий с Унынием и Похотью.",
                    Cost = 160,
                    SellPrice = 80,
                    Acquisition = "Магазин и Событие: Престол небесного полководца",
                    Keywords = "sinking, уныние, похоть, надвигающаяся волна",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Престол небесного полководца", Type = "Event", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Гемакон",
                    Tier = 1,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier1/Гемакон.png",
                    Effect = "После нанесения урона врагу Скиллом, восстанавливает 12,5% от недостающих HP юнита (один раз на каждого юнита за Ход).",
                    Description = "Гемакон, обеспечивающий восстановление здоровья после атак, особенно эффективный с навыками Гнева.",
                    Cost = 142,
                    SellPrice = 71,
                    Acquisition = "Магазин и Событие: Нимфа",
                    Keywords = "heal, восстановление, гнев, хил",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Нимфа", Type = "Event", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Извращение",
                    Tier = 1,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier1/Извращение.png",
                    Effect = "После убийства 1 и более врага атакующим Скиллом, получите +1 ЭГО ресурс, соответствующий принадлежности этого Скилла, в следующем Начале Хода.",
                    Description = "Извращение, которое усиливает генерацию ЭГО ресурсов при убийствах, особенно для Гнева.",
                    Cost = 157,
                    SellPrice = 79,
                    Acquisition = "Магазин и Событие: Спираль презрения",
                    Keywords = "ЭГО ресурс, гнев, грехи, убийство",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Спираль презрения", Type = "Event", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Сверхядовитая кожа",
                    Tier = 1,
                    Status = "Тремор",
                    ImagePath = "Resources/Images/EGO/Tier1/Сверхядовитая кожа.png",
                    Effect = "При победе в Столкновении с помощью Скилла, накладывающего (включая на себя) tremor или Счётчик tremor, накладывает (количество оставшихся монет/2) tremor на цель.",
                    Description = "Сверхядовитая кожа, усиливающая тремор при победе в столкновении.",
                    Cost = 147,
                    SellPrice = 74,
                    Acquisition = "Магазин и Событие: Плачущая жабка",
                    Keywords = "tremor, победа в столкновении, монеты, яд, кожа",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Событие: Плачущая жабка", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Связка оберегов",
                    Tier = 1,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier1/Связка оберегов.png",
                    Effect = "После нанесения 12+ урона Скиллом, накладывает на цель 2 rupture.",
                    Description = "Связка оберегов, накладывающая разрыв при значительном уроне.",
                    Cost = 143,
                    SellPrice = 72,
                    Acquisition = "Магазин и Событие: Чтобы никто не плакал",
                    Keywords = "rupture, разрыв, рубящий урон, обереги",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 },
                        new Sources { Location = "Событие: Чтобы никто не плакал", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Вязка слизь",
                    Tier = 1,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier1/Вязка слизь.png",
                    Effect = "При попадании с помощью Пронзающего Скилла или Скилла принадлежности Чревоугодия, накладывает 2 defenseLevelDown.",
                    Description = "Вязкая слизь, ослабляющая защиту врагов.",
                    Cost = 156,
                    SellPrice = 78,
                    Acquisition = "Магазин и Событие: Ходячая жемчужина",
                    Keywords = "defenseLevelDown, пронзающий, чревоугодие, слизь, ослабление",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Ходячая жемчужина", Type = "Event", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Погашенный фонарь",
                    Tier = 1,
                    Status = "Тремор",
                    ImagePath = "Resources/Images/EGO/Tier1/Погашенный фонарь.png",
                    Effect = "[Эффекты применяются только к №7 размещённой Идентичности] Союзники получают на +1 больше Счётчика tremor от Скиллов или эффектов Монет.",
                    Description = "Погашенный фонарь, усиливающий тремор для седьмой позиции.",
                    Cost = 152,
                    SellPrice = 76,
                    Acquisition = "Тема 'Чудо в 20-м Районе БокГак' и Магазин",
                    Keywords = "tremor, позиция 7, оглушение, мораль, паника, фонарь",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе БокГак'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Связывающая нить",
                    Tier = 1,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier1/Связывающая нить.png",
                    Effect = "Когда союзник с наибольшей Скоростью наносит урон врагу с помощью Скилла, который накладывает bleed, Счётчик, или 'Уникальное bleed', наносит на +12.5% больше урона.",
                    Description = "Связывающая нить, усиливающая урон от кровотечения для самого быстрого союзника.",
                    Cost = 159,
                    SellPrice = 80,
                    Acquisition = "Магазин и Событие: Цветок госсипиума",
                    Keywords = "bleed, скорость, усиление урона, кровотечение, нить",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Событие: Цветок госсипиума", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Хирургический скальпель",
                    Tier = 1,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier1/Хирургический скальпель.png",
                    Effect = "[Эффект применяется только к №4 размещённой Идентичности] [Конец Хода] Идентичность с Режущими атакующими Скиллами получают (количество Режущих атакующих Скиллов, которые они имеют) haste в следующем Ходу.",
                    Description = "Хирургический скальпель, ускоряющий режущие атаки для четвёртой позиции.",
                    Cost = 155,
                    SellPrice = 78,
                    Acquisition = "Магазин",
                    Keywords = "haste, режущий урон, позиция 4, скальпель, скорость",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.16 }
                    }
                },

                new EgoGift
                {
                    Name = "Ржавый намордник",
                    Tier = 1,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier1/Ржавый намордник.png",
                    Effect = "Если союзник попадает и наносит 12+ HP урона по врагу за Скилл, накладывает 2 bleed.",
                    Description = "Ржавый намордник, накладывающий кровотечение при значительном уроне.",
                    Cost = 164,
                    SellPrice = 82,
                    Acquisition = "Магазин",
                    Keywords = "bleed, кровотечение, режущий урон, намордник",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 }
                    }
                },

                new EgoGift
                {
                    Name = "Лохмотья",
                    Tier = 1,
                    Status = "Утопание",
                    ImagePath = "Resources/Images/EGO/Tier1/Лохмотья.png",
                    Effect = "Союзники с Атакующими Скиллами, которые накладывают sinking, Счётчик, или 'Уникальное sinking', восстанавливают дополнительно 5 SP (один раз за Ход) при победе в Столкновении;",
                    Description = "Лохмотья, управляющие SP и усиливающие урон при отрицательном SP.",
                    Cost = 155,
                    SellPrice = 78,
                    Acquisition = "Магазин",
                    Keywords = "sinking, SP, отрицательные монеты, урон, лохмотья",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 }
                    }
                },

                new EgoGift
                {
                    Name = "Предрассудки",
                    Tier = 1,
                    Status = "Поддержка",
                    ImagePath = "Resources/Images/EGO/Tier1/Предрассудки.png",
                    Effect = "Первое Начало Хода в Бою: союзник с наименьшим HP восстанавливает 15% от его максимального HP.",
                    Description = "Предрассудки, обеспечивающие лечение наиболее раненому союзнику.",
                    Cost = 152,
                    SellPrice = 76,
                    Acquisition = "Магазин и Событие: Паук брака",
                    Keywords = "heal, восстановление, гордыня, лечение, предрассудки",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Событие: Паук брака", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Шапка с помпоном",
                    Tier = 1,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier1/Шапка с помпоном.png",
                    Effect = "[Конец Хода] Получает 1 poise на следующий ход за каждый атакующий Скилл, который нанес урон перед Концом Хода.",
                    Description = "Шапка с помпоном, генерирующая poise за успешные атаки.",
                    Cost = 150,
                    SellPrice = 75,
                    Acquisition = "Тема 'Чудо в 20-м Районе' и Магазин",
                    Keywords = "poise, дыхание, направляемый бой, шапка, помпон",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Фрагмент греха",
                    Tier = 1,
                    Status = "Компонент",
                    ImagePath = "Resources/Images/EGO/Tier1/Фрагмент греха.png",
                    Effect = "Базовый компонент для создания высших EGO gifts",
                    Description = "Фрагмент греха, необходимый для создания мощных EGO gifts.",
                    Acquisition = "Событие",
                    Keywords = "грех, материал, создание"
                },

                new EgoGift
                {
                    Name = "Поляризация волн",
                    Tier = 1,
                    Status = "Огонь",
                    ImagePath = "Resources/Images/EGO/Tier1/Поляризация волн.png",
                    Effect = "Если у союзника SP на 30+ больше, чем у цели, он наносит на +7,5% больше урона Скиллами, наносящими burn, Счётчик burn или 'Уникальный burn'.",
                    Description = "Поляризация волн, усиливающая огненные атаки при преимуществе в SP.",
                    Cost = 158,
                    SellPrice = 79,
                    Acquisition = "Магазин",
                    Keywords = "burn, огонь, SP, поляризация, волны",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 }
                    }
                },

                new EgoGift
                {
                    Name = "Памятная подвеска",
                    Tier = 1,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier1/Памятная подвеска.png",
                    Effect = "[Начало Фазы Сражения] Случайный союзник получает 3 poise.",
                    Description = "Памятная подвеска, распределяющая poise в начале фазы сражения.",
                    Cost = 145,
                    SellPrice = 73,
                    Acquisition = "Магазин и Событие: Проклятый стрелöк",
                    Keywords = "poise, дыхание, уныние, подвеска, приоритет",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Проклятый стрелöк", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Упаковочная коробка",
                    Tier = 1,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier1/Упаковочная коробка.png",
                    Effect = "(При Попадании Скиллами союзника) случайным образом выбирает один из следующих эффектов: burn, bleed, tremor, rupture, и sinking, затем накладывает 1 Значение выбранного эффекта (3 раза за ход).",
                    Description = "Упаковочная коробка, случайным образом накладывающая различные эффекты при попадании и убийствах.",
                    Cost = 151,
                    SellPrice = 76,
                    Acquisition = "Тема 'Чудо в 20-м Районе БокГак' и Магазин",
                    Keywords = "универсальный, случайные эффекты, burn, bleed, tremor, rupture, sinking, poise, charge, haste, усиления",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе БокГак'", Type = "Theme", DropRate = 0.08 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 }
                    }
                },

                new EgoGift
                {
                    Name = "Истёртый точильный камень",
                    Tier = 1,
                    Status = "Рубящий",
                    ImagePath = "Resources/Images/EGO/Tier1/Истёртый точильный камень.png",
                    Effect = "[Эффекты применяются только к №3 размещённой Идентичности] Рубящие Скиллы получают +(кол-во рубящих базовых атакующих Скиллов) Уровней Атаки.",
                    Description = "Истёртый точильный камень, усиливающий рубящие атаки для третьей позиции.",
                    Cost = 148,
                    SellPrice = 74,
                    Acquisition = "Магазин",
                    Keywords = "рубящий, позиция 3, уровень атаки, слабость, усиление урона, точильный камень",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 }
                    }
                },

                new EgoGift
                {
                    Name = "Декоративная подкова",
                    Tier = 1,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier1/Декоративная подкова.png",
                    Effect = "[Начало Хода] Накладывает 2 poise на случайного союзника без poise.",
                    Description = "Декоративная подкова, распределяющая poise между союзниками.",
                    Cost = 154,
                    SellPrice = 77,
                    Acquisition = "Магазин",
                    Keywords = "poise, дыхание, распределение, подкова, удача",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 }
                    }
                },

                new EgoGift
                {
                    Name = "Грязный гаечный ключ",
                    Tier = 1,
                    Status = "Тремор",
                    ImagePath = "Resources/Images/EGO/Tier1/Грязный гаечный ключ.png",
                    Effect = "Дробящие Скиллы наносят на +10% больше урона.",
                    Description = "Грязный гаечный ключ, усиливающий дробящие атаки и накладывающий тремор.",
                    Cost = 148,
                    SellPrice = 74,
                    Acquisition = "Тема 'МОРЕ' и Магазин",
                    Keywords = "tremor, дробящий урон, финальная монетка, гаечный ключ",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'МОРЕ'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Отклонение пандикона",
                    Tier = 1,
                    Status = "Тремор",
                    ImagePath = "Resources/Images/EGO/Tier1/Отклонение пандикона.png",
                    Effect = "Первое Начало Хода в Бою: накладывает 4 tremor и +4 Счётчика tremor на всех врагов (или на все Части Аномалий).",
                    Description = "Отклонение пандикона, массово накладывающее тремор в начале боя.",
                    Cost = 145,
                    SellPrice = 73,
                    Acquisition = "Магазин и Событие: Паровая транспортная машина",
                    Keywords = "tremor, зависть, начало боя, массовый эффект, пандикон",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Паровая транспортная машина", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Расплавленный парафин",
                    Tier = 1,
                    Status = "Огонь",
                    ImagePath = "Resources/Images/EGO/Tier1/Расплавленный парафин.png",
                    Effect = "При победе в Cтолкновении с помощью Скилла, наносящего burn, Счётчик burn или 'Уникальный burn', накладывает на цель (количество оставшихся Монеток / 2) burn.",
                    Description = "Расплавленный парафин, усиливающий горение при победе в столкновении.",
                    Cost = 150,
                    SellPrice = 75,
                    Acquisition = "Магазин и Событие: Пророк кожи",
                    Keywords = "burn, огонь, победа в столкновении, монеты, парафин",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Пророк кожи", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Скрижаль",
                    Tier = 1,
                    Status = "Поддержка",
                    ImagePath = "Resources/Images/EGO/Tier1/Скрижаль.png",
                    Effect = "Каждый раз, когда враг Оглушается, союзник с наименьшим HP восстанавливает 5% от своего максимального HP.",
                    Description = "Скрижаль, восстанавливающая здоровье при оглушении врагов.",
                    Cost = 165,
                    SellPrice = 83,
                    Acquisition = "Магазин и Событие: Писец небесного палача",
                    Keywords = "heal, восстановление, оглушение, чревоугодие, скрижаль",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 },
                        new Sources { Location = "Событие: Писец небесного палача", Type = "Event", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Ещё один пластырь",
                    Tier = 1,
                    Status = "Пронзающий",
                    ImagePath = "Resources/Images/EGO/Tier1/Ещё один пластырь.png",
                    Effect = "[Конец Хода] Cреди союзников с bleed, Идентичность с наименьшим HP восстанавливает 5% от ее макс. HP.",
                    Description = "Ещё один пластырь, восстанавливающий здоровье союзникам с кровотечением.",
                    Cost = 150,
                    SellPrice = 75,
                    Acquisition = "Магазин и Событие: Калечащий плюшевый мишка",
                    Keywords = "heal, bleed, восстановление, пластырь, кровотечение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 },
                        new Sources { Location = "Событие: Калечащий плюшевый мишка", Type = "Event", DropRate = 0.06 }
                    }
                },

                new EgoGift
                {
                    Name = "Высокоэластичная обувь из стали",
                    Tier = 1,
                    Status = "Пронзающий",
                    ImagePath = "Resources/Images/EGO/Tier1/Высокоэластичная обувь из стали.png",
                    Effect = "[Эффекты применяется только к №3 размещённым Идентичностям] Пронзающие Скиллы получают +(кол-во пронзающие базовых атакующих Скиллов) Уровень Атаки (одни и те же Скиллы не учитываются дважды; макс. 3).",
                    Description = "Высокоэластичная обувь из стали, усиливающая пронзающие атаки для третьей позиции.",
                    Cost = 148,
                    SellPrice = 74,
                    Acquisition = "Магазин",
                    Keywords = "пронзающий, позиция 3, уровень атаки, haste, усиление урона, обувь"
                },

                new EgoGift
                {
                    Name = "Безглавый портрет",
                    Tier = 1,
                    Status = "Утопание",
                    ImagePath = "Resources/Images/EGO/Tier1/Безглавый портрет.png",
                    Effect = "[Начало Фазы Сражения] Накладывается 2 Утопание на всех врагов с SP ниже 0.",
                    Description = "Безглавый портрет, связанный с утопанием.",
                    Cost = 151,
                    SellPrice = 76,
                    Acquisition = "Магазин и Событие: Портрет определенного дня",
                    Keywords = "sinking, утопание, портрет",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Портрет определенного дня", Type = "Event", DropRate = 0.08 }
                    }
                }
            };

            return gifts;
        }

        private static List<EgoGift> CreateTier2Gifts()
        {
            var gifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Флакон цельной крови",
                    Tier = 2,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier2/Флакон цельной крови.png",
                    Effect = "Активируется когда союзник с Атакующими Скиллами, которые накладывают bleed или 'Уникальное Кровотечение' умирает (один раз за волну): - Все союзники восстанавливают (9% от умершей Идентичности) HP и увеличивает bloodfeast на (уровень умершей Идентичности + 9)",
                    Description = "Флакон цельной крови, активирующий мощные эффекты при смерти союзника с кровотечением.",
                    Cost = 200,
                    SellPrice = 100,
                    Acquisition = "Магазин",
                    Keywords = "bleed, кровотечение, смерть союзника, восстановление",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 }
                    }
                },

                new EgoGift
                {
                    Name = "Стандартная кепка корпорации W",
                    Tier = 2,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier2/Стандартная кепка корпорации W.png",
                    Effect = "После использования Скилла, который получает Счётчик charge или 'Уникальный Заряд', получает emergencyBarrierBattery равную полученному Счётчику charge (макс. 7 на идентичность)",
                    Description = "Стандартная кепка корпорации W, преобразующая заряд в защитные барьеры.",
                    Cost = 204,
                    SellPrice = 102,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
                    Keywords = "charge, барьер, защита, корпорация W",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'", Type = "Dungeon", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                                new EgoGift
                {
                    Name = "Пестряк-кровожад",
                    Tier = 2,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier2/Пестряк-кровожад.png",
                    Effect = "После попадания по врагу Скиллом, накладывающим bleed или 'Уникальное bleed', накладывает 4 bleed",
                    Description = "Пестряк-кровожад, усиливающий наложение кровотечения на врагов.",
                    Cost = 202,
                    SellPrice = 101,
                    Acquisition = "Магазин и Событие: Плачущий гроб",
                    Keywords = "bleed, кровотечение, усиление эффектов",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 },
                        new Sources { Location = "Плачущий гроб", Type = "Event", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Разрез ангела",
                    Tier = 2,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier2/Разрез ангела.png",
                    Effect = "[Конец Хода] Когда у союзника больше 20 poise, указанный персонаж расходует 1 poise, чтобы получить +1 Счётчик poise",
                    Description = "Разрез ангела, управляющий системой poise",
                    Cost = 211,
                    SellPrice = 106,
                    Acquisition = "Магазин и Событие: Пьяница",
                    Keywords = "poise, дыхание, заряд, ангел",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Пьяница", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Анти-овечья заземляющая вилка",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Анти-овечья заземляющая вилка.png",
                    Effect = "При попадании во время фазы боя, накладывает 1 paralyze на атакующего следующий ход. (Один раз за ход для каждого персонажа)",
                    Description = "Заземляющая вилка, парализующая атакующих",
                    Cost = 203,
                    SellPrice = 102,
                    Acquisition = "Тема 'Путь 4 - Станция №3' и Магазин",
                    Keywords = "paralyze, паралич, защита, вилка",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Путь 4 - Станция №3'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Трепет",
                    Tier = 2,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier2/Трепет.png",
                    Effect = "[Эффект применяется только к №1, №2 размещённым Идентичностям] При использовании Скиллов, которые накладывают bleed, Счётчик bleed или 'Уникальное bleed', а также если основная цель имеет 7+ Счётчика bleed, наносит на +10% больше урона.",
                    Description = "Трепет, усиливающий кровотечение у первых двух идентичностей",
                    Cost = 207,
                    SellPrice = 104,
                    Acquisition = "Магазин и Событие: Спираль презрения",
                    Keywords = "bleed, кровотечение, усиление урона, позиция",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Спираль презрения", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Колючие силки",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Колючие силки.png",
                    Effect = "[Конец Хода] Накладывает на всех врагов rupture, равный их Скорости.",
                    Description = "Колючие силки, накладывающие разрыв на врагов",
                    Cost = 187,
                    SellPrice = 94,
                    Acquisition = "Магазин и Событие: Охотник за розами",
                    Keywords = "rupture, разрыв, похоть, скорость, силки",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 },
                        new Sources { Location = "Событие: Охотник за розами", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Биогенераторная батарея",
                    Tier = 2,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier2/Биогенераторная батарея.png",
                    Effect = "Союзники с Счётчиками charge получают 1 offenseLevelUp при каждом Столкновении. (макс. 6. Сбрасывается После Атаки или когда Счётчик charge полностью пропадает)",
                    Description = "Биогенераторная батарея, усиливающая зарядные атаки",
                    Cost = 203,
                    SellPrice = 102,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе' и Магазин",
                    Keywords = "charge, offenseLevelUp, haste, столкновение, батарея",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Чёрная тетрадь",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Чёрная тетрадь.png",
                    Effect = "Первое Начало Хода в Бою: даёт случайные ЭГО ресурсы в количестве, равное числу противников.",
                    Description = "Чёрная тетрадь, генерирующая ЭГО ресурсы",
                    Cost = 195,
                    SellPrice = 98,
                    Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости' и Магазин",
                    Keywords = "ЭГО ресурс, грехи, курокумо, клинок, генерация",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Уступаю плоть, дабы забрать их кости'", Type = "Theme", DropRate = 0.08 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Улуч. татуировки - средний палец",
                    Tier = 2,
                    Status = "Дробящий",
                    ImagePath = "Resources/Images/EGO/Tier2/Улуч. татуировки - средний палец.png",
                    Effect = "Дробящие Скиллы: +1 к Силе Столкновения. Идентичности Среднего пальца: получают +1 к Силе Столкновения и +1 к Силе Монетки вместо этого.",
                    Description = "Улучшенные татуировки для членов Среднего пальца.",
                    Cost = 200,
                    SellPrice = 100,
                    Acquisition = "Тема 'Чистка под покровом ночи' и Магазин",
                    Keywords = "средний палец, татуировки, дробящий, vengeanceMark",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чистка под покровом ночи'", Type = "Theme", DropRate = 0.12 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Доспех из чёрного железа",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Доспех из чёрного железа.png",
                    Effect = "[Начало Хода] Накладывает (Уровень Защиты / 3) Щита на 1 союзника с наибольшим Уровнем Защиты (В приоритете Идентичности «Хэйшоу» из стаи У; макс. Щита 15)",
                    Description = "Доспех из чёрного железа, обеспечивающий защиту и усиления",
                    Cost = 205,
                    SellPrice = 103,
                    Acquisition = "Тема 'Весенняя огранка' и Магазин",
                    Keywords = "щит, tremor, haste, defenseLevelUp, хэйшоу, защита",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Весенняя огранка'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Кроваво-красная грива",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Кроваво-красная грива.png",
                    Effect = "Эффект для всех Идентичностей с атакующими Скиллами принадлежности Гнева или Похоти.",
                    Description = "Кроваво-красная грива, адаптирующаяся к получению урона",
                    Cost = 203,
                    SellPrice = 102,
                    Acquisition = "Тема 'Вера и эрозия' и Магазин",
                    Keywords = "haste, defensePowerUp, bind, гнев, похоть, адаптация",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Вера и эрозия'", Type = "Theme", DropRate = 0.08 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Пот и кровь",
                    Tier = 2,
                    Status = "Тремор",
                    ImagePath = "Resources/Images/EGO/Tier2/Пот и кровь.png",
                    Effect = "Скиллы наносят на +10% больше урона по Оглушенным целям.",
                    Description = "Пот и кровь, усиливающий урон по оглушённым целям",
                    Cost = 200,
                    SellPrice = 100,
                    Acquisition = "Магазин и Событие: Пьяница",
                    Keywords = "tremor, оглушение, урон, леность, усиление",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Пьяница", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Меч кровавого пламени",
                    Tier = 2,
                    Status = "Огонь",
                    ImagePath = "Resources/Images/EGO/Tier2/Меч кровавого пламени.png",
                    Effect = "Первое Начало Хода в Бою: Идентичности с Базовыми Атакующими Скиллами, которые получают burn, получают 5 burn и восстанавливают 8 SP.",
                    Description = "Меч кровавого пламени, управляющий горением и здоровьем",
                    Cost = 201,
                    SellPrice = 101,
                    Acquisition = "Тема 'Весенняя огранка' и Магазин",
                    Keywords = "burn, SP, хэйшоу, здоровье, ограничение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Весенняя огранка'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Разбитые очки",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Разбитые очки.png",
                    Effect = "Когда союзник вступает в Столкновение против врага с большей Скоростью, получает +1 к Силе Столкновения.",
                    Description = "Разбитые очки, усиливающие столкновения с более быстрыми врагами",
                    Cost = 218,
                    SellPrice = 109,
                    Acquisition = "Тема 'Неизменный' и Магазин",
                    Keywords = "rupture, столкновение, скорость, offenseLevelUp",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Неизменный'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Искусство связывания в стиле дворецкого",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Искусство связывания в стиле дворецкого.png",
                    Effect = "[Начало Хода] Накладывает 1 bind на врага, который восстановился от Оглушения в прошлом ходу.",
                    Description = "Искусство связывания, контролирующее оглушённых врагов",
                    Cost = 148,
                    SellPrice = 74,
                    Acquisition = "Тема 'Душераздирающий' и Магазин",
                    Keywords = "bind, оглушение, урон, контроль, дворецкий",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Душераздирающий'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 }
                    }
                },

                new EgoGift
                {
                    Name = "Консервированное мороженое",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Консервированное мороженое.png",
                    Effect = "Когда союзник теряет HP из-за эффекта своего Скилла Атаки, исцеляет половину HP, потерянных из-за эффекта Скилла, в следующем Начале Хода. (округление вниз)",
                    Description = "Консервированное мороженое, восстанавливающее здоровье",
                    Cost = 195,
                    SellPrice = 98,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе' и Магазин",
                    Keywords = "heal, восстановление, HP, ЭГО, лечение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Кардиоваскулярный реактивный модуль",
                    Tier = 2,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier2/Кардиоваскулярный реактивный модуль.png",
                    Effect = "Когда союзник с Счётчиками charge проигрывает Столкновение, получает Щит равный Счётчикам charge. (один раз за ход)",
                    Description = "Кардиоваскулярный реактивный модуль, защищающий через заряд",
                    Cost = 197,
                    SellPrice = 99,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе' и Магазин",
                    Keywords = "charge, щит, HP, восстановление, многопрофильный офис",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Большой мешок с подарками",
                    Tier = 2,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier2/Большой мешок с подарками.png",
                    Effect = "[Начало Хода] Накладывает 2 poise, +2 Счётчика poise и 2 protection на союзника с наименьшим HP.",
                    Description = "Большой мешок с подарками, защищающий наиболее раненого союзника.",
                    Cost = 198,
                    SellPrice = 99,
                    Acquisition = "Тема 'Чудо в 20-м Районе' и Магазин",
                    Keywords = "большой мешок, подарки, poise, protection",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Кармилла",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Кармилла.png",
                    Effect = "В Начале Ненаправляемого Боя, все враги получают фиксированный урон, равный 20% от их максимального HP.",
                    Description = "Кармилла, наносящая фиксированный урон в начале боя",
                    Cost = 217,
                    SellPrice = 109,
                    Acquisition = "Магазин и Событие: Четыре сотни роз",
                    Keywords = "фиксированный урон, начало боя, ненаправляемый бой, кармилла",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Четыре сотни роз", Type = "Event", DropRate = 0.06 }
                    }
                },

                new EgoGift
                {
                    Name = "Ребёнок в колбе",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Ребёнок в колбе.png",
                    Effect = "[Начало Боя] Добавляет +1 ЭГО ресурс к каждому Греху.",
                    Description = "Ребёнок в колбе, генерирующий ЭГО ресурсы в начале боя",
                    Cost = 212,
                    SellPrice = 106,
                    Acquisition = "Магазин и Событие: Дети в стеклянном флаконе",
                    Keywords = "ЭГО ресурс, грехи, начало боя, генерация, колба",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Дети в стеклянном флаконе", Type = "Event", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Мундштук",
                    Tier = 2,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier2/Мундштук.png",
                    Effect = "[Начало Хода] Союзники с poise получают 1 attackPowerUp.",
                    Description = "Мундштук, усиливающий атаку союзников с poise.",
                    Cost = 197,
                    SellPrice = 99,
                    Acquisition = "Магазин",
                    Keywords = "poise, attackPowerUp, усиление атаки",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 }
                    }
                },

                new EgoGift
                {
                    Name = "Кофе с журавликами",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Кофе с журавликами.png",
                    Effect = "[Начало Хода] Добавляет +1 ЭГО ресурс к случайному Греху, который не использовался в прошлом Ходу.",
                    Description = "Кофе с журавликами, усиливающий генерацию ЭГО ресурсов.",
                    Cost = 186,
                    SellPrice = 93,
                    Acquisition = "Магазин и Событие: Бумажный председатель",
                    Keywords = "ЭГО ресурс, похоть, грехи, генерация",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Бумажный председатель", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Давящие бинты",
                    Tier = 2,
                    Status = "Дробящий",
                    ImagePath = "Resources/Images/EGO/Tier2/Давящие бинты.png",
                    Effect = "[Эффект применяется только к №5 размещённой Идентичности] При попадании по врагу Дробящим Скиллом, наносит 3 бонусного Дробящего урона.",
                    Description = "Давящие бинты, усиливающие дробящие атаки и обеспечивающие восстановление.",
                    Cost = 209,
                    SellPrice = 105,
                    Acquisition = "Магазин",
                    Keywords = "дробящий урон, уныние, восстановление, бинты",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 }
                    }
                },

                new EgoGift
                {
                    Name = "Запачканная игла с нитью",
                    Tier = 2,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier2/Запачканная игла с нитью.png",
                    Effect = "Следующий эффект применяется к первому Скиллу, который наносит урон врагу. Накладывает (нанесенный урон/3) bleed.",
                    Description = "Запачканная игла с нитью, накладывающая кровотечение на врагов.",
                    Cost = 208,
                    SellPrice = 104,
                    Acquisition = "Тема 'Адская куря' и Магазин",
                    Keywords = "bleed, кровотечение, игла, нить",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Адская куря'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Взор презрения",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Взор презрения.png",
                    Effect = "[Эффекты применяются только к №6 размещённой Идентичности] Начало Фазы Боя: наносит на +10% больше урона за каждый вражеский атакующий Скилл, который нацелен на этого юнита (макс. 20%).",
                    Description = "Взор презрения, контратакующий множественные атаки врагов.",
                    Cost = 208,
                    SellPrice = 104,
                    Acquisition = "Тема 'Путь 3' и Магазин",
                    Keywords = "контратака, фиксированный урон, SP, презрение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Путь 3'", Type = "Theme", DropRate = 0.11 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Кристаллизованная кровь",
                    Tier = 2,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier2/Кристаллизованная кровь.png",
                    Effect = "[Конец Хода] Если на этого юнита наложено bleed, уменьшает bleed на себе на половину и восстанавливает (уменьшенное bleed х Счётчик bleed на себе) HP.",
                    Description = "Кристаллизованная кровь, преобразующая кровотечение в лечение.",
                    Cost = 195,
                    SellPrice = 98,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе' и Магазин",
                    Keywords = "bleed, восстановление, кровь, кристалл",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе'", Type = "Theme", DropRate = 0.12 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.07 }
                    }
                },

                new EgoGift
                {
                    Name = "Неугасаемый очаг",
                    Tier = 2,
                    Status = "Огонь",
                    ImagePath = "Resources/Images/EGO/Tier2/Неугасаемый очаг.png",
                    Effect = "[Конец Хода] Враги с 3+ Счётчиками burn активируют burn еще раз (уменьшает Счётчик burn на 1)",
                    Description = "Неугасаемый очаг, усиливающий эффекты горения на врагах.",
                    Cost = 201,
                    SellPrice = 101,
                    Acquisition = "Магазин и Событие: Базилисуп",
                    Keywords = "burn, огонь, очаг, горение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 },
                        new Sources { Location = "Событие: Базилисуп", Type = "Event", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Десятитысячилетний кипящий котёл",
                    Tier = 2,
                    Status = "Огонь",
                    ImagePath = "Resources/Images/EGO/Tier2/Десятитысячилетний кипящий котёл.png",
                    Effect = "Скиллы, которые накладывают burn, Счётчик burn, или 'Уникальный burn', получают +1 к Силе Столкновения.",
                    Description = "Древний котёл, усиливающий силу столкновения огненных навыков.",
                    Cost = 208,
                    SellPrice = 104,
                    Acquisition = "Магазин и Событие: Базилисуп",
                    Keywords = "burn, сила столкновения, котёл, огонь",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 },
                        new Sources { Location = "Событие: Базилисуп", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Пожирающий куб",
                    Tier = 2,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier2/Пожирающий куб.png",
                    Effect = "Когда союзник умирает, получает 1 ресурс Э.Г.О. за каждый из Навыков 1 и 2 этого союзника.",
                    Description = "Пожирающий куб, поглощающий энергию смерти и убийств.",
                    Cost = 195,
                    SellPrice = 98,
                    Acquisition = "Тема 'Бесконечное шествие' и Магазин",
                    Keywords = "ЭГО ресурс, смерть, убийство, куб",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Бесконечное шествие'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.06 }
                    }
                }
            };

            return gifts;
        }

        private static List<EgoGift> CreateTier3Gifts()
        {
            var gifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Голубые осколки звезды",
                    Tier = 3,
                    Status = "Утопание",
                    ImagePath = "Resources/Images/EGO/Tier3/Голубые осколки звезды.png",
                    Effect = "Противники в состоянии Низкой Морали или Паники получают -1 к Итоговому Значению Cкилла.",
                    Description = "ЭГО дар, специализирующийся на ослаблении противников с низкой моралью и паникой.",
                    Cost = 246,
                    SellPrice = 123,
                    Acquisition = "Магазин",
                    Keywords = "sinking, defenseLevelDown, мораль, паника",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.15 }
                    }
                },

                new EgoGift
                {
                    Name = "Глефа из кристаллизованной крови",
                    Tier = 3,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier3/Глефа из кристаллизованной крови.png",
                    Effect = "Эффекты усиливаются в зависимости от количества Идентичностей с Атакующими Скиллами, которые расходуют bloodfeast.",
                    Description = "Оружие, созданное из кристаллизованной крови, усиливается в кровавых битвах.",
                    Cost = 248,
                    SellPrice = 124,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
                    Keywords = "bloodfeast, hardbloodCast, кровь, идентичности",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'", Type = "Theme", DropRate = 0.1 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 }
                    }
                },

                new EgoGift
                {
                    Name = "Плоть из крови, кровь из плоти",
                    Tier = 3,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier3/Плоть из крови, кровь из плоти.png",
                    Effect = "Усиливается в зависимости от Идентичностей с bloodfeast.",
                    Description = "Симбиотический ЭГО дар, превращающий кровь в оружие и защиту.",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
                    Keywords = "bloodfeast, bleed, кровотечение, симбиоз",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'", Type = "Theme", DropRate = 0.08 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Брошенная пространственная рукавица",
                    Tier = 3,
                    Status = "Заряд",
                    ImagePath = "Resources/Images/EGO/Tier3/Брошенная пространственная рукавица.png",
                    Effect = "При нанесение урона противникам с помощью Скиллов, которые расходуют Счётчик charge или 'Уникальный Заряд', сопротивление к наименьшему типу атаки увеличивается на +0,1 на этот Ход (один раз за ход)",
                    Description = "Пространственный ЭГО дар, усиливающий атаки с зарядом.",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
                    Keywords = "charge, сопротивление, пространственный",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Сгущающийся запах крови",
                    Tier = 3,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier3/Сгущающийся запах крови.png",
                    Effect = "Союзник с наименьшим HP восстанавливает 3% HP при попадании Атакующими Скиллами, которые накладывают bleed (2 раза за ход).",
                    Description = "ЭГО дар, усиливающий команды, специализирующиеся на кровотечении.",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Магазин",
                    Keywords = "bleed, столкновение, лечение, сила",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.14 }
                    }
                },

                new EgoGift
                {
                    Name = "Грустная игрушка",
                    Tier = 3,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier3/Грустная игрушка.png",
                    Effect = "Наносит на +(абсолютное значение SP/1.5)% больше урона. (минимум 0%).",
                    Description = "Грустная игрушка, взаимодействующая с SP и монетками.",
                    Cost = 250,
                    SellPrice = 125,
                    Acquisition = "Тема 'Чудо в 20-м Районе' и Магазин",
                    Keywords = "грустная игрушка, SP, положительные монетки, отрицательные монетки",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе'", Type = "Theme", DropRate = 0.09 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },

                new EgoGift
                {
                    Name = "Изношенная рукоять",
                    Tier = 3,
                    Status = "Разрыв",
                    ImagePath = "Resources/Images/EGO/Tier3/Изношенная рукоять.png",
                    Effect = "[Начало Хода] Все союзники получают 1 lustPowerUp.",
                    Description = "Древняя рукоять, усиливающая владельцев мечей и членов клана Курокумо.",
                    Cost = 248,
                    SellPrice = 124,
                    Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости БокГак' и Магазин",
                    Keywords = "lustPowerUp, slashPowerUp, клинок, курокумо",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Уступаю плоть, дабы забрать их кости БокГак'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.13 }
                    }
                },

                new EgoGift
                {
                    Name = "Стандартное оперативное снаряжение",
                    Tier = 3,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier3/Стандартное оперативное снаряжение.png",
                    Effect = "Идентичности Компании «Лимбус»: Скилл 2 получает +1 Уровень Атаки и наносит на +15% больше урона; Скилл 3 получает +2 Уровня Атаки и наносит на +30% больше урона.",
                    Description = "Стандартное оперативное снаряжение для идентичностей Компании Лимбус.",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Тема 'LCB: Регулярный осмотр' и Магазин",
                    Keywords = "лимбус, оперативное снаряжение, усиление скиллов, резерв",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'LCB: Регулярный осмотр'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Операционная пропускная карта",
                    Tier = 3,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier3/Операционная пропускная карта.png",
                    Effect = "Во время Боя, когда союзник отступает или побеждён, случайным образом применяет 3 offenseLevelUp между всеми Идентичностями в Начале следующего Хода (2 раза за ход).",
                    Description = "Операционная пропускная карта, усиляющая команду при потерях.",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Тема 'LCB: Регулярный осмотр' и Магазин",
                    Keywords = "операционная карта, потеря союзников, усиление, SP восстановление",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'LCB: Регулярный осмотр'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },

                new EgoGift
                {
                    Name = "Упаковочный бант",
                    Tier = 3,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier3/Упаковочный бант.png",
                    Effect = @"[Начало Хода] Случайным образом выбирает 1 из эффектов: wrathDamageUp, lustDamageUp, slothDamageUp, glutDamageUp, gloomDamageUp, prideDamageUp, envyDamageUp, и применяет 1 раз на всех союзников
[Начало Хода] Случайным образом выбирает 1 из эффектов: wrathFragility, lustFragility, slothFragility, glutFragility, gloomFragility, prideFragility, envyFragility, и применяет 1 раз на всех противников",
                    Description = "Упаковочный бант, случайным образом усиляющий союзников и ослабляющий врагов.",
                    Cost = 249,
                    SellPrice = 125,
                    Acquisition = "Тема 'Чудо в 20-м Районе БокГак' и Магазин",
                    Keywords = "упаковочный бант, случайные усиления, случайные ослабления",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чудо в 20-м Районе БокГак'", Type = "Theme", DropRate = 0.06 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.09 }
                    }
                },

                new EgoGift
                {
                    Name = "Цепи лояльности",
                    Tier = 3,
                    Status = "Дробящий",
                    ImagePath = "Resources/Images/EGO/Tier3/Цепи лояльности.png",
                    Effect = "При получение удара, все союзники в той же фракции наносят на +5% больше Дробящего урона (15% за ход).",
                    Description = "Цепи лояльности, усиляющие дробящий урон при получении урона.",
                    Cost = 253,
                    SellPrice = 127,
                    Acquisition = "Тема 'Чистка под покровом ночи' и Магазин",
                    Keywords = "цепи лояльности, дробящий, средний палец, фракция",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Чистка под покровом ночи'", Type = "Theme", DropRate = 0.12 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Ржавая рукоять",
                    Tier = 3,
                    Status = "Рубящий",
                    ImagePath = "Resources/Images/EGO/Tier3/Ржавая рукоять.png",
                    Effect = @"[Начало Хода] Все союзники получают 1 slashPowerUp.
                    Идентичности Родословной Клинка или Клана Курокумо получают 2 slashPowerUp вместо предыдущего эффекта.",
                    Description = "Древняя рукоять, усиляющая рубящие атаки.",
                    Cost = 235,
                    SellPrice = 118,
                    Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости' и Магазин",
                    Keywords = "ржавая рукоять, рубящий, родословная клинка, курокумо",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Уступаю плоть, дабы забрать их кости'", Type = "Theme", DropRate = 0.08 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.11 }
                    }
                },
                

                new EgoGift
                {
                    Name = "Сломанный клинок",
                    Tier = 3,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier3/Сломанный клинок.png",
                    Effect = @"[Начало Хода] Все союзники получают 1 slashPowerUp.
                    Идентичности Родословной Клинка или Клана Курокумо получают 2 slashPowerUp вместо предыдущего эффекта.",
                    Description = "Сломанный клинок, усиляющий критические удары.",
                    Cost = 244,
                    SellPrice = 122,
                    Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости' и Магазин",
                    Keywords = "сломанный клинок, критические удары, poise, родословная клинка",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Уступаю плоть, дабы забрать их кости'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                },
                

                new EgoGift
                {
                    Name = "Слепой дождь",
                    Tier = 3,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier3/Слепой дождь.png",
                    Effect = "При активации А-Рез. Лености или использовании Скилла, имеющего 2 Веса Атаки: Все союзники получают +2 к Итоговому Значению Cкилла в Начале Фазы Боя.",
                    Description = "Слепой дождь",
                    Cost = 252,
                    SellPrice = 126,
                    Acquisition = "Магазин и Событие: Бродячий лис",
                    Keywords = "универсальный, леность, итоговое значение, фаза боя",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.12 },
                        new Sources { Location = "Событие: Бродячий лис", Type = "Event", DropRate = 0.08 }
                    }
                },

                new EgoGift
                {
                    Name = "Поврежденный клинок",
                    Tier = 3,
                    Status = "Кровотечение",
                    ImagePath = "Resources/Images/EGO/Tier3/Поврежденный клинок.png",
                    Effect = "На протяжении Боя: при накладывании bleed или 'Уникальное bleed' в первый раз против врага с помощью атакующего Скилла, накладывает дополнительно +2 Счётчика bleed. (Один раз за Бой).",
                    Description = "Поврежденный клинок, усиляющий кровотечение.",
                    Cost = 244,
                    SellPrice = 122,
                    Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости' и Магазин",
                    Keywords = "поврежденный клинок, bleed, курокумо, кровотечение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Тема 'Уступаю плоть, дабы забрать их кости'", Type = "Theme", DropRate = 0.07 },
                        new Sources { Location = "Магазин", Type = "Shop", DropRate = 0.1 }
                    }
                }
            };

            return gifts;
        }

        private static List<EgoGift> CreateTier4Gifts()
        {
            var gifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Непреклонность",
                    Tier = 4,
                    Status = "Рубящий",
                    ImagePath = "Resources/Images/EGO/Tier4/Непреклонность.png",
                    Effect = @"[Начало Боя] Активируется при наличии 3 или более Идентичностей Родословной Клинка.

                    [Начало Хода] Все союзники получают 2 slashPowerUp и наносят на +35% больше режущего урона.

                    [Начало Хода] Идентичность Родословной Клинка наносящий врагу критический удар, наносит дополнительный урон Гордыней, равный poise на себе.",
                    Description = "Непреклонность, усиляющая команду с Родословной Клинка.",
                    SellPrice = 200,
                    Acquisition = "Рецепт: Ржавая рукоять + Сломанный клинок",
                    Keywords = "непреклонность, родословная клинка, рубящий, критические удары, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Ржавая рукоять + Сломанный клинок" }
                    }
                },

                new EgoGift
                {
                    Name = "Величественность",
                    Tier = 4,
                    Status = "Рубящий",
                    ImagePath = "Resources/Images/EGO/Tier4/Величественность.png",
                    Effect = "[Начало Боя] Активируется при наличии 3 или более Идентичностей клана «Курокумо».",
                    Description = "Величественность, усиляющая команду с кланом Курокумо.",
                    SellPrice = 200,
                    Acquisition = "Рецепт: Ржавая рукоять + Поврежденный клинок",
                    Keywords = "величественность, курокумо, рубящий, bleed, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Ржавая рукоять + Поврежденный клинок" }
                    }
                },

                new EgoGift
                {
                    Name = "Культивация",
                    Tier = 4,
                    Status = "Рубящий",
                    ImagePath = "Resources/Images/EGO/Tier4/Культивация.png",
                    Effect = "[Начало Хода] Активируется, когда на поле есть 3 или более Идентичности Родословной Клинка.",
                    Description = "Культивация, усиляющая команду с Родословной Клинка через poise.",
                    Cost = 0,
                    SellPrice = 194,
                    Acquisition = "Рецепт: Изношенная рукоять + Сломанный клинок",
                    Keywords = "культивация, родословная клинка, рубящий, poise, критические удары, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Изношенная рукоять + Сломанный клинок", Type = "Crafting", DropRate = 1.0 }
                    }
                },

                new EgoGift
                {
                    Name = "Вечные цепи связи",
                    Tier = 4,
                    Status = "Дробящий",
                    ImagePath = "Resources/Images/EGO/Tier4/Вечные цепи связи.png",
                    Effect = "[Начало Боя] Активируется, когда есть 2 или более Идентичности Среднего пальца (учитываются только Идентичности на поле в момент начала Сражения).",
                    Description = "Вечные цепи связи, мощно усиляющие Средний палец.",
                    Cost = 0,
                    SellPrice = 201,
                    Acquisition = "Рецепт: Улуч. татуировки - средний палец + Цепи лояльности",
                    Keywords = "вечные цепи связи, средний палец, дробящий, фракция, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Улуч. татуировки - средний палец + Цепи лояльности", Type = "Crafting", DropRate = 1.0 }
                    }
                },

                new EgoGift
                {
                    Name = "Разрубленная память",
                    Tier = 4,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier4/Разрубленная память.png",
                    Effect = "Эффект разрубленной памяти",
                    Description = "Фрагмент памяти",
                    Cost = 300,
                    SellPrice = 150,
                    Acquisition = "Рецепт",
                    Keywords = "память, фрагмент, материал"
                },

                new EgoGift
                {
                    Name = "Проколотая память",
                    Tier = 4,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier4/Проколотая память.png",
                    Effect = "Эффект проколотой памяти",
                    Description = "Фрагмент памяти",
                    Cost = 300,
                    SellPrice = 150,
                    Acquisition = "Рецепт",
                    Keywords = "память, фрагмент, материал"
                },

                new EgoGift
                {
                    Name = "Раздробленная память",
                    Tier = 4,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier4/Раздробленная память.png",
                    Effect = "Эффект раздробленной памяти",
                    Description = "Фрагмент памяти",
                    Cost = 300,
                    SellPrice = 150,
                    Acquisition = "Рецепт",
                    Keywords = "память, фрагмент, материал"
                },

                new EgoGift
                {
                    Name = "Оперативное снаряжение высокого риска",
                    Tier = 4,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier4/Оперативное снаряжение высокого риска.png",
                    Effect = "Идентичности Компании «Лимбус»: Скилл 2 получает +2 Уровня Атаки и наносит на +20% больше урона; Скилл 3 получает +3 Уровня Атаки и наносит на +40% больше урона.",
                    Description = "Оперативное снаряжение высокого риска для экстремальных операций.",
                    Keywords = "лимбус, высокий риск, усиление скиллов, потеря союзников, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Стандартное оперативное снаряжение + Операционная пропускная карта" }
                    }
                },

                new EgoGift
                {
                    Name = "Подарок",
                    Tier = 4,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier4/Подарок.png",
                    Effect = @"При Попадании Скиллами союзника, случайным образом выбирает 2 из следующих эффектов: burn, bleed, tremor, rupture, и sinking, затем накладывает 1 Значение выбранного эффекта (3 раза за ход)
                    При Попадании Скиллами союзника, случайным образом выбирает один из следующих эффектов: Счётчик poise, Счётчик charge, haste, offenseLevelUp, и defenseLevelUp, затем накладывает 1 выбранный эффект на 2 случайных союзников (3 раза за ход)

                    [Начало Хода] Случайным образом выбирает 2 эффекта из этих эффектов: wrathDamageUp, lustDamageUp, slothDamageUp, glutDamageUp, gloomDamageUp, prideDamageUp, envyDamageUp, и применяет их 1 раз на всех союзников

                    [Начало Хода] Случайным образом выбирает 2 эффекта из этих эффектов: wrathFragility, lustFragility, slothFragility, glutFragility, gloomFragility, prideFragility, envyFragility, и применяет их 1 раз на всех противников",
                    Description = "Подарок, дарующий множество случайных эффектов.",
                    SellPrice = 199,
                    Keywords = "подарок, случайные эффекты, усиления, ослабления, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Упаковочная коробка + Упаковочный бант" }
                    }
                },

                new EgoGift
                {
                    Name = "Весёлая игрушка",
                    Tier = 4,
                    Status = "Дыхание",
                    ImagePath = "Resources/Images/EGO/Tier4/Весёлая игрушка.png",
                    Effect = "Получает 2 poise на следующий ход, основываясь на числе союзных атакующих Скиллов, которые наносили урон до Конца Хода.",
                    Description = "Весёлая игрушка, комплексно взаимодействующая с poise, SP и монетками.",
                    Cost = 0,
                    SellPrice = 200,
                    Acquisition = "Рецепт: Шапка с помпоном + Большой мешок с подарками + Грустная игрушка",
                    Keywords = "весёлая игрушка, poise, SP, монетки, защита, рецепт",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Рецепт: Шапка с помпоном + Большой мешок с подарками + Грустная игрушка", Type = "Crafting", DropRate = 1.0 }
                    }
                }
            };

            return gifts;
        }

        private static List<EgoGift> CreateTier5Gifts()
        {
            var gifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Лунный остаток",
                    Tier = 5,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier5/Лунный остаток.png",
                    Effect = "Считается ЭГО Даром 5 Ранга. Может быть продан в Магазине или использован для Объединения.",
                    Description = "Лунный остаток, используемый для создания других ЭГО даров",
                    Cost = 500,
                    SellPrice = 250,
                    Acquisition = "Дубликат ЭГО Дара V Ранга",
                    Keywords = "лунный остаток, материал, создание, объединение",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Дубликат ЭГО Дара V Ранга", Type = "Duplicate", DropRate = 0.05 }
                    }
                },

                new EgoGift
                {
                    Name = "Память о луне",
                    Tier = 5,
                    Status = "Универсальный",
                    ImagePath = "Resources/Images/EGO/Tier5/Память о луне.png",
                    Effect = "Все типы сопротивления врагов (для Аномалий, все части тела) меняются на 'Фатальные'.",
                    Description = "Память о луне, меняющая сопротивления врагов на фатальные",
                    Cost = 600,
                    SellPrice = 300,
                    Acquisition = "Рецепт",
                    Keywords = "сопротивление, фатальные, аномалии, память, луна",
                    Sources = new List<Sources>
                    {
                        new Sources { Location = "Соединить Разрубленная память + Проколотая память + Раздробленная память + Фрагмент греха + Фрагмент греха"}
                    }
                }
            };

            return gifts;
        }
    }
}