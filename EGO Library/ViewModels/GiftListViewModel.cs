// ViewModels/GiftListViewModel.cs
using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private ObservableCollection<EgoGift> _gifts;
        private EgoGift _selectedGift;
        private string _searchText;

        public ObservableCollection<EgoGift> Gifts
        {
            get => _gifts;
            set { _gifts = value; OnPropertyChanged(); }
        }

        public EgoGift SelectedGift
        {
            get => _selectedGift;
            set { _selectedGift = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterGifts(); }
        }

        public ICommand ViewGiftDetailCommand { get; }
        public ICommand AddGiftCommand { get; }
        public ICommand DeleteGiftCommand { get; }

        public GiftListViewModel(DataService dataService)
        {
            _dataService = dataService;
            Gifts = _dataService.GetAllGifts();

            ViewGiftDetailCommand = new RelayCommand(ExecuteViewGiftDetail, CanExecuteViewGiftDetail);
            AddGiftCommand = new RelayCommand(ExecuteAddGift);
            DeleteGiftCommand = new RelayCommand(ExecuteDeleteGift, CanExecuteDeleteGift);
        }

        private void FilterGifts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Gifts = _dataService.GetAllGifts();
            }
            else
            {
                var filtered = _dataService.GetAllGifts()
                    .Where(g => g.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               g.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               g.Status.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Gifts = new ObservableCollection<EgoGift>(filtered);
            }
        }

        private void ExecuteViewGiftDetail(object obj)
        {
            if (SelectedGift != null)
            {
                // Навигация к деталям дара
                //NavigationService.NavigateToGiftDetail(SelectedGift);
            }
        }

        private bool CanExecuteViewGiftDetail(object obj)
        {
            return SelectedGift != null;
        }

        private void ExecuteAddGift(object obj)
        {
            // Логика добавления нового дара
            var newGift = new EgoGift { Name = "New Gift", Tier = 1 };
            _dataService.AddGift(newGift);
            Gifts = _dataService.GetAllGifts();
        }

        private void ExecuteDeleteGift(object obj)
        {
            if (SelectedGift != null)
            {
                _dataService.DeleteGift(SelectedGift.Id);
                Gifts = _dataService.GetAllGifts();
            }
        }

        private bool CanExecuteDeleteGift(object obj)
        {
            return SelectedGift != null;
        }
    }
}