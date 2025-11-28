using EGO_Library.Services;
using EGO_Library.ViewModels;
using EGO_Library.Views;
using System.Windows;
using System.Threading.Tasks;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализируем базу данных
            await DatabaseInitializer.InitializeAsync();

            // Создаем сервисы
            var authService = new AuthService();
            var dataService = new DataService();

            // Создаем MainViewModel
            var mainViewModel = new MainViewModel(authService, dataService);

            // Создаем и показываем главное окно
            MainWindow = new MainWindow { DataContext = mainViewModel };
            MainWindow.Show();
        }
    }
}