using EGO_Library.Data;
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

            try
            {
                // Создаем и инициализируем базу данных
                using var context = new AppDbContext();
                context.Database.EnsureCreated();

                // Инициализируем сервис данных
                var dataService = new DataService(context);

                // Создаем и показываем главное окно
                var mainWindow = new MainWindow();
                mainWindow.DataContext = new MainViewModel(dataService);
                mainWindow.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка запуска приложения: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}