using EGO_Library.ViewModels;
using System.ComponentModel;

namespace EGO_Library.Services
{
    public class NavigationService : INotifyPropertyChanged
    {
        private BaseViewModel? _currentViewModel;

        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Navigate(BaseViewModel viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}
