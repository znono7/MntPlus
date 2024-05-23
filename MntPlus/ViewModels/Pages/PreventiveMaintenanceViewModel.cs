using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class PreventiveMaintenanceViewModel : BaseViewModel
    {
        public ICommand CreatePmCommand { get; set; }



        public PreventiveMaintenanceViewModel()
        {
            CreatePmCommand = new RelayCommand(() => CreatePm());
        }

        private void CreatePm()
        {
            IoContainer.Application.GoToPage(ApplicationPage.NewPreventiveMaintenance);
        }
    }
}
