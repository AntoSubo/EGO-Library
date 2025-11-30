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
                    Status = "Charge",
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
                    Status = "Debuff",
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
                    Status = "Charge",
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
                    Status = "Charge",
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
                    Status = "Support",
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
                    Status = "Sinking",
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
                    Status = "Heal",
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
                    Status = "Resource",
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
                    Name = "Флакон цельной крови",
                    Tier = 2,
                    Status = "Bleed",
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
                    Status = "Charge",
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
                    Status = "Bleed",
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
                    Status = "Sinking",
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
                    Status = "Bleed",
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
                    Status = "Bleed",
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
                    Status = "Charge",
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
                    Status = "Bleed",
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
                    Status = "Lust",
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