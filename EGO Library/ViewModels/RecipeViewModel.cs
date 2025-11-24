using EGO_Library.Services;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand GoBackCommand { get; }

        public RecipeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }
    }
}