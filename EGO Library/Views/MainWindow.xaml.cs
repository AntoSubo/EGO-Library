using EGO_Library.Views.Controls;
using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToGiftList();
        }

        public void NavigateToGiftList()
        {
            MainContent.Content = new GiftListView();
        }

        public void NavigateToRecipes()
        {
            MainContent.Content = new RecipeView();
        }

        public void NavigateToAbout()
        {
            MainContent.Content = new AboutView();
        }

        public void NavigateToGiftDetail()
        {
            MainContent.Content = new GiftDetailView();
        }

        public ContentControl MainContentControl => MainContent;
    }
}