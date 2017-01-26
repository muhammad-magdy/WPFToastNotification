using FirstFloor.ModernUI.Windows.Controls;
using WPFNotificationDemo.ViewModel;

namespace WPFNotificationDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}
