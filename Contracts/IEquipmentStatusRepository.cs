using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentStatusRepository
    {
        Task<IEnumerable<EquipmentStatus>?> GetAllEquipmentStatusAsync(bool trackChanges);
        Task<EquipmentStatus?> GetEquipmentStatusByIdAsync(Guid equipmentStatusId , bool trackChanges);
        void CreateEquipmentStatus(EquipmentStatus equipmentStatus);

        void DeleteEquipmentStatus(EquipmentStatus equipmentStatus);

    }
}
