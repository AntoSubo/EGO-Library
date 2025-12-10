using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EGO_Library.ViewModels;

namespace EGO_Library.Views.Controls
{
    public partial class RecipeView : UserControl
    {
        public RecipeView()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is RecipeViewModel viewModel && viewModel.IsImageZoomed)
            {
                if (e.Key == Key.Escape)
                {
                    viewModel.CloseImageZoom();
                    e.Handled = true;
                }
            }
        }

        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is RecipeViewModel viewModel)
            {
                viewModel.CloseImageZoom();
            }
        }
    }
}