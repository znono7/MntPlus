using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class EquipmentStore
    {
        public event Action<EquipmentDto?>? EquipmentCreated;
        public event Action<EquipmentDto?>? EquipmentUpdated;
        public void CreateEquipment(EquipmentDto? equipment)
        {
            EquipmentCreated?.Invoke(equipment);
        }

        public void UpdateEquipment(EquipmentDto? equipment)
        {
            EquipmentUpdated?.Invoke(equipment);
        }
    }
}
