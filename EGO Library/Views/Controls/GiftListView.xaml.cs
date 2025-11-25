using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EGO_Library.Models;
using EGO_Library.ViewModels;

namespace EGO_Library.Views.Controls
{
    public partial class GiftListView : UserControl
    {
        public GiftListView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is EgoGift gift)
            {
                var viewModel = DataContext as GiftListViewModel;
                if (viewModel != null && viewModel.SelectGiftCommand.CanExecute(gift))
                {
                    viewModel.SelectGiftCommand.Execute(gift);
                }
            }
        }
    }
}