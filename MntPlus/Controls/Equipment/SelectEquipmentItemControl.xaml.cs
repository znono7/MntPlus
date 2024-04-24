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

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for EquipmentControl.xaml
    /// </summary>
    public partial class SelectEquipmentItemControl : UserControl
    {
        public SelectEquipmentItemControl() 
        {
            InitializeComponent();
        }

        private T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && (element as FrameworkElement).Name == name)
                {
                    return element;
                }
                else
                {
                    var result = FindVisualChild<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        private void AddDescBtn_Click(object sender, RoutedEventArgs e)
        {
            var myTextBox = FindVisualChild<TextBox>(this, "DescTxt");
            if (myTextBox is TextBox)
            {
                myTextBox.Visibility = Visibility.Visible;
                myTextBox.Focus();
            }
        }
    }
}
