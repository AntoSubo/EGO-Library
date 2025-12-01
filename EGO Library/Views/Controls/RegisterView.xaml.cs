using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views.Controls
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded; 
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            if (DataContext is ViewModels.RegisterViewModel viewModel)
            {
                viewModel.RequestClearPasswords += OnRequestClearPasswords;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {

            if (DataContext is ViewModels.RegisterViewModel viewModel)
            {
                viewModel.RequestClearPasswords -= OnRequestClearPasswords;
            }
        }

        private void OnRequestClearPasswords(object sender, System.EventArgs e)
        {
            
            PasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.RegisterViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.RegisterViewModel viewModel)
            {
                viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            }
        }
    }
}