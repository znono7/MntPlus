using Entities;
using Shared;

namespace Service.Contracts
{ 
    public interface IEquipmentService
    { 
       // Task<IEnumerable<EquipmentDto?>?> GetAllEquipmentsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAllEquipmentsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentAsync(Guid equipmentId, bool trackChanges);
        //Task<EquipmentDto?> CreateEquipmentAsync(EquipmentForCreationDto equipment);
        Task<ApiBaseResponse> CreateEquipmentAsync(EquipmentForCreationDto equipment);
        //Task<EquipmentDto?> UpdateEquipmentAsync(Guid equipmentId, EquipmentForUpdateDto equipment);
        //Task<ApiBaseResponse> UpdateEquipmentAsync(Guid equipmentId, EquipmentForUpdateDto equipment);
        Task<ApiBaseResponse> DeleteEquipmentAsync(Guid equipmentId,bool trackChanges); 
        Task<ApiBaseResponse> UpdateEquipmentAsync(Guid equipmentId, EquipmentForUpdateDto equipmentForUpdate, bool trackChanges);
        Task<ApiBaseResponse> UpdateEquipmentImageAsync(Guid equipmentId, EquipmentForImageUpdateDto equipmentImage, bool trackChanges);
    }
}
