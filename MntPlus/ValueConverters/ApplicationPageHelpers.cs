using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageHelpers
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        
        public static BasePage? ToBasePage(this ApplicationPage page, object? viewModel = null)
        {
            if(viewModel is null)
            {
                viewModel = new ();
            }
            // Find the appropriate page
            switch (page)
            {

                case ApplicationPage.Assets:
                    return new AssetsPage(viewModel as EquipmentPageViewModel); 
                    case ApplicationPage.ManageWork:
                        return new ManageWorkPage(viewModel as ManageWorkViewModel);
                    case ApplicationPage.Users:
                        return new UsersPage(viewModel as UsersPageViewModel);
                    case ApplicationPage.Settings:
                        return new SettingsPage(viewModel as SettingsViewModel);
                    case ApplicationPage.Locations:
                        return new LocationsPage(viewModel as LocationsPageViewModel);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that matches the base page
            if (page is AssetsPage)
                return ApplicationPage.Assets;
            if (page is ManageWorkPage)
                return ApplicationPage.ManageWork;
            if (page is UsersPage)
                return ApplicationPage.Users;
            if (page is SettingsPage)
                return ApplicationPage.Settings;
            if (page is LocationsPage)
                return ApplicationPage.Locations;

            // Alert developer of issue
            Debugger.Break();
            return default;
        }

    }
}
