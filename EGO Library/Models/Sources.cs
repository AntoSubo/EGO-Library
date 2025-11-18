using System.ComponentModel;

namespace EGO_Library.Models
{
    public class Sources : INotifyPropertyChanged
    {
        private string _location = string.Empty;
        private string _type = string.Empty;
        private int _floor;
        private double _dropRate;

        public int Id { get; set; }
        public int EgoGiftId { get; set; } //внешний ключ

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }

        public int Floor
        {
            get => _floor;
            set { _floor = value; OnPropertyChanged(nameof(Floor)); }
        }

        public double DropRate
        {
            get => _dropRate;
            set { _dropRate = value; OnPropertyChanged(nameof(DropRate)); }
        }

        public Sources() { }

        public Sources(string location, string type, int floor = 0, double dropRate = 0)
        {
            Location = location;
            Type = type;
            Floor = floor;
            DropRate = dropRate;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Location} (Floor {Floor})";
        }
    }
}