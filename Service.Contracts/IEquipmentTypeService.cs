using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEquipmentTypeService
    {
        Task<ApiBaseResponse> GetAllEquipmentTypesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentTypeAsync(Guid equipmentTypeId, bool trackChanges);
        Task<ApiBaseResponse> CreateEquipmentType(EquipmentTypeDtoCreate equipmentType);
        Task<ApiBaseResponse> DeleteEquipmentType(Guid typeId , bool trackChanges);

    }
}
