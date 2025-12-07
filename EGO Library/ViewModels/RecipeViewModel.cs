using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace EGO_Library.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly DataService _dataService;

        private ObservableCollection<Recipes> _allRecipes = new ObservableCollection<Recipes>();
        private ObservableCollection<Recipes> _filteredRecipes = new ObservableCollection<Recipes>();
        private string _searchText = string.Empty;
        private string _selectedDifficulty = "All";

        public ObservableCollection<Recipes> Recipes => _filteredRecipes;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterRecipes();
                }
            }
        }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged();
                    FilterRecipes();
                }
            }
        }

        public List<string> AvailableDifficulties { get; } = new List<string>
        {
            "All", "Easy", "Medium", "Hard"
        };

        public int TotalRecipes => _allRecipes?.Count ?? 0;
        public bool HasRecipes => _allRecipes?.Count > 0;

        public ICommand GoBackCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public RecipeViewModel(INavigationService navigationService, DataService dataService = null)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            Debug.WriteLine($"RecipeViewModel создан. DataService: {dataService != null}");

            if (_dataService != null)
            {
                _ = LoadRecipesAsync();
            }
            else
            {
                Debug.WriteLine("DataService is null! Рецепты не будут загружены.");
            }
        }

        private async Task LoadRecipesAsync()
        {
            try
            {
                Debug.WriteLine("Начинаем загрузку рецептов...");

                var recipes = await _dataService.GetAllRecipesAsync();
                Debug.WriteLine($"Получено {recipes?.Count ?? 0} рецептов из DataService");

                if (recipes != null && recipes.Any())
                {
                    Debug.WriteLine($"Первый рецепт: {recipes[0].Name}");
                    Debug.WriteLine($"ResultGift: {recipes[0].ResultGift?.Name}");
                    Debug.WriteLine($"RequiredGifts count: {recipes[0].RequiredGifts?.Count}");

                    _allRecipes = new ObservableCollection<Recipes>(recipes);

                    Debug.WriteLine($"Загружено {_allRecipes.Count} рецептов в коллекцию");

                    FilterRecipes();

                    OnPropertyChanged(nameof(Recipes));
                    OnPropertyChanged(nameof(TotalRecipes));
                    OnPropertyChanged(nameof(HasRecipes));
                }
                else
                {
                    Debug.WriteLine("Рецептов нет в базе данных!");
                    _allRecipes.Clear();
                    _filteredRecipes.Clear();

                    OnPropertyChanged(nameof(Recipes));
                    OnPropertyChanged(nameof(TotalRecipes));
                    OnPropertyChanged(nameof(HasRecipes));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки рецептов: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void FilterRecipes()
        {
            try
            {
                if (_allRecipes == null || !_allRecipes.Any())
                {
                    _filteredRecipes.Clear();
                    return;
                }

                var filtered = _allRecipes.Where(r =>
                    (string.IsNullOrEmpty(SearchText) ||
                     (r.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                     (r.Description?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                    (SelectedDifficulty == "All" || r.Difficulty == SelectedDifficulty)
                ).ToList();

                _filteredRecipes = new ObservableCollection<Recipes>(filtered);
                Debug.WriteLine($"Отфильтровано {_filteredRecipes.Count} рецептов");

                OnPropertyChanged(nameof(Recipes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedDifficulty = "All";
        }
    }
}