using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Controls
{
    public partial class RecipeView : UserControl
    {
        private string giftName;

        public RecipeView(string giftName)
        {
            InitializeComponent();
            this.giftName = giftName;
            RecipeTitle.Text = $"Recipe for {giftName}";
            ComponentsText.Text = "Components: Rusty Blade + Sun Fragment"; // пример
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Navigate(new GiftDetailView(giftName));
        }
    }
}
