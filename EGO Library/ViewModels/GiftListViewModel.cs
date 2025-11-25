using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<EgoGift> _gifts;
        private string _searchText;
        private int? _selectedTier;
        private string _selectedStatus = "All";

        public ObservableCollection<EgoGift> Gifts
        {
            get => _gifts;
            set { _gifts = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); _ = LoadGiftsAsync(); }
        }

        public int? SelectedTier
        {
            get => _selectedTier;
            set { _selectedTier = value; OnPropertyChanged(); _ = LoadGiftsAsync(); }
        }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(); _ = LoadGiftsAsync(); }
        }

        public List<int> AvailableTiers { get; private set; } = new List<int>();
        public List<string> AvailableStatuses { get; private set; } = new List<string> { "All" };

        // Команды
        public ICommand ClearFiltersCommand { get; }
        public ICommand NavigateToAboutCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand SelectGiftCommand { get; }
        public ICommand NavigateToRecipesCommand { get; }

        public GiftListViewModel(DataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            NavigateToAboutCommand = new RelayCommand(_ => _navigationService.NavigateToAbout());
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
            SelectGiftCommand = new RelayCommand(SelectGift);
            NavigateToRecipesCommand = new RelayCommand(_ => _navigationService.NavigateToRecipes());

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadAvailableFiltersAsync();
            await LoadGiftsAsync();
        }

        private async Task LoadAvailableFiltersAsync()
        {
            AvailableTiers = await _dataService.GetAvailableTiersAsync();
            var statuses = await _dataService.GetAvailableStatusesAsync();
            AvailableStatuses.AddRange(statuses);
            OnPropertyChanged(nameof(AvailableTiers));
            OnPropertyChanged(nameof(AvailableStatuses));
        }

        private async Task LoadGiftsAsync()
        {
            var gifts = await _dataService.GetGiftsAsync(SearchText, SelectedTier,
                SelectedStatus == "All" ? null : SelectedStatus);
            Gifts = new ObservableCollection<EgoGift>(gifts);
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedTier = null;
            SelectedStatus = "All";
        }

        private static void ShowHelp()
        {
            System.Windows.MessageBox.Show(
                "Для просмотра деталей дара просто кликните на него в списке.\n\n" +
                "Фильтры:\n- Поиск: по названию, описанию или статусу\n- Уровень: фильтр по Tier\n- Статус: фильтр по эффекту",
                "Помощь");
        }

        private void SelectGift(object parameter)
        {
            if (parameter is EgoGift gift)
            {
                _navigationService.NavigateToGiftDetail(gift);
            }
        }
    }
}