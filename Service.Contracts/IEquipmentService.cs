using Entities;
using Shared;

namespace Service.Contracts
{ 
    public interface IEquipmentService
    { 
       // Task<IEnumerable<EquipmentDto?>?> GetAllEquipmentsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAllEquipmentsAsync(bool trackChanges);
        Task<EquipmentDto?> GetEquipmentAsync(Guid equipmentId, bool trackChanges);
        //Task<EquipmentDto?> CreateEquipmentAsync(EquipmentForCreationDto equipment);
        Task<ApiBaseResponse> CreateEquipmentAsync(EquipmentForCreationDto equipment);
    }
}
