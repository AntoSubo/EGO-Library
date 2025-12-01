using EGO_Library.Models;
using EGO_Library.ViewModels;
using EGO_Library.Views.Controls;

namespace EGO_Library.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DataService _dataService;
        private readonly IAuthService _authService;

        public NavigationService(MainViewModel mainViewModel, IAuthService authService, DataService dataService)
        {
            _mainViewModel = mainViewModel;
            _authService = authService;
            _dataService = dataService;
        }

        // основная навигация
        public void NavigateToGiftList()
        {
            var giftListView = new GiftListView();
            giftListView.DataContext = new GiftListViewModel(_dataService, this);
            _mainViewModel.CurrentView = giftListView;
        }

        public void NavigateToRecipes()
        {
            var recipeView = new RecipeView();
            recipeView.DataContext = new RecipeViewModel(this, _dataService);
            _mainViewModel.CurrentView = recipeView;
        }

        public void NavigateToAbout()
        {
            var aboutView = new AboutView();
            aboutView.DataContext = new AboutViewModel(this);
            _mainViewModel.CurrentView = aboutView;
        }

        public void NavigateToGiftDetail(EgoGift gift)
        {
            var giftDetailView = new GiftDetailView();
            giftDetailView.DataContext = new GiftDetailViewModel(gift, this, _dataService);
            _mainViewModel.CurrentView = giftDetailView;
        }

        // атризаця
        public void NavigateToLogin()
        {
            _mainViewModel.ShowLoginView();
        }

        public void NavigateToRegister()
        {
            _mainViewModel.ShowRegisterView();
        }

        public void GoBack()
        {
            if (_authService.IsAuthenticated)
            {
                NavigateToGiftList();
            }
            else
            {
                NavigateToLogin();
            }
        }
    }
}