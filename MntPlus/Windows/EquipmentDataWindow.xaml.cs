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
    public partial class EquipmentDataWindow : Window
    {
        public EquipmentDataWindow(EquipmentDataViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void editImg_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void editImg_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
