using EGO_Library.Services;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand GoBackCommand { get; }

        public AboutViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }
    }
}