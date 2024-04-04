using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MntPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        private void Themes_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AppWindow_Activated(object sender, EventArgs e)
        {
            // Show overlay if we lose focus
            (DataContext as MainWindowViewModel).DimmableOverlayVisible = false;
        }

        private void AppWindow_Deactivated(object sender, EventArgs e)
        {
            // Show overlay if we lose focus
            (DataContext as MainWindowViewModel).DimmableOverlayVisible = true;
        }
    }
}