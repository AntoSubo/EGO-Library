using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EGO_Library.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly DataService _dataService;

        private ObservableCollection<Recipe> _recipes;
        private string _searchText;
        private string _selectedDifficulty = "All";

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set { _recipes = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterRecipes(); }
        }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set { _selectedDifficulty = value; OnPropertyChanged(); FilterRecipes(); }
        }

        public List<string> AvailableDifficulties { get; } = new List<string>
        {
            "All", "Easy", "Medium", "Hard"
        };

        public int TotalRecipes => Recipes?.Count ?? 0;
        public bool HasRecipes => Recipes?.Count > 0;

        public ICommand GoBackCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand ShowGiftDetailCommand { get; }

        public RecipeViewModel(INavigationService navigationService, DataService dataService = null)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            ShowGiftDetailCommand = new RelayCommand(ShowGiftDetail);

            _ = LoadRecipesAsync();
        }

        private async Task LoadRecipesAsync()
        {
            //if (_dataService != null)
            //{
            //    var recipes = await _dataService.GetAllRecipesAsync();
            //    Recipes = new ObservableCollection<Recipe>(recipes);
            //}
            //else
            //{

            //}
        }


        private void FilterRecipes()
        {
            if (Recipes == null) return;

            var filtered = Recipes.Where(r =>
                (string.IsNullOrEmpty(SearchText) ||
                 r.Name.Contains(SearchText) ||
                 r.Description.Contains(SearchText)) &&
                (SelectedDifficulty == "All" || r.Difficulty == SelectedDifficulty)
            );

            Recipes = new ObservableCollection<Recipe>(filtered);
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedDifficulty = "All";
            _ = LoadRecipesAsync();
        }

        private void ShowGiftDetail(object parameter)
        {
            if (parameter is EgoGift gift)
            {
                _navigationService.NavigateToGiftDetail(gift);
            }
        }
    }
}