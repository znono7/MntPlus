using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentClassRepository
    {
        Task<IEnumerable<EquipmentClass>?> GetAllEquipmentClassesAsync(bool trackChanges);
        Task<EquipmentClass?> GetEquipmentClassAsync(Guid equipmentClassId, bool trackChanges);
        void CreateEquipmentClass(EquipmentClass equipmentClass);
        void DeleteEquipmentClass(EquipmentClass equipmentClass);
    }
}
