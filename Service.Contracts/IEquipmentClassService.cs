using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEquipmentClassService
    {
        Task<ApiBaseResponse> GetAllEquipmentClassesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentClassAsync(Guid equipmentClassId, bool trackChanges);
        Task<ApiBaseResponse> CreateEquipmentClass(EquipmentClassForCreationDto equipmentClass);
        Task<ApiBaseResponse> DeleteEquipmentClass(Guid equipmentClassId, bool trackChanges);
    }
}
