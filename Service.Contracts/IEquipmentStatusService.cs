using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEquipmentStatusService
    {
        Task<ApiBaseResponse> GetAllEquipmentStatusesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentStatusAsync(Guid equipmentStatusId, bool trackChanges);
        Task<ApiBaseResponse> CreateEquipmentStatus(EquipmentStatusDtoCreate equipmentStatus);
        Task<ApiBaseResponse> DeleteEquipmentStatus(Guid equipmentStatusId, bool trackChanges);
    }
}
