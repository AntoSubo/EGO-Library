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
        public ObservableCollection<Recipes> Recipes { get; } = new ObservableCollection<Recipes>();

        public bool HasRecipes => Recipes.Any();

        public ICommand GoBackCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand RefreshCommand { get; }

        public GiftDetailViewModel(EgoGift gift, INavigationService navigationService, DataService dataService = null)
        {
            _gift = gift;
            _navigationService = navigationService;
            _dataService = dataService;

            // инициализация команд
            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
            HelpCommand = new RelayCommand(_ => ShowHelp());
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