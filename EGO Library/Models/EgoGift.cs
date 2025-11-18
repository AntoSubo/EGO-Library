using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EGO_Library.Models
{
    public class EgoGift : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _status = string.Empty;
        private string _icon = string.Empty;
        private string _description = string.Empty;

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int Tier { get; set; }

        [MaxLength(50)]
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        [MaxLength(10)]
        public string Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(nameof(Icon)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        // Коллекция источников
        public ObservableCollection<Sources> Sources { get; set; } = new ObservableCollection<Sources>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public EgoGift() { }

        public EgoGift(string name, int tier, string status, string icon, string description)
        {
            Name = name;
            Tier = tier;
            Status = status;
            Icon = icon;
            Description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}