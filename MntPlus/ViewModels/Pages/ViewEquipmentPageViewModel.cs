using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input; 

namespace MntPlus.WPF
{
    public class ViewEquipmentPageViewModel : BaseViewModel
    {
        public EquipmentInfoViewModel equipmentInfo { get; set;}
        public AssetChildrenViewModel childrenViewModel { get; set; }
        public EquipmentPartViewModel equipmentPartViewModel { get; set; }
        public ICommand BackPageCommand { get; set; }
        public ViewEquipmentPageViewModel()
        {
            BackPageCommand = new RelayCommand(BackPage);
            equipmentInfo = new EquipmentInfoViewModel();
            childrenViewModel = new AssetChildrenViewModel();
            equipmentPartViewModel = new EquipmentPartViewModel();
        }

        private void BackPage() 
        {
            IoContainer.Application.GoToPage(ApplicationPage.Assets);
            
        }
    }
}
