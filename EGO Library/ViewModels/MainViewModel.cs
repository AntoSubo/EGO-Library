using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;

namespace EGO_Library.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly INavigationService _navigationService;
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EgoGift> Gifts { get; set; } = new ObservableCollection<EgoGift>();

        // Команды навигации для главного меню (если будет)
        public RelayCommand NavigateToGiftListCommand { get; }
        public RelayCommand NavigateToRecipesCommand { get; }
        public RelayCommand NavigateToAboutCommand { get; }

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
            _navigationService = new NavigationService(this, dataService);

            // Инициализация команд
            NavigateToGiftListCommand = new RelayCommand(_ => _navigationService.NavigateToGiftList());
            NavigateToRecipesCommand = new RelayCommand(_ => _navigationService.NavigateToRecipes());
            NavigateToAboutCommand = new RelayCommand(_ => _navigationService.NavigateToAbout());

            // Стартовая страница
            _navigationService.NavigateToGiftList();
        }
    }
}