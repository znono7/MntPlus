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

namespace MntPlus
{
    /// <summary>
    /// Interaction logic for EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : BasePage<EquipmentPageViewModel>
    {
        public EquipmentPage()
        {
            InitializeComponent();
        }

        public EquipmentPage(EquipmentPageViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
