using EGO_Library.ViewModels;
using System.Windows.Controls;

namespace EGO_Library.Views.Controls
{
    public partial class GiftListView : UserControl
    {
        public GiftListView()
        {
            InitializeComponent();
            DataContext = new GiftListViewModel(); // Важно
        }
    }
}