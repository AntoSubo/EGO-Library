using EGO_Library.Services;
using EGO_Library.ViewModels;
using EGO_Library.Views;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Обработка необработанных исключений
                AppDomain.CurrentDomain.UnhandledException += (s, args) =>
                    HandleUnhandledException(args.ExceptionObject as Exception);

                TaskScheduler.UnobservedTaskException += (s, args) =>
                    HandleUnhandledException(args.Exception);

                // Инициализация базы данных
                await InitializeDatabaseAsync();

                // Создаем сервисы
                var dataService = new DataService();

                // Создаем и показываем главное окно
                MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(dataService)
                };
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                HandleUnhandledException(ex);
            }
        }

        private async Task InitializeDatabaseAsync()
        {
            try
            {
                // Проверяем статус базы
                await DatabaseInitializer.CheckDatabaseStatusAsync();

                // Инициализируем базу (создаст и заполнит если нужно)
                await DatabaseInitializer.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize database: {ex.Message}\n\nApplication will continue but data may not be available.",
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void HandleUnhandledException(Exception ex)
        {
            MessageBox.Show(
                $"Critical error: {ex?.Message ?? "Unknown error"}\n\nApplication will close.",
                "Fatal Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown(1);
        }
    }
}