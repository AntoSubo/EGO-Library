namespace EGO_Library.Models
{
    public class Recipes
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public List<EgoGift> Components { get; set; } // компоненты
    }
}
