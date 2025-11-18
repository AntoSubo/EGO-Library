using EGO_Library.Models;
using EGO_Library.Services;
using EGO_Library.Views.Controls;
using System.Collections.ObjectModel;
using System.Linq;
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

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;

            // Команды навигации
            ShowGiftListCommand = new RelayCommand(_ => ShowGiftList());
            ShowRecipesCommand = new RelayCommand(_ => ShowRecipes());
            ShowAboutCommand = new RelayCommand(_ => ShowAbout());

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
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void AddSampleData()
        {
            var sampleGifts = new List<EgoGift>
            {
                new EgoGift("Wealth", 4, "Charge", "💰", "Increases max Charge by 2"),
                new EgoGift("Inferno", 3, "Burn", "🔥", "Applies Burn status each turn"),
                new EgoGift("Fortitude", 2, "Defense", "🛡️", "Reduces incoming damage by 15%"),
                new EgoGift("Precision", 3, "Poise", "🎯", "Increases critical hit chance"),
                new EgoGift("Vitality", 1, "Health", "❤️", "Restores HP each turn")
            };

            foreach (var gift in sampleGifts)
            {
                _dataService.AddGift(gift);
            }

            // Перезагружаем данные
            var updatedGifts = _dataService.GetAllGifts();
            Gifts.Clear();
            foreach (var gift in updatedGifts)
            {
                Gifts.Add(gift);
            }
        }

        private void ShowGiftList()
        {
            CurrentView = new GiftListView();
        }

        private void ShowRecipes()
        {
            CurrentView = new RecipeView();
        }

        private void ShowAbout()
        {
            CurrentView = new AboutView();
        }
    }
}