using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views.Controls
{
    public partial class RecipeView : UserControl
    {
        public RecipeView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToGiftDetail(); // Возврат к деталям дара
        }
    }
}