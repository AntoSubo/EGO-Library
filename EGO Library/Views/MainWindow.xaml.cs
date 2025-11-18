using EGO_Library.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // При загрузке окна привязываем CurrentView к ContentControl
            if (DataContext is MainViewModel viewModel)
            {
                var binding = new System.Windows.Data.Binding("CurrentView");
                MainContent.SetBinding(ContentControl.ContentProperty, binding);
            }
        }

        // Методы навигации для кнопок
        public void NavigateToGiftList()
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.ShowGiftListCommand.Execute(null);
            }
        }

        public void NavigateToRecipes()
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.ShowRecipesCommand.Execute(null);
            }
        }

        public void NavigateToAbout()
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.ShowAboutCommand.Execute(null);
            }
        }

        public void NavigateToGiftDetail()
        {
            // Временная заглушка - переходим к списку
            NavigateToGiftList();
        }
    }
}