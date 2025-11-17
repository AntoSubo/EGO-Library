using EGO_Library.Services;

namespace EGO_Library.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public RelayCommand GoBackCommand { get; }

        //public AboutViewModel(NavigationService navigation)
        //{
        //    GoBackCommand = new RelayCommand(_ =>
        //    {
        //        //navigation.Navigate(new GiftListViewModel(navigation));
        //    });
        //}
    }
}
