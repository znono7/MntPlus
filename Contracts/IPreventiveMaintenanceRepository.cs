using Entities;


namespace Contracts
{
    public interface IPreventiveMaintenanceRepository
    {
        Task<IEnumerable<PreventiveMaintenance>?> GetAllPreventiveMaintenancesAsync(bool trackChanges);
        Task<PreventiveMaintenance?> GetPreventiveMaintenanceAsync(Guid preventiveMaintenanceId, bool trackChanges);
        Task<PreventiveMaintenance?> GetPreventiveMaintenanceForDeleteAsync(Guid preventiveMaintenanceId, bool trackChanges);
        void CreatePreventiveMaintenance(PreventiveMaintenance preventiveMaintenance);
        void DeletePreventiveMaintenance(PreventiveMaintenance preventiveMaintenance);
        Task<int> GetNextPreventiveMaintenanceNumberAsync();
        void BulkDeletePreventiveMaintenance(IEnumerable<PreventiveMaintenance> preventiveMaintenances);
        Task<IEnumerable<PreventiveMaintenance>> GetPreventiveMaintenancesByIdsAsync(List<Guid?> preventiveMaintenanceIds, bool trackChanges);
    }
}
