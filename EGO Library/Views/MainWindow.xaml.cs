using System.Windows;
using System.Windows.Controls;
using EGO_Library.Controls;

namespace EGO_Library.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Подгружаем стартовый экран
            MainContent.Content = new GiftListView();
        }

        // Метод для навигации на другой UserControl
        public void Navigate(UserControl nextView)
        {
            MainContent.Content = nextView;
        }
    }
}
