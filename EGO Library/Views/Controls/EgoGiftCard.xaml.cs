using EGO_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EGO_Library.Controls
{
    public partial class EgoGiftCard : UserControl
    {
            public EgoGiftCard()
            {
                InitializeComponent();
            }

            public static readonly DependencyProperty GiftProperty =
                DependencyProperty.Register("Gift", typeof(object), typeof(EgoGiftCard));

            public object Gift
            {
                get { return GetValue(GiftProperty); }
                set { SetValue(GiftProperty, value); }
            }
        
    }
}
