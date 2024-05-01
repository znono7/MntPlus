using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWorkOrderHistoryRepository
    {
        Task<IEnumerable<WorkOrderHistory>?> GetWorkOrderHistoriesAsync(Guid workOrderId, bool trackChanges);
        Task<WorkOrderHistory?> GetWorkOrderHistoryAsync(Guid id, bool trackChanges);
        void CreateWorkOrderHistory(WorkOrderHistory workOrderHistory);
        void DeleteWorkOrderHistory(WorkOrderHistory workOrderHistory);
    }
}
