using EGO_Library.Services;
using System;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        private string _successMessage;

        // Событие для запроса очистки паролей в View
        public event EventHandler RequestClearPasswords;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            RegisterCommand = new RelayCommand(_ => Register(), _ => CanRegister());
            ClearCommand = new RelayCommand(_ => Clear());
            NavigateToLoginCommand = new RelayCommand(_ => _navigationService.NavigateToLogin());
        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private void Register()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Валидация
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Пароль должен содержать минимум 6 символов";
                return;
            }

            if (_authService.Register(Username, Password, Email))
            {
                SuccessMessage = "Регистрация успешна! Вы автоматически вошли в систему.";
            }
            else
            {
                ErrorMessage = "Ошибка при регистрации. Попробуйте еще раз.";
            }
        }

        private void Clear()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Вызываем событие для очистки PasswordBox в View
            RequestClearPasswords?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToLogin()
        {
            _navigationService.NavigateToLogin();
        }
    }
}