using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentRepository
    { 
        IEnumerable<Equipment> GetAllEquipments(bool trackChanges);
        Equipment? GetEquipment(Guid equipmentId, bool trackChanges);
        void CreateEquipment(Equipment equipment);
    }
}
