using System.ComponentModel;

namespace EGO_Library.Models
{
    public class Sources 
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Floor { get; set; }
        public double DropRate { get; set; }

        // Внешний ключ
        public int EgoGiftId { get; set; }

        // Навигационное свойство
        public virtual EgoGift EgoGift { get; set; }

        public override string ToString()
        {
            return $"{Location} {(Floor > 0 ? $"(Floor {Floor})" : "")}";
        }
    }
}