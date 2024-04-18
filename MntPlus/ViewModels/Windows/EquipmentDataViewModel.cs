using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class EquipmentDataViewModel : BaseViewModel
    {

        public EquipmentInfoViewModel? EquipmentInfoViewModel { get; set; }
        public EquipmentDto Equipment { get; set; }

        public EquipmentDataViewModel(/*EquipmentDto equipment*/)
        {
           // Equipment = equipment;
            EquipmentInfoViewModel = new EquipmentInfoViewModel(/*equipment*/);
        }
    }
}
 