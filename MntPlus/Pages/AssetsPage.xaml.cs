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
    /// Interaction logic for EquipmentPage.xaml
    /// </summary>
    public partial class AssetsPage : BasePage<EquipmentPageViewModel>
    {
        public AssetsPage()
        { 
            InitializeComponent();
        }

        public AssetsPage(EquipmentPageViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
