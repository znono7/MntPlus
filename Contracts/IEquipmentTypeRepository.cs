using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentTypeRepository
    {
        Task<IEnumerable<EquipmentType>?> GetAllEquipmentTypesAsync(bool trackChanges);
        Task<EquipmentType?> GetEquipmentTypeAsync(Guid equipmentTypeId, bool trackChanges);
        void CreateEquipmentType(EquipmentType equipmentType);
        void DeleteEquipmentType(EquipmentType equipmentType);
    }
}
