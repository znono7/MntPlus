﻿using System;
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
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : BasePage<InventoryPageViewModel>
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        public InventoryPage(InventoryPageViewModel model)  : base(model)
        {
            InitializeComponent();
        }
    }
}
