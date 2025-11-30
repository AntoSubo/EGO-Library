using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace EGO_Library.ViewModels
{
    public class GiftDetailViewModel : BaseViewModel
    {
        private readonly EgoGift _gift;
        private readonly INavigationService _navigationService;
        private readonly DataService _dataService;

        public EgoGift Gift => _gift;
        public ObservableCollection<Recipe> Recipes { get; } = new ObservableCollection<Recipe>();

        public bool HasRecipes => Recipes.Any();

        public ICommand GoBackCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand RefreshCommand { get; }

        public GiftDetailViewModel(EgoGift gift, INavigationService navigationService, DataService dataService = null)
        {
            _gift = gift;
            _navigationService = navigationService;
            _dataService = dataService;

            // Инициализация команд
            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
            HelpCommand = new RelayCommand(_ => ShowHelp());
            RefreshCommand = new RelayCommand(async _ => await LoadRecipesAsync());

            // Загрузка рецептов
            _ = LoadRecipesAsync();
        }

        private async Task LoadRecipesAsync()
        {
            //if (_dataService != null && _gift?.Id > 0)
            //{
            //    try
            //    {
            //        var recipes = await _dataService.GetRecipesByGiftIdAsync(_gift.Id);
            //        Recipes.Clear();
            //        foreach (var recipe in recipes)
            //        {
            //            Recipes.Add(recipe);
            //        }
            //        OnPropertyChanged(nameof(HasRecipes));
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}", "Ошибка");
            //    }
            //}
        }

        private void ShowHelp()
        {
            System.Windows.MessageBox.Show(
                "Детальная информация о даре:\n\n" +
                "- Название и иконка\n" +
                "- Уровень (Tier)\n" +
                "- Статус-эффект\n" +
                "- Основной эффект\n" +
                "- Описание\n" +
                "- Источники получения\n" +
                "- Рецепты слияния (кликабельные)\n\n" +
                "Нажмите на любой рецепт для перехода к списку всех рецептов.",
                "Помощь");
        }
    }
}