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
using System.Windows.Shapes;

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for NewEquipmentWindow.xaml
    /// </summary>
    public partial class NewEquipmentWindow : Window
    {
        public NewEquipmentWindow(AssetStore assetStore)
        {
            InitializeComponent();
            DataContext = new NewEquipmentViewModel(this,assetStore);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
