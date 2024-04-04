using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus
{
   public class Equipment
    {
        public string EquipmentId { get; set; }
        public string? EquipmentParent { get; set; }
        public string EquipmentName { get; set; }
        public string? EquipmentCategory { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }


    }
}
