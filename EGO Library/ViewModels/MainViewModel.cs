using EGO_Library.Commands;
using EGO_Library.Models;
using EGO_Library.Views.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace EGO_Library.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EgoGift> Gifts { get; set; }
        //public RelayCommand<EgoGift> SelectGiftCommand { get; }
        public RelayCommand ShowGiftListCommand { get; }
        public RelayCommand ShowRecipesCommand { get; }
        public RelayCommand ShowAboutCommand { get; }

        public MainViewModel()
        {
            // Стартовая страница - список даров
            CurrentView = new GiftListView();

            LoadGifts();
            //SelectGiftCommand = new RelayCommand<EgoGift>(SelectGift);
            ShowGiftListCommand = new RelayCommand(_ => ShowGiftList());
            ShowRecipesCommand = new RelayCommand(_ => ShowRecipes());
            ShowAboutCommand = new RelayCommand(_ => ShowAbout());
        }

        private void LoadGifts()
        {
            Gifts = new ObservableCollection<EgoGift>
            {
                new EgoGift {
                    Name = "Wealth",
                    Tier = 4,
                    Status = "Charge",
                    Icon = "💰",
                    Description = "Increases max Charge by 2",
                    Sources = new List<string> { "Mirror Dungeon Floor 5" }
                },
                new EgoGift {
                    Name = "Inferno",
                    Tier = 3,
                    Status = "Burn",
                    Icon = "🔥",
                    Description = "Applies Burn status each turn",
                    Sources = new List<string> { "Mirror Dungeon Floor 3" }
                }
            };
        }

        private void SelectGift(EgoGift gift)
        {
            // Переход на детальную страницу дара
            var detailView = new GiftDetailView();
            if (detailView.DataContext is GiftDetailViewModel detailVm)
            {
                // Здесь передаем данные о даре
            }
            CurrentView = detailView;
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