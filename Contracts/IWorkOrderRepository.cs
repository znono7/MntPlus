using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWorkOrderRepository
    {
        Task<IEnumerable<WorkOrder>?> GetAllWorkOrdersAsync(bool trackChanges);
        Task<WorkOrder?> GetWorkOrderAsync(Guid workOrderId, bool trackChanges);
        Task<WorkOrder?> GetWorkOrderForDeleteAsync(Guid workOrderId, bool trackChanges);
        void CreateWorkOrder(WorkOrder workOrder);
        void DeleteWorkOrder(WorkOrder workOrder);
        Task<int> GetNextWorkOrderNumberAsync();
        void BulkDeleteWorkOrder(IEnumerable<WorkOrder> workOrders);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersByIdsAsync(List<Guid> workOrderIds, bool trackChanges);
    }
}
