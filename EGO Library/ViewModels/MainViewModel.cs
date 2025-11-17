using EGO_Library.Models;
using EGO_Library.Services;
using EGO_Library.Views.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace EGO_Library.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EgoGift> Gifts { get; set; } = new ObservableCollection<EgoGift>();

        public RelayCommand ShowGiftListCommand { get; }
        public RelayCommand ShowRecipesCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand LoadDataCommand { get; }

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;

            // Команды
            ShowGiftListCommand = new RelayCommand(_ => ShowGiftList());
            ShowRecipesCommand = new RelayCommand(_ => ShowRecipes());
            ShowAboutCommand = new RelayCommand(_ => ShowAbout());
            LoadDataCommand = new RelayCommand(_ => LoadData());

            // Загружаем данные и показываем список даров
            LoadData();
            ShowGiftList();
        }

        private void LoadData()
        {
            try
            {
                var gifts = _dataService.GetAllGifts();
                Gifts.Clear();
                foreach (var gift in gifts)
                {
                    Gifts.Add(gift);
                }

                // Если нет данных - добавляем демо-данные
                if (!Gifts.Any())
                {
                    AddSampleData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void AddSampleData()
        {
            var sampleGifts = new List<EgoGift>
            {
                new EgoGift
                {
                    Name = "Wealth",
                    Tier = 4,
                    Status = "Charge",
                    Icon = "💰",
                    Description = "Increases max Charge by 2"
                },
                new EgoGift
                {
                    Name = "Inferno",
                    Tier = 3,
                    Status = "Burn",
                    Icon = "🔥",
                    Description = "Applies Burn status each turn"
                },
                new EgoGift
                {
                    Name = "Fortitude",
                    Tier = 2,
                    Status = "Defense",
                    Icon = "🛡️",
                    Description = "Reduces incoming damage by 15%"
                }
            };

            foreach (var gift in sampleGifts)
            {
                _dataService.AddGift(gift);
            }

            LoadData(); // Перезагружаем данные
        }

        private void ShowGiftList()
        {
            var giftListView = new GiftListView();
            if (giftListView.DataContext is GiftListViewModel giftListVm)
            {
                giftListVm.Gifts = Gifts;
            }
            CurrentView = giftListView;
        }

        private void ShowRecipes()
        {
            var recipeView = new RecipeView();
            if (recipeView.DataContext is RecipeViewModel recipeVm)
            {
                // Загружаем рецепты если нужно
            }
            CurrentView = recipeView;
        }

        private void ShowAbout()
        {
            CurrentView = new AboutView();
        }
    }
}