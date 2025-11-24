using EGO_Library.Services;
using EGO_Library.ViewModels;
using EGO_Library.Views;
using System.Windows;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем сервисы
            var dataService = new DataService();

            // Создаем MainViewModel
            var mainViewModel = new MainViewModel(dataService);

            // Создаем и показываем главное окно
            MainWindow = new MainWindow { DataContext = mainViewModel };
            MainWindow.Show();
        }
    }
}