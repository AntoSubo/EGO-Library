using EGO_Library.Services;
using EGO_Library.ViewModels;
using EGO_Library.Views;
using System.Windows;
using System.Threading.Tasks;
using EGO_Library.Data;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {

                // ПРИНУДИТЕЛЬНО ИНИЦИАЛИЗИРУЕМ БАЗУ ДАННЫХ
                await DatabaseInitializer.CreateDatabaseAsync();


                // Создаем сервисы
                var authService = new AuthService();
                var dataService = new DataService();

                // ТЕСТИРУЕМ ЗАГРУЗКУ ДАННЫХ
                var testGifts = await dataService.GetGiftsAsync();
                Console.WriteLine($"TEST: Successfully loaded {testGifts.Count} gifts from database");

                // Создаем MainViewModel
                var mainViewModel = new MainViewModel(authService, dataService);

                // Создаем и показываем главное окно
                MainWindow = new MainWindow { DataContext = mainViewModel };
                MainWindow.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Ошибка запуска приложения: {ex.Message}", "Ошибка");
                Shutdown();
            }
        }
    }
}