using Entities;
using Shared;

namespace Service.Contracts
{
    public interface IPreventiveMaintenanceService
    {
        Task<ApiBaseResponse> GetAllPreventiveMaintenancesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetPreventiveMaintenanceAsync(Guid preventiveMaintenanceId, bool trackChanges);
        Task<ApiBaseResponse> CreatePreventiveMaintenance(PreventiveMaintenanceDtoForCreation preventiveMaintenance);
        Task<ApiBaseResponse> CreateLastNumberPreventiveMaintenance();
        Task<ApiBaseResponse> DeletePreventiveMaintenance(Guid preventiveMaintenanceId, bool trackChanges);
        Task<ApiBaseResponse> BulkDeletePreventiveMaintenance(List<Guid?> preventiveMaintenanceIds, bool trackChanges);
        Task<ApiBaseResponse> UpdatePreventiveMaintenance(Guid preventiveMaintenanceId, PreventiveMaintenanceDtoForCreation preventiveMaintenance, bool trackChanges);
        Task<ApiBaseResponse> UpdateStatPreventiveMaintenance(Guid preventiveMaintenanceId, string preventiveMaintenanceStat, bool trackChanges);
    }
}
