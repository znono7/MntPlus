using Shared;
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
    /// Interaction logic for EquipmentInfoControl.xaml
    /// </summary>
    public partial class EquipmentInfoControl : UserControl
    {
        public EquipmentInfoControl()
        {
            InitializeComponent();
            
        }

        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersDataGrid.SelectedItem is not null)
            {
                var model = (EquipmentAssignedToDto)usersDataGrid.SelectedItem;
                var viewModel = (EquipmentInfoViewModel)DataContext;
                viewModel.SelectedAssignee = model;
            }
               
        }
    }
}
