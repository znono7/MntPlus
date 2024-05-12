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
using System.Windows.Shapes;

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddTeamWindow : Window
    {
        public AddTeamWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetTop()
        {
            // Get the screen's working area

            var workingArea = SystemParameters.WorkArea;

            // Calculate the horizontal center
            double centerX = (workingArea.Width - ActualWidth) / 2;


            // Set the window's position
            Left = centerX;
            Top = -ActualHeight + 460;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // SetTop();
        }
    }
}
