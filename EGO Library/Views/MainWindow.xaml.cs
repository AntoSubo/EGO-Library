using EGO_Library.Views.Controls;
using System.Windows;

namespace EGO_Library.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Просто показываем один UserControl
            MainContent.Content = new GiftListView();
        }
    }
}