namespace EGO_Library.Models
{
    public class EgoGift
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public string Status { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public List<string> Sources { get; set; }
        public List<string> FusionRecipes { get; set; }

        public EgoGift()
        {
            Sources = new List<string>();
            FusionRecipes = new List<string>();
        }
    }
}