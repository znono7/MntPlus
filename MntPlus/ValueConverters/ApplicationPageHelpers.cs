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

                case ApplicationPage.Equipement:
                    return new EquipmentPage(viewModel as EquipmentPageViewModel); 
                    case ApplicationPage.ManageWork:
                        return new ManageWorkPage(viewModel as ManageWorkViewModel);
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
            if (page is EquipmentPage)
                return ApplicationPage.Equipement;
            if (page is ManageWorkPage)
                return ApplicationPage.ManageWork;

            // Alert developer of issue
            Debugger.Break();
            return default;
        }

    }
}
