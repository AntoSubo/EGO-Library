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
                await DatabaseInitializer.CreateDatabaseAsync();

                var authService = new AuthService();
                var dataService = new DataService();

                var mainViewModel = new MainViewModel(authService, dataService);

                MainWindow = new MainWindow { DataContext = mainViewModel };
                MainWindow.Show();

            }
            catch (Exception ex)
            {
                Shutdown();
            }
        }
    }
}