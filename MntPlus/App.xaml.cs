using System.Configuration;
using System.Data;
using System.Windows;

namespace MntPlus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup the main application 
            //ApplicationSetup();


            // Show the main window
            //AddEquipmentWindow window = new AddEquipmentWindow();
            //window.DataContext = new AddEquipmentViewModel();
            Current.MainWindow = new MainWindow();


            Current.MainWindow.Show();
        }
    }

}
