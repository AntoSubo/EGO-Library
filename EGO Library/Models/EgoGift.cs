// Models/EgoGift.cs
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EGO_Library.Models
{
    public class EgoGift
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Tier { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Icon { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Коллекция источников (можно инициализировать пустой коллекцией)
        public ObservableCollection<Sources> Sources { get; set; } = new ObservableCollection<Sources>();

        // Свойство для удобства работы со списком строк (если нужно)
        public List<string> SourceStrings
        {
            get => Sources.Select(s => s.ToString()).ToList();
        }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Конструктор для удобства
        public EgoGift() { }

        public EgoGift(string name, int tier, string status, string icon, string description)
        {
            Name = name;
            Tier = tier;
            Status = status;
            Icon = icon;
            Description = description;
        }

        // Метод для добавления источника
        public void AddSource(string location, string type, int floor = 0, double dropRate = 0)
        {
            Sources.Add(new Sources(location, type, floor, dropRate));
        }
    }
}