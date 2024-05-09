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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for LocationControl.xaml
    /// </summary>
    public partial class LocationControl : UserControl
    {
        public LocationControl()
        {
            InitializeComponent();
        }

        private bool isExpanded { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isExpanded = !isExpanded;
            var viewModel = DataContext as PrimaryLocationViewModel;
            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                To = isExpanded ? 180 : 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            
                
            viewModel?.ExpandCommand?.Execute(null);
        }
    }
}
