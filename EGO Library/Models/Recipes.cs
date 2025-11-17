// Models/Recipe.cs
using System.Collections.Generic;

namespace EGO_Library.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public EgoGift? ResultGift { get; set; }
        public List<EgoGift> RequiredGifts { get; set; }
        public string? Location { get; set; }
        public string? Difficulty { get; set; }

        public Recipe()
        {
            RequiredGifts = new List<EgoGift>();
        }

        public Recipe(EgoGift resultGift, List<EgoGift> requiredGifts, string location = "Mirror Dungeon")
        {
            ResultGift = resultGift;
            RequiredGifts = requiredGifts ?? new List<EgoGift>();
            Location = location;
            Difficulty = resultGift?.Tier switch
            {
                4 => "Hard",
                3 => "Medium",
                _ => "Easy"
            };
        }
    }
}