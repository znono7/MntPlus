using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class NewPreventiveMaintenanceViewModel : BaseViewModel
    {
        public ICommand BackPageCommand { get; set; }
        public ICommand ScheduleWindCommand { get; set; }

        public PmTypesColllection? TypesColllection { get; set; }
        public PmTypes? SelectedType { get; set; }

         
        public NewPreventiveMaintenanceViewModel()
        {
            TypesColllection = new PmTypesColllection();
            BackPageCommand = new RelayCommand(() => BackPage());
            ScheduleWindCommand = new RelayCommand(() => ScheduleWind());
        }

        private void BackPage()
        {
            IoContainer.Application.GoToPage(ApplicationPage.PreventiveMaintenance);
        }

        private void ScheduleWind()
        {
            ScheduleTimeWindow scheduleTimeWindow = new () { DataContext = new ScheduleTimeViewModel() };
            scheduleTimeWindow.ShowDialog();
        }
    }
}
