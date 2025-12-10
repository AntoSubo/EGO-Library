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
        private EgoGift _zoomedGift;
        private bool _isImageZoomed;
        private double _imageZoomLevel = 1.0;

        public ObservableCollection<Recipes> Recipes => _filteredRecipes;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
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
                    OnPropertyChanged(nameof(SelectedDifficulty));
                    FilterRecipes();
                }
            }
        }

        public EgoGift ZoomedGift
        {
            get => _zoomedGift;
            set
            {
                if (_zoomedGift != value)
                {
                    _zoomedGift = value;
                    OnPropertyChanged(nameof(ZoomedGift));
                }
            }
        }

        public bool IsImageZoomed
        {
            get => _isImageZoomed;
            set
            {
                if (_isImageZoomed != value)
                {
                    _isImageZoomed = value;
                    OnPropertyChanged(nameof(IsImageZoomed));

                    // Сбрасываем зум при закрытии
                    if (!value)
                    {
                        ImageZoomLevel = 1.0;
                    }
                }
            }
        }

        public double ImageZoomLevel
        {
            get => _imageZoomLevel;
            set
            {
                var newValue = Math.Max(0.5, Math.Min(5.0, value));
                if (_imageZoomLevel != newValue)
                {
                    _imageZoomLevel = newValue;
                    OnPropertyChanged(nameof(ImageZoomLevel));
                }
            }
        }

        public List<string> AvailableDifficulties { get; } = new List<string>
        {
            "All", "Easy", "Medium", "Hard"
        };

        public int TotalRecipes => _allRecipes.Count;
        public bool HasRecipes => _allRecipes.Count > 0;

        public ICommand GoBackCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand ZoomImageCommand { get; }
        public ICommand CloseZoomCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }
        public ICommand ResetZoomCommand { get; }

        public RecipeViewModel(INavigationService navigationService, DataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            ZoomImageCommand = new RelayCommand(obj =>
            {
                if (obj is EgoGift gift)
                {
                    ZoomImage(gift);
                }
            });

            CloseZoomCommand = new RelayCommand(_ => CloseImageZoom());
            ZoomInCommand = new RelayCommand(_ => ZoomIn());
            ZoomOutCommand = new RelayCommand(_ => ZoomOut());
            ResetZoomCommand = new RelayCommand(_ => ResetZoom());

            _ = LoadRecipesAsync();
        }

        private async Task LoadRecipesAsync()
        {
            try
            {
                var recipes = await _dataService.GetAllRecipesAsync();

                if (recipes != null)
                {
                    _allRecipes = new ObservableCollection<Recipes>(recipes);
                }

                FilterRecipes();
                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(TotalRecipes));
                OnPropertyChanged(nameof(HasRecipes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RecipeViewModel] Ошибка загрузки: {ex.Message}");
            }
        }

        private void FilterRecipes()
        {
            var filtered = _allRecipes.Where(r =>
                (string.IsNullOrEmpty(SearchText) ||
                 (r.Name?.ToLower().Contains(SearchText.ToLower()) == true) ||
                 (r.Description?.ToLower().Contains(SearchText.ToLower()) == true)) &&
                (SelectedDifficulty == "All" || r.Difficulty == SelectedDifficulty)
            ).ToList();

            _filteredRecipes = new ObservableCollection<Recipes>(filtered);
            OnPropertyChanged(nameof(Recipes));
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedDifficulty = "All";
        }

        private void ZoomImage(EgoGift gift)
        {
            if (gift != null)
            {
                ZoomedGift = gift;
                IsImageZoomed = true;
                ImageZoomLevel = 1.0;
            }
        }

        public void CloseImageZoom()
        {
            IsImageZoomed = false;
            ZoomedGift = null;
        }

        private void ZoomIn() => ImageZoomLevel += 0.2;
        private void ZoomOut() => ImageZoomLevel -= 0.2;
        private void ResetZoom() => ImageZoomLevel = 1.0;
    }
}