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

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
       
        public bool Confirmed { get; private set; }
        public string? EquipmentName { get; set; }
        public string? TitleWindow { get; set; }
        public string? Message { get; set; } 

        public ConfirmationWindow(string? _EquipmentName , string? message = "Vous êtes maintenant sûr du processus de suppression?") 
        {
            InitializeComponent();
            DataContext = this;
           
            Confirmed = false;
            //EquipmentName = _EquipmentName;
            TitleWindow = _EquipmentName;
            Message = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetTop();
        }

        private void SetTop()
        {
            // Get the screen's working area

            var workingArea = SystemParameters.WorkArea;

            // Calculate the horizontal center
            double centerX = (workingArea.Width - ActualWidth) / 2;


            // Set the window's position
            Left = centerX;
            Top = -ActualHeight + 320;
        }
    }
}