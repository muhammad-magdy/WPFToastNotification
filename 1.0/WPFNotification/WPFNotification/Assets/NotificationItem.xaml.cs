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

namespace WPFNotification.Assets
{
    /// <summary>
    /// Interaction logic for NotificationItem.xaml
    /// </summary>
    public partial class NotificationItem : UserControl
    {
        public NotificationItem()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.Visibility = Visibility.Hidden;
            parentWindow.Close();
        }
    }
}
