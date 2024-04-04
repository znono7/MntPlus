using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DXApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //BingGeocodeDataProvider geocodeProvider = new BingGeocodeDataProvider();
            //geocodeProvider.BingKey = "sSKcFJDPTu4CTLP8JnOm~iESaBQxmxIRyy0SiX9oecQ~Atj4d8_0S8F06t76YHusoz6FAxn_UoHQp373Zs9b4epfP-6OmGpQUKRuj-mPYeMa"; // Replace with your Bing Maps API key
            //GeoPoint location = new GeoPoint(40, -120);
            //geocodeProvider.RequestLocationInformation(location);
        }
    }
}
