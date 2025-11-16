using EGO_Library.Commands;
using EGO_Library.Views;
using System.Windows;

namespace EGO_Library.Services
{
    public class NavigationCommands
    {
        private static MainWindow MainWindow => Application.Current.MainWindow as MainWindow;

        public static RelayCommand NavigateToGiftListCommand { get; } = new RelayCommand(
            _ => MainWindow?.NavigateToGiftList());

        public static RelayCommand NavigateToRecipesCommand { get; } = new RelayCommand(
            _ => MainWindow?.NavigateToRecipes());

        public static RelayCommand NavigateToAboutCommand { get; } = new RelayCommand(
            _ => MainWindow?.NavigateToAbout());

        public static RelayCommand NavigateToGiftDetailCommand { get; } = new RelayCommand(
            _ => MainWindow?.NavigateToGiftDetail());
    }
}