using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Controls
{
    public partial class GiftListView : UserControl
    {
        public GiftListView()
        {
            InitializeComponent();
        }

        private void OpenGiftDetail_Click(object sender, RoutedEventArgs e)
        {
            // Переход на детали подарка
            var detailView = new GiftDetailView("Rusty Blade"); // пример выбранного подарка
            ((MainWindow)Application.Current.MainWindow).Navigate(detailView);
        }
    }
}
