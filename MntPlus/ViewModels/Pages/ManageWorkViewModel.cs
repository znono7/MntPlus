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

        public ManageWorkViewModel()
        {
            AddWorkOrderCommand = new RelayCommand(AddWorkOrder);
        }

        private void AddWorkOrder()
        {
           var wind =  new SelectEquipmentWindow { DataContext = new SelectEquipmentViewModel() };
            wind.ShowDialog();
        }
    }
}
 