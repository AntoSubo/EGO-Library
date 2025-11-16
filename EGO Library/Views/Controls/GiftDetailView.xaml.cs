using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EGO_Library.Views.Controls
{
    public partial class GiftDetailView : UserControl
    {
        public GiftDetailView()
        {
            InitializeComponent();
        }

        private void RecipeItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToRecipes();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToGiftList();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gift Details Help\n\n• Click on recipes to view crafting details", "Help");
        }
    }
}