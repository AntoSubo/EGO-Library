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

                // удаление и пересоздание бд

                await context.Database.EnsureDeletedAsync();

                await context.Database.EnsureCreatedAsync();


                // пользователя

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


                // дары

                var realGifts = CreateRealGifts();
                await context.EgoGifts.AddRangeAsync(realGifts);
                await context.SaveChangesAsync();


                // источнеке
                var giftsCount = await context.EgoGifts.CountAsync();
                var sourcesCount = await context.Sources.CountAsync();
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
        // создание его даров
        private static List<EgoGift> CreateRealGifts()
        {
            var gifts = new List<EgoGift>();

            gifts.AddRange(new List<EgoGift>
    {
        new EgoGift
        {
            Name = "Фиксатор запястья",
            Tier = 1,
            Status = "Заряд",
            ImagePath = "Resources/Images/EGO/Tier1/Фиксатор запястья.png",
            Effect = "[Начало Хода] Союзника с 5+ (Счётчиками charge + 'Уникальным charge') получают 1 damageUp. Если у союзника есть атакующий Скилл принадлежности Гордыни, получает 2 damageUp вместо предыдущего эффекта.",
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
            ImagePath = "Resources/Images/EGO/Tier1/Кукла вуду.png",
            Effect = "[Начало Хода] Наносит 3 фиксированного урона Завистью всем врагам. Накладывает 1 powerDown на врагов с 33% или менее HP",
            Description = "Мистическая кукла, наносящая урон и ослабляющая ослабленных врагов",
            Cost = 149,
            SellPrice = 75,
            Acquisition = "Магазин и Событие: Ты будешь играть?",
            Keywords = "зависть, powerDown, фиксированный урон",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.12
                },
                new Sources {
                    Location = "Событие: Ты будешь играть?",
                    Type = "Event",
                    DropRate = 0.06
                }
            }
        },

        new EgoGift
        {
            Name = "Источник бесперебойного питания",
            Tier = 1,
            Status = "Заряд",
            ImagePath = "Resources/Images/EGO/Tier1/Источник бесперебойного питания.png",
            Effect = "[Конец Хода] Если 3- Счётчика charge, получает +3 Счётчика charge в следующем Начале Хода. [Конец Хода] Если 3+ Счётчика charge, получает 1 damageUp в Начале Хода.",
            Description = "Стабилизирует энергопоток, усиливая заряд и урон в зависимости от текущего уровня заряда",
            Cost = 156,
            SellPrice = 78,
            Acquisition = "Магазин и Событие: kqe-1j-23",
            Keywords = "charge, damageUp, энергия",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.14
                },
                new Sources {
                    Location = "Событие: kqe-1j-23",
                    Type = "Event",
                    DropRate = 0.07
                }
            }
        },

        new EgoGift
        {
            Name = "Неразряжаемый дефибриллятор",
            Tier = 1,
            Status = "Заряд",
            ImagePath = "Resources/Images/EGO/Tier1/Неразряжаемый дефибриллятор.png",
            Effect = "[Конец Хода] Если 3- Счётчика charge, получает +3 Счётчика charge в следующем Начале Хода. [Конец Хода] Если 3+ Счётчика charge, получает 1 damageUp в следующем Начале Хода.",
            Description = "Неразряжаемый дефибриллятор, который управляет зарядом и усиливает урон.",
            Cost = 156,
            SellPrice = 78,
            Acquisition = "Магазин и Событие",
            Keywords = "charge, damageUp, изолятор",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                },
                new Sources {
                    Location = "Событие",
                    Type = "Event",
                    DropRate = 0.08
                }
            }
        },

        new EgoGift
        {
            Name = "Гадание на завтра",
            Tier = 1,
            ImagePath = "Resources/Images/EGO/Tier1/Гадание на завтра.png",
            Effect = "Улучшает первую наградную карту Боя до самого высокого Ранга, который может быть на этаже.",
            Description = "Гадание, которое предсказывает удачу и улучшает боевые награды.",
            Cost = 140,
            SellPrice = 70,
            Acquisition = "Магазин и Событие: Звёздный светоч",
            Keywords = "support, улучшение карт, награды",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.12
                },
                new Sources {
                    Location = "Событие: Звёздный светоч",
                    Type = "Event",
                    DropRate = 0.06
                }
            }
        },

        new EgoGift
        {
            Name = "Тернистый путь",
            Tier = 1,
            Status = "Утопание",
            ImagePath = "Resources/Images/EGO/Tier1/Тернистый путь.png",
            Effect = "При попадании и нанесении урона по HP врага Скиллом, что накладывает sinking или 'Уникальное sinking', накладывает 3 sinking и +2 Счётчика sinking в Конце Хода. (Один раз на каждого врага за Ход). При активации А-Рез. Уныния или Похоти все противники получают 2 sinking и +3 Счётчика sinking в Начале Фазы Боя.",
            Description = "Тернистый путь, усиливающий эффекты sinking и взаимодействующий с Унынием и Похотью.",
            Cost = 160,
            SellPrice = 80,
            Acquisition = "Магазин и Событие: Престол небесного полководца",
            Keywords = "sinking, уныние, похоть, надвигающаяся волна",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                },
                new Sources {
                    Location = "Престол небесного полководца",
                    Type = "Event",
                    DropRate = 0.1
                }
            }
        },

        new EgoGift
        {
            Name = "Гемакон",
            Tier = 1,
            ImagePath = "Resources/Images/EGO/Tier1/Гемакон.png",
            Effect = "После нанесения урона врагу Скиллом, восстанавливает 12,5% от недостающих HP юнита (один раз на каждого юнита за Ход). Если Скилл принадлежности Гнева, восстанавливает 25% от недостающего HP вместо предыдущего эффекта.",
            Description = "Гемакон, обеспечивающий восстановление здоровья после атак, особенно эффективный с навыками Гнева.",
            Cost = 142,
            SellPrice = 71,
            Acquisition = "Магазин и Событие: Нимфа",
            Keywords = "heal, восстановление, гнев, хил",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                },
                new Sources {
                    Location = "Нимфа",
                    Type = "Event",
                    DropRate = 0.1
                }
            }
        },

        new EgoGift
        {
            Name = "Извращение",
            Tier = 1,
            ImagePath = "Resources/Images/EGO/Tier1/Извращение.png",
            Effect = "После убийства 1 и более врага атакующим Скиллом, получите +1 ЭГО ресурс, соответствующий принадлежности этого Скилла, в следующем Начале Хода. Если принадлежностью указанного Скилла был Гнев, получите +1 ЭГО ресурс всех Грехов, которые есть у союзника, в следующем Начале Хода вместо предыдущего эффекта.",
            Description = "Извращение, которое усиливает генерацию ЭГО ресурсов при убийствах, особенно для Гнева.",
            Cost = 157,
            SellPrice = 79,
            Acquisition = "Магазин и Событие: Спираль презрения",
            Keywords = "ЭГО ресурс, гнев, грехи, убийство",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                },
                new Sources {
                    Location = "Спираль презрения",
                    Type = "Event",
                    DropRate = 0.1
                }
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
            Effect = "После нанесения 12+ урона Скиллом, накладывает на цель 2 rupture. Если используется Рубящий Скилл, накладывает 4 rupture вместо предыдущего эффекта.",
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
            Effect = "[Эффекты применяются только к №7 размещённой Идентичности] Союзники получают на +1 больше Счётчика tremor от Скиллов или эффектов Монет. Если цель союзника в состоянии Оглушения, Низкой Морали, или Паники, получает на +2 больше вместо предыдущего эффекта.",
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
            Effect = "Если союзник попадает и наносит 12+ HP урона по врагу за Скилл, накладывает 2 bleed. При использовании Режущего Скилла, накладывает 4 bleed вместо предыдущего эффекта.",
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
            Effect = "Союзники с Атакующими Скиллами, которые накладывают sinking, Счётчик, или 'Уникальное sinking', восстанавливают дополнительно 5 SP (один раз за Ход) при победе в Столкновении; если на максимальном SP, наносят на +7.5% больше урона. Когда вышеназванный союзник использует Скилл с Отрицательными Монетками (кроме ЭГО Скиллов): на -15 или выше SP, сводит SP до -15 наносит на +(количество потерянного SP)% больше урона; если уже на менее чем -15 SP, наносит на +15% больше урона.",
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
            Effect = "Первое Начало Хода в Бою: союзник с наименьшим HP восстанавливает 15% от его максимального HP. При активации А-Рез. Гордыни, союзник с наименьшим HP восстанавливает 12.5% от своего недостающего HP в Начале Фазы Боя.",
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
            Effect = "[Конец Хода] Получает 1 poise на следующий ход за каждый атакующий Скилл, который нанес урон перед Концом Хода. В Направляемых Боях: получает 3 poise за каждый атакующий Скилл, который нанес урон перед Концом Хода, вместо предыдущего эффекта.",
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
            Effect = "[Начало Фазы Сражения] Случайный союзник получает 3 poise. Ставит в приоритет Идентичности со Скиллами, которые дают Счётчик poise. Ставит в приоритет союзников со Скиллами принадлежности Уныния; эти союзники получают 3 poise и +2 Счетчика poise.",
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
            Name = "Флакон цельной крови",
            Tier = 2,
            Status = "Кровотечение",
            ImagePath = "Resources/Images/EGO/Tier2/Флакон цельной крови.png",
            Effect = "Активируется когда союзник с Атакующими Скиллами, которые накладывают bleed или 'Уникальное Кровотечение' умирает (один раз за волну): - Все союзники восстанавливают (9% от умершей Идентичности) HP и увеличивает bloodfeast на (уровень умершей Идентичности + 9) - Все союзники получают (уровень умершей Идентичности/9) offenseLevelUp на следующий ход (макс. 6; округление вниз)",
            Description = "Флакон цельной крови, активирующий мощные эффекты при смерти союзника с кровотечением.",
            Cost = 200,
            SellPrice = 100,
            Acquisition = "Магазин",
            Keywords = "bleed, кровотечение, смерть союзника, восстановление",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.12
                }
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
                new Sources {
                    Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'",
                    Type = "Dungeon",
                    DropRate = 0.1
                },
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.08
                }
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
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                },
                new Sources {
                    Location = "Плачущий гроб",
                    Type = "Event",
                    DropRate = 0.1
                }
            }
        },

        new EgoGift
        {
            Name = "Голубые осколки звезды",
            Tier = 3,
            Status = "Утопание",
            ImagePath = "Resources/Images/EGO/Tier3/Голубые осколки звезды.png",
            Effect = "Противники в состоянии Низкой Морали или Паники получают -1 к Итоговому Значению Cкилла. При нанесение урона вышеуказанным противникам с помощью Скиллов, которые накладывают sinking, накладывает 2 defenseLevelDown (2 раза за ход)",
            Description = "ЭГО дар, специализирующийся на ослаблении противников с низкой моралью и паникой.",
            Cost = 246,
            SellPrice = 123,
            Acquisition = "Магазин",
            Keywords = "sinking, defenseLevelDown, мораль, паника",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.15
                }
            }
        },

        new EgoGift
        {
            Name = "Глефа из кристаллизованной крови",
            Tier = 3,
            Status = "Кровотечение",
            ImagePath = "Resources/Images/EGO/Tier3/Глефа из кристаллизованной крови.png",
            Effect = "Эффекты усиливаются в зависимости от количества Идентичностей с Атакующими Скиллами, которые расходуют bloodfeast. [Начало Боя] Все союзники получают (N - 2) hardbloodCast (макс. 3)",
            Description = "Оружие, созданное из кристаллизованной крови, усиливается в кровавых битвах.",
            Cost = 248,
            SellPrice = 124,
            Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
            Keywords = "bloodfeast, hardbloodCast, кровь, идентичности",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'",
                    Type = "Theme",
                    DropRate = 0.1
                },
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.12
                }
            }
        },

        new EgoGift
        {
            Name = "Плоть из крови, кровь из плоти",
            Tier = 3,
            Status = "Кровотечение",
            ImagePath = "Resources/Images/EGO/Tier3/Плоть из крови, кровь из плоти.png",
            Effect = "Усиливается в зависимости от Идентичностей с bloodfeast. Первое Начало Хода волны: увеличивает bloodfeast на (N x 20); накладывает 3 bleed и +3 Счётчика bleed на все цели. Союзники получают на -(N x 10%) меньше урона от bleed. Враги получают на +(N x 5)% больше урона от bleed.",
            Description = "Симбиотический ЭГО дар, превращающий кровь в оружие и защиту.",
            Cost = 252,
            SellPrice = 126,
            Acquisition = "Тема 'Убийство в ВАРП-экспрессе БокГак' и Магазин",
            Keywords = "bloodfeast, bleed, кровотечение, симбиоз",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'",
                    Type = "Theme",
                    DropRate = 0.08
                },
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.1
                }
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
                new Sources {
                    Location = "Тема 'Убийство в ВАРП-экспрессе БокГак'",
                    Type = "Theme",
                    DropRate = 0.09
                },
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.11
                }
            }
        },

        new EgoGift
        {
            Name = "Сгущающийся запах крови",
            Tier = 3,
            Status = "Кровотечение",
            ImagePath = "Resources/Images/EGO/Tier3/Сгущающийся запах крови.png",
            Effect = "Союзник с наименьшим HP восстанавливает 3% HP при попадании Атакующими Скиллами, которые накладывают bleed (2 раза за ход). В Столкновение с bleed против bleed противников +1 к Силе Столкновения. При 5+ Идентичностях с bleed: +1 к Силе и +10% урона. При 10+ Идентичностях: +2 к Силе и +15% урона.",
            Description = "ЭГО дар, усиливающий команды, специализирующиеся на кровотечении.",
            Cost = 252,
            SellPrice = 126,
            Acquisition = "Магазин",
            Keywords = "bleed, столкновение, лечение, сила",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.14
                }
            }
        },

        new EgoGift
        {
            Name = "Изношенная рукоять",
            Tier = 3,
            Status = "Разрыв",
            ImagePath = "Resources/Images/EGO/Tier3/Изношенная рукоять.png",
            Effect = "[Начало Хода] Все союзники получают 1 lustPowerUp. Идентичности Родословной Клинка или клана «Курокумо» получают 2 lustPowerUp и 1 slashPowerUp вместо предыдущего эффекта.",
            Description = "Древняя рукоять, усиливающая владельцев мечей и членов клана Курокумо.",
            Cost = 248,
            SellPrice = 124,
            Acquisition = "Тема 'Уступаю плоть, дабы забрать их кости БокГак' и Магазин",
            Keywords = "lustPowerUp, slashPowerUp, клинок, курокумо",
            Sources = new List<Sources>
            {
                new Sources {
                    Location = "Тема 'Уступаю плоть, дабы забрать их кости БокГак'",
                    Type = "Theme",
                    DropRate = 0.07
                },
                new Sources {
                    Location = "Магазин",
                    Type = "Shop",
                    DropRate = 0.13
                }
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
            Effect = "[Конец Хода] Накладывает на всех врагов rupture, равный их Скорости. При активации А-Рез. Похоти накладывает 3 rupture и +3 Счётчика rupture на случайного врага в Начале Фазы Боя.",
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
            Effect = "Союзники с Счётчиками charge получают 1 offenseLevelUp при каждом Столкновении. (макс. 6. Сбрасывается После Атаки или когда Счётчик charge полностью пропадает) [Конец Хода] Идентичности с 10+ Счётчиками charge получают 1 haste в следующем ходу. При 18+ Счётчиках charge, также получают 3 offenseLevelUp.",
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
            Effect = "Первое Начало Хода в Бою: даёт случайные ЭГО ресурсы в количестве, равное числу противников. Если на поле 4 или больше Идентичностей Родословной Клинка или клана «Курокумо» участвующих в этом Бою (учитываются только Идентичности на поле в Начало Столкновения), даёт ЭГО ресурс Греха, которого меньше всего вместо предыдущего эффекта.",
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
            Name = "Доспех из чёрного железа",
            Tier = 2,
            Status = "Разрыв",
            ImagePath = "Resources/Images/EGO/Tier2/Доспех из чёрного железа.png",
            Effect = "[Начало Хода] Накладывает (Уровень Защиты / 3) Щита на 1 союзника с наибольшим Уровнем Защиты (В приоритете Идентичности «Хэйшоу» из стаи У; макс. Щита 15) Когда союзник с Щитом побеждает в Столкновение, накладывает (Скорость / 2) tremor; затем, получает 2 haste и 2 defenseLevelUp на следующий ход (один раз за ход за Идентичность)",
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
            Effect = "Эффект для всех Идентичностей с атакующими Скиллами принадлежности Гнева или Похоти. Конец Хода: если этот юнит не получал никакого урона от атакующих Скиллов врагов в этом Ходу, получает 2 haste и 2 defensePowerUp. Конец Хода: если этот юнит получил урон от атакующих Скиллов врагов в этом Ходу, получает 2 bind и +2 к Силе Столкновения в следующем Ходу.",
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
            Effect = "Скиллы наносят на +10% больше урона по Оглушенным целям. Если Скилл накладывает tremor, Счётчик tremor, активирует amplitudeConversion, tremorBurst, или amplitudeEntanglement, или если Скилл принадлежности Лености, урон по Оглушённой цели увеличивается до 20% вместо предыдущего эффекта.",
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
            Effect = "Первое Начало Хода в Бою: Идентичности с Базовыми Атакующими Скиллами, которые получают burn, получают 5 burn и восстанавливают 8 SP. Для Идентичностей «Хэйшоу» из ветви Ю: HP ограничивается до 49% от макс.HP. Взамен, получают на 50% меньше урона и на 50% меньше исцеления (исключая лечение полученное от ЭГО Даров)",
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
            Effect = "Когда союзник вступает в Столкновение против врага с большей Скоростью, получает +1 к Силе Столкновения. Когда союзник выигрывает Столкновение против врага с большей Скоростью, получает 2 offenseLevelUp в следующем Ходу. (Макс. 4)",
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
            Effect = "[Начало Хода] Накладывает 1 bind на врага, который восстановился от Оглушения в прошлом ходу. При попадании от союзника с наивысшим процентом HP: если цель в состоянии Оглушения, наносит +7.5% больше урона за каждый уровень Оглушения.",
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
            Effect = "Когда союзник теряет HP из-за эффекта своего Скилла Атаки, исцеляет половину HP, потерянных из-за эффекта Скилла, в следующем Начале Хода. (округление вниз) Когда союзник восстанавливает HP из-за эффекта ЭГО, он восстанавливает на +12,5% больше HP. (округление вниз)",
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
            Effect = "Когда союзник с Счётчиками charge проигрывает Столкновение, получает Щит равный Счётчикам charge. (один раз за ход) Когда HP падает ниже 50%, получает (Счётчик charge х 2) Щита вместо предыдущего эффекта. (один раз за ход) Если Идентичность Многопрофильного Офиса получает урон по HP во время этого Боя: в следующем ходу, они восстанавливают 33% HP, потерянного в прошлом ходу (округление вниз)",
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
            Effect = "[Начало Хода] Союзники с poise получают 1 attackPowerUp. Союзники, имеющие 5+ Счётчиков poise, получают 2 attackPowerUp вместо предыдущего эффекта.",
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
            Effect = "[Начало Хода] Добавляет +1 ЭГО ресурс к случайному Греху, который не использовался в прошлом Ходу. Если же в предыдущий Ход был использован Скилл принадлежности Похоти, добавляет по +1 ЭГО ресурсу к Грехам, которые не использовались в прошлом Ходу.",
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
            Effect = "[Эффект применяется только к №5 размещённой Идентичности] При попадании по врагу Дробящим Скиллом, наносит 3 бонусного Дробящего урона. Атакующие Скиллы принадлежности Уныния: также восстанавливает дополнительно 3 здоровья.",
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
            Effect = "[Эффекты применяются только к №6 размещённой Идентичности] Начало Фазы Боя: наносит на +10% больше урона за каждый вражеский атакующий Скилл, который нацелен на этого юнита (макс. 20%). Начало Фазы Боя: когда является целью 2+ атакующих Скиллов, наносит (кол-во Скиллов, которые нацелены на этого юнита) фиксированного урона HP и SP всем врагам, которые нацелены на этого юнита. (макс. 3; этот эффект не наносит урона союзникам).",
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
            Effect = "[Конец Хода] Если на этого юнита наложено bleed, уменьшает bleed на себе на половину и восстанавливает (уменьшенное bleed х Счётчик bleed на себе) HP. (округление вниз. За один раз может восстановить до 10% от макс. HP)",
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
            Effect = "Когда союзник умирает, получает 1 ресурс Э.Г.О. за каждый из Навыков 1 и 2 этого союзника. [После Атаки] Если враг убит, получает 1 случайный ресурс Э.Г.О. (один раз за Скилл).",
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
    });

            return gifts;
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