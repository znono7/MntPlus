using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PreventiveMaintenanceRepository : RepositoryBase<PreventiveMaintenance>, IPreventiveMaintenanceRepository
    {
        public PreventiveMaintenanceRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void BulkDeletePreventiveMaintenance(IEnumerable<PreventiveMaintenance> preventiveMaintenances) => DeleteBulk(preventiveMaintenances);
       

        public void CreatePreventiveMaintenance(PreventiveMaintenance preventiveMaintenance) => Create(preventiveMaintenance);
       

        public void DeletePreventiveMaintenance(PreventiveMaintenance preventiveMaintenance) => Delete(preventiveMaintenance);
       

        public async Task<IEnumerable<PreventiveMaintenance>?> GetAllPreventiveMaintenancesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(p => p.UserAssignedTo) 
            .Include(p => p.TeamAssignedTo)
            .Include(p => p.Asset)
            .ThenInclude(a => a!.Location)
            .Include(p => p.UserCreatedBy)
            .Include(p => p.Schedule)
            .Include(p => p.MeterSchedule)
            .ThenInclude(m => m!.Meter)
            .OrderBy(c => c.Number)
            .ToListAsync();
       

        public async Task<int> GetNextPreventiveMaintenanceNumberAsync()
        {
            var maxNumber = await FindByCondition(w => true, false)
                            .MaxAsync(wo => (int?)wo.Number) ?? 0; 
            return maxNumber + 1;
            
        }

        public async Task<PreventiveMaintenance?> GetPreventiveMaintenanceAsync(Guid preventiveMaintenanceId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(preventiveMaintenanceId), trackChanges)
            .Include(p => p.UserAssignedTo)
            .Include(p => p.TeamAssignedTo)
            .Include(p => p.Asset)
            .ThenInclude(a => a!.Location)
            .Include(p => p.UserCreatedBy)
            .Include(p => p.Schedule)
            .Include(p => p.MeterSchedule)
            .ThenInclude(m => m!.Meter)
            .OrderBy(c => c.Name)
            .SingleOrDefaultAsync();
       

        public async Task<PreventiveMaintenance?> GetPreventiveMaintenanceForDeleteAsync(Guid preventiveMaintenanceId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(preventiveMaintenanceId), trackChanges)
             .Include(p => p.Schedule)
            .Include(p => p.MeterSchedule)
            .Include(p => p.PreventiveMaintenanceHistories)
            .SingleOrDefaultAsync();
       

        public async Task<IEnumerable<PreventiveMaintenance>> GetPreventiveMaintenancesByIdsAsync(List<Guid?> preventiveMaintenanceIds, bool trackChanges) =>
            await FindByCondition(w => preventiveMaintenanceIds.Contains(w.Id), trackChanges)
            .Include(p => p.Schedule)
            .Include(p => p.MeterSchedule)
            .Include(p => p.PreventiveMaintenanceHistories)
            .ToListAsync();
       
    }
}
