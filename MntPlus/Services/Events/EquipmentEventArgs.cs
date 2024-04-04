using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus
{
   public class EquipmentEventArgs : EventArgs
    {
        public Equipment? AddEquipment { get; set; }
    }
}
