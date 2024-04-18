using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentDepartmentRepository
    {
        Task<IEnumerable<EquipmentDepartment>?> GetAllEquipmentDepartmentsAsync(bool trackChanges);
        Task<EquipmentDepartment?> GetEquipmentDepartmentAsync(Guid equipmentDepartmentId, bool trackChanges);
        void CreateEquipmentDepartment(EquipmentDepartment equipmentDepartment);
        void DeleteEquipmentDepartment(EquipmentDepartment equipmentDepartment);
    }
}
