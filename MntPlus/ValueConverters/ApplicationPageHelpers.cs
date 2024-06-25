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
            if(viewModel == null)
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
                    case ApplicationPage.Teams:
                        return new TeamsPage(viewModel as TeamsPageViewModel);
                    case ApplicationPage.Requests:
                        return new RequestsPage(viewModel as RequestsPageViewModel);
                    case ApplicationPage.PreventiveMaintenance:
                        return new PreventiveMaintenancePage(viewModel as PreventiveMaintenanceViewModel);
                    case ApplicationPage.PartsInventory:
                        return new PartsInventoryPage(viewModel as PartsInventoryViewModel);
                    case ApplicationPage.NewPreventiveMaintenance:
                        return new NewPreventiveMaintenancePage(viewModel as NewPreventiveMaintenanceViewModel);
                    case ApplicationPage.Equipment:
                        return new EquipmentPage(viewModel as ViewEquipmentPageViewModel);
                    case ApplicationPage.Inventory:
                        return new InventoryPage(viewModel as InventoryPageViewModel);
                    case ApplicationPage.Meters:
                        return new MeterPage(viewModel as MeterPageViewModel);
                    case ApplicationPage.Checklists:
                        return new ChecklistsPage(viewModel as ChecklistsViewModel);
                    case ApplicationPage.NewAsset:
                        return new NewAssetPage(viewModel as NewAssetViewModel);


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
            if (page is TeamsPage)
                return ApplicationPage.Teams;
            if (page is RequestsPage)
                return ApplicationPage.Requests;
            if (page is PreventiveMaintenancePage)
                return ApplicationPage.PreventiveMaintenance;
            if (page is PartsInventoryPage)
                return ApplicationPage.PartsInventory;
            if (page is NewPreventiveMaintenancePage)
                return ApplicationPage.NewPreventiveMaintenance;
            if (page is EquipmentPage)
                return ApplicationPage.Equipment;
            if (page is InventoryPage)
                return ApplicationPage.Inventory;
            if (page is MeterPage)
                return ApplicationPage.Meters;
            if (page is ChecklistsPage)
                return ApplicationPage.Checklists;
            if (page is NewAssetPage)
                return ApplicationPage.NewAsset;

            // Alert developer of issue
            Debugger.Break();
            return default;
        }

    }
}
