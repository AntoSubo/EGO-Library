using EGO_Library.Services;

namespace EGO_Library.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public NavigationService Navigation { get; }

        public MainViewModel()
        {
            Navigation = new NavigationService();

            // стартовая страница
            Navigation.Navigate(new GiftListViewModel(Navigation));
        }
    }
}
