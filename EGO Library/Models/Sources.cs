namespace EGO_Library.Models
{
    public class Sources
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public List<EgoGift> EgoGifts { get; set; } // навигационное свойство для EF Core
    }
}
