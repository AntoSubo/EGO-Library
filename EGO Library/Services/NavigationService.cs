using EGO_Library.Models;
using EGO_Library.ViewModels;
using EGO_Library.Views.Controls;

namespace EGO_Library.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DataService _dataService;

        public NavigationService(MainViewModel mainViewModel, DataService dataService)
        {
            _mainViewModel = mainViewModel;
            _dataService = dataService;
        }

        public void NavigateToGiftList()
        {
            var giftListView = new GiftListView();
            giftListView.DataContext = new GiftListViewModel(_dataService, this);
            _mainViewModel.CurrentView = giftListView;
        }

        public void NavigateToRecipes()
        {
            var recipeView = new RecipeView();
            recipeView.DataContext = new RecipeViewModel(this);
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
            giftDetailView.DataContext = new GiftDetailViewModel(gift, this);
            _mainViewModel.CurrentView = giftDetailView;
        }

        public void GoBack()
        {
            NavigateToGiftList(); // Возврат к списку даров
        }
    }
}