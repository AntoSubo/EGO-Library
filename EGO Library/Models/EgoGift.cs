// Models/EgoGift.cs
using System.Collections.Generic;

namespace EGO_Library.Models
{
    public class EgoGift
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? Tier { get; set; }
        public string? Status { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public List<string> Sources { get; set; }
        public List<string> FusionRecipes { get; set; }
        public bool IsOwned { get; set; }

        public EgoGift()
        {
            Sources = new List<string>();
            FusionRecipes = new List<string>();
        }

        public EgoGift(string id, string name, int tier, string status, string description)
            : this()
        {
            Id = id;
            Name = name;
            Tier = tier;
            Status = status;
            Description = description;
        }
    }
}