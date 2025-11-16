using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views.Controls
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToGiftList(); // Возврат к списку даров
        }
    }
}