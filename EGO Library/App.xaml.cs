// App.xaml.cs
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
                // Create and initialize database
                using var context = new AppDbContext();
                context.Database.EnsureCreated();

                // Initialize services
                //var dataService = new DataService(context);

                // Create and show main window
                MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(dataService)
                };
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start application: {ex.Message}",
                              "Startup Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}