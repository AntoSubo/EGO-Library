using EGO_Library.Services;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        private string _username = "admin";
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            LoginCommand = new RelayCommand(_ => Login(), _ => CanLogin());
            ClearCommand = new RelayCommand(_ => Clear());
            NavigateToRegisterCommand = new RelayCommand(_ => _navigationService.NavigateToRegister());
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void Login()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (_authService.Login(Username, Password))
                {
                    ErrorMessage = "Успешный вход!";
                }
                else
                {
                    ErrorMessage = "Неверное имя пользователя или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private void Clear()
        {
            Username = "admin";
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}