using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        IEquipmentService EquipmentService { get; }
        IAssignorService AssignorService { get; }
        IEquipmentDepartmentService EquipmentDepartmentService { get; }
        IEquipmentClassService EquipmentClassService { get; }
        IEquipmentOrganizationService EquipmentOrganizationService { get; }
        IEquipmentStatusService EquipmentStatusService { get; }
        IEquipmentTypeService EquipmentTypeService { get; }

    }
}
