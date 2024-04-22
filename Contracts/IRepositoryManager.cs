using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IEquipmentRepository Equipment { get; } 
        IAssignorRepository Assignor { get; }

        IEquipmentClassRepository EquipmentClass { get; }
        IEquipmentDepartmentRepository EquipmentDepartment { get; } 
        IEquipmentOrganizationRepository EquipmentOrganization { get; }
        IEquipmentStatusRepository EquipmentStatus { get; }
        IEquipmentTypeRepository EquipmentType { get; }

       Task SaveAsync(); 
    }
}
