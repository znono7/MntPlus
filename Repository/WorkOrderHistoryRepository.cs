using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WorkOrderHistoryRepository : RepositoryBase<WorkOrderHistory>, IWorkOrderHistoryRepository
    {
        public WorkOrderHistoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateWorkOrderHistory(WorkOrderHistory workOrderHistory) => Create(workOrderHistory);
       

        public void DeleteWorkOrderHistory(WorkOrderHistory workOrderHistory) => Delete(workOrderHistory);
       

        public async Task<IEnumerable<WorkOrderHistory>?> GetWorkOrderHistoriesAsync(Guid workOrderId, bool trackChanges) =>
            await FindByCondition(c => c.WorkOrderId.Equals(workOrderId), trackChanges)
            .Include(c => c.ChangedBy)
            .OrderByDescending(c => c.DateChanged)
            .ToListAsync();
       

        public async Task<WorkOrderHistory?> GetWorkOrderHistoryAsync( Guid id, bool trackChanges) =>
            await FindByCondition(c =>  c.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        
        
    }
}
