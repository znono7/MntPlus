using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MntPlus.WPF
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
            ApplicationSetup();




            Current.MainWindow = /*new NewEquipmentWindow();*/ new MainWindow();


            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            AppServices.Setup();
            IoContainer.Setup();
        }
    }

}
