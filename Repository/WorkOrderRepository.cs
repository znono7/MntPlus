using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class WorkOrderRepository : RepositoryBase<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void BulkDeleteWorkOrder(IEnumerable<WorkOrder> workOrders) => DeleteBulk(workOrders);
       

        public void CreateWorkOrder(WorkOrder workOrder) => Create(workOrder);
       

        public void DeleteWorkOrder(WorkOrder workOrder) => Delete(workOrder);
      

        public async Task<IEnumerable<WorkOrder>?> GetAllWorkOrdersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(w => w.UserAssignedTo) 
            .Include(w => w.TeamAssignedTo)  
            .Include(w => w.Asset)
            .ThenInclude(a => a!.Location)
            .Include(w => w.UserCreatedBy)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<int> GetNextWorkOrderNumberAsync()
        {
            var maxNumber = await FindByCondition(w => true, false)
                            .MaxAsync(wo => (int?)wo.Number) ?? 0; 
            return maxNumber + 1;
        }

        public async Task<WorkOrder?> GetWorkOrderAsync(Guid workOrderId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(workOrderId), trackChanges)
              .Include(w => w.UserAssignedTo)
            .Include(w => w.TeamAssignedTo) 
            .Include(w => w.Asset)
            .ThenInclude(a => a!.Location)
            .Include(w => w.UserCreatedBy)
            .Include(w => w.Asset)
            .SingleOrDefaultAsync();

        public async Task<WorkOrder?> GetWorkOrderForDeleteAsync(Guid workOrderId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(workOrderId), trackChanges)
            .Include(w => w.WorkOrderHistories)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersByIdsAsync(List<Guid> workOrderIds, bool trackChanges) =>
            await FindByCondition(w => workOrderIds.Contains(w.Id), trackChanges)
            .Include(w => w.WorkOrderHistories)
            .ToListAsync();
       
    }
}
