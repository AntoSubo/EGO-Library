using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel : BaseViewModel
    {
        private ObservableCollection<EgoGift> _gifts;
        private EgoGift? _selectedGift;
        private string _searchText = string.Empty;

        public ObservableCollection<EgoGift> Gifts
        {
            get => _gifts;
            set { _gifts = value; OnPropertyChanged(); }
        }

        public EgoGift? SelectedGift
        {
            get => _selectedGift;
            set { _selectedGift = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public ICommand ViewGiftDetailCommand { get; }

        public GiftListViewModel()
        {
            Gifts = new ObservableCollection<EgoGift>();
            ViewGiftDetailCommand = new RelayCommand(ExecuteViewGiftDetail);
        }

        private void ExecuteViewGiftDetail(object? obj)
        {
            // Этот метод будет работать когда выберем дар
            if (SelectedGift != null)
            {
                // Здесь будет навигация к деталям дара
                System.Windows.MessageBox.Show($"Выбран: {SelectedGift.Name}");
            }
        }
    }
}