using System.Collections.ObjectModel;
using System.ComponentModel;
using EGO_Library.Models;
namespace EGO_Library.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EgoGift> Gifts { get; set; }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterGifts();
            }
        }

        private ObservableCollection<EgoGift> allGifts;

        public MainViewModel()
        {
            // Здесь просто тестовые данные, потом заменим на загрузку из файла
            allGifts = new ObservableCollection<EgoGift>
            {
                new EgoGift { Name = "Rusty Blade", Tier = 1, StatusEffect = "Bleed" },
                new EgoGift { Name = "Sun Fragment", Tier = 3, StatusEffect = "Burn" },
                new EgoGift { Name = "Lust Shard", Tier = 2, StatusEffect = "Poise" }
            };

            Gifts = new ObservableCollection<EgoGift>(allGifts);
        }

        private void FilterGifts()
        {
            Gifts.Clear();
            foreach (var gift in allGifts)
            {
                if (string.IsNullOrEmpty(SearchText) ||
                    gift.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase))
                    Gifts.Add(gift);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
