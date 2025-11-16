using EGO_Library.Services;
using System.Windows;

namespace EGO_Library
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализируем базу данных
            await DatabaseInitializer.InitializeAsync();
        }
    }
}