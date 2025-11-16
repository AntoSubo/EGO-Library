using EGO_Library.Commands;
using EGO_Library.Services;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel : BaseViewModel
    {
        public RelayCommand OpenGiftCommand { get; }

        private readonly NavigationService _navigation;

        public GiftListViewModel(NavigationService navigation)
        {
            _navigation = navigation;

            OpenGiftCommand = new RelayCommand(_ =>
            {
                _navigation.Navigate(new GiftDetailViewModel(_navigation));
            });
        }
    }
}
