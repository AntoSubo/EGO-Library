using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Controls
{
    public partial class GiftDetailView : UserControl
    {
        private string giftName;

        public GiftDetailView(string giftName)
        {
            InitializeComponent();
            this.giftName = giftName;
            GiftNameText.Text = giftName;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Navigate(new GiftListView());
        }

        private void ViewRecipe_Click(object sender, RoutedEventArgs e)
        {
            var recipeView = new RecipeView(giftName);
            ((MainWindow)Application.Current.MainWindow).Navigate(recipeView);
        }
    }
}
