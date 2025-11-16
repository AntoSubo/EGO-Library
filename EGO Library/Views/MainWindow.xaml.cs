using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Frame FrameControl => MainFrame;
    }
}
