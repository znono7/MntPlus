using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
   public class ManageWorkViewModel : BaseViewModel
    {
        public ICommand AddWorkOrderCommand { get; set; }

        public bool TaskPopupIsOpen { get; set; }

        public ICommand TaskPopupCommand { get; set; }
        public ViewTaskViewModel TaskViewModel { get; set; } 
        public ManageWorkViewModel()
        {
            AddWorkOrderCommand = new RelayCommand(AddWorkOrder);
            TaskPopupCommand = new RelayCommand(TaskPopup);
            TaskViewModel = new ViewTaskViewModel();
        }

        private void TaskPopup()
        {
            TaskPopupIsOpen = !TaskPopupIsOpen;

        }

        private void AddWorkOrder() 
        {
           var wind =  new SelectEquipmentWindow { DataContext = new SelectEquipmentViewModel() };
            wind.ShowDialog();
        }
    }
}
 