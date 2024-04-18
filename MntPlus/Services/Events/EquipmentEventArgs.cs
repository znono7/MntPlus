using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
   public class EquipmentIemEventArgs : EventArgs
    {
        public EquipmentItem? AddEquipment { get; set; }
    }

    public class EquipmentEventArgs : EventArgs
    {
        public Equipment? AddEquipment { get; set; }
    }
    public class EquipmentDtoEventArgs : EventArgs
    {
        public EquipmentDto? AddEquipmentDto { get; set; }
    }

}
