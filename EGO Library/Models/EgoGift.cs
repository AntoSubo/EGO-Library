namespace EGO_Library.Models
{
    public class EgoGift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public string StatusEffect { get; set; }
        public string Icon { get; set; } 
        public string Description { get; set; }
        public List<Sources> Sources { get; set; }
        public List<Recipes> FusionRecipes { get; set; }

        public EgoGift() { }

        
        public EgoGift(int id, string name, int tier, string status, string description)
        {
            Id = id;
            Name = name;
            Tier = tier;
            StatusEffect = status;
            Description = description;
            Sources = new List<Sources>();
            FusionRecipes = new List<Recipes>();
        }

    }

}
