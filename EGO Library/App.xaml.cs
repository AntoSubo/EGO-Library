using System.Windows;
using EGO_Library.ViewModels;
using EGO_Library.Services; // если у тебя есть NavigationService
using EGO_Library.Views;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            var navigation = new NavigationService(mainWindow.MainFrame);

            mainWindow.DataContext = new GiftListViewModel(navigation);

            mainWindow.Show();
        }
    }
}
