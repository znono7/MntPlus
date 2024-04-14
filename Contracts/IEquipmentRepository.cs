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
        Task<IEnumerable<Equipment>> GetAllEquipmentsAsync(bool trackChanges);
        Task<Equipment?> GetEquipmentAsync(Guid equipmentId, bool trackChanges);
        void CreateEquipment(Equipment equipment);
    }
}
