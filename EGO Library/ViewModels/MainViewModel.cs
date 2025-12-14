using EGO_Library.Services;
using EGO_Library.Views.Controls;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly DataService _dataService;
        private readonly INavigationService _navigationService;
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public bool IsAuthenticated => _authService?.IsAuthenticated ?? false;

        public ICommand NavigateToGiftListCommand { get; }
        public ICommand NavigateToRecipesCommand { get; }
        public ICommand NavigateToAboutCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(IAuthService authService, DataService dataService)
        {
            _authService = authService;
            _dataService = dataService;

            _navigationService = new NavigationService(this, _authService, _dataService);

            _authService.AuthenticationChanged += OnAuthenticationChanged;

            NavigateToGiftListCommand = new RelayCommand(_ => _navigationService.NavigateToGiftList());
            NavigateToRecipesCommand = new RelayCommand(_ => _navigationService.NavigateToRecipes());
            NavigateToAboutCommand = new RelayCommand(_ => _navigationService.NavigateToAbout());

            if (_authService.IsAuthenticated)
            {
                _navigationService.NavigateToGiftList();
            }
            else
            {
                ShowLoginView(); 
            }
        }

        private void OnAuthenticationChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged(nameof(IsAuthenticated));

            if (_authService.IsAuthenticated)
            {
                _navigationService.NavigateToGiftList();
            }
            else
            {
                ShowLoginView();
            }
        }

        public void ShowLoginView()
        {
            var loginView = new LoginView();
            loginView.DataContext = new LoginViewModel(_authService, _navigationService);
            CurrentView = loginView;
        }

        public void ShowRegisterView()
        {
            var registerView = new RegisterView();
            registerView.DataContext = new RegisterViewModel(_authService, _navigationService);
            CurrentView = registerView;
        }
    }
}