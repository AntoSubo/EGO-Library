using EGO_Library.Commands;
using EGO_Library.Services;

namespace EGO_Library.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        public RelayCommand GoBackCommand { get; }

        public RecipeViewModel(NavigationService navigation)
        {
            GoBackCommand = new RelayCommand(_ =>
            {
                //navigation.Navigate(new GiftListViewModel(navigation));
            });
        }
    }
}
