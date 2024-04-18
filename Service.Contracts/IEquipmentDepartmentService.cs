using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEquipmentDepartmentService
    {
        Task<ApiBaseResponse> GetAllEquipmentDepartmentsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentDepartmentAsync(Guid equipmentDepartmentId, bool trackChanges);
        Task<ApiBaseResponse> CreateEquipmentDepartment(EquipmentDepartmentCreateDto equipmentDepartment);
        Task<ApiBaseResponse> DeleteEquipmentDepartment(Guid equipmentDepartmentId, bool trackChanges);
      

    }
}
