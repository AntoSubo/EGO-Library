using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EGO_Library.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Внешний ключ для результирующего дара
        public int ResultGiftId { get; set; }
        public EgoGift ResultGift { get; set; } = null!;

        // Требуемые дары
        public List<EgoGift> RequiredGifts { get; set; } = new List<EgoGift>();

        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Difficulty { get; set; } = string.Empty;

        public Recipe() { }

        public Recipe(string name, string description, EgoGift resultGift, List<EgoGift> requiredGifts, string location = "Mirror Dungeon")
        {
            Name = name;
            Description = description;
            ResultGift = resultGift;
            RequiredGifts = requiredGifts ?? new List<EgoGift>();
            Location = location;
            Difficulty = resultGift.Tier switch
            {
                4 or 5 => "Hard",
                3 => "Medium",
                _ => "Easy"
            };
        }
    }
}