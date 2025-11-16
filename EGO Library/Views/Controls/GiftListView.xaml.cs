using EGO_Library.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EGO_Library.Views.Controls
{
    public partial class GiftListView : UserControl
    {
        public GiftListView()
        {
            InitializeComponent();
            DataContext = new GiftListViewModel();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToAbout();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("EGO Library Help\n\n• Click on gifts to view details\n• Use filters to find specific gifts", "Help");
        }

        private void GiftItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToGiftDetail();
        }
    }
}