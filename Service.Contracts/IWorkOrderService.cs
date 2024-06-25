using Entities;
using Shared;

namespace Service.Contracts
{
    public interface IWorkOrderService
    {
        Task<ApiBaseResponse> GetAllWorkOrdersAsync(bool trackChanges);
        Task<ApiBaseResponse> GetWorkOrderAsync(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> CreateWorkOrder(WorkOrderForCreationDto workOrder); 
        Task<ApiBaseResponse> CreateLastNumberWorkOrder();
        Task<ApiBaseResponse> DeleteWorkOrder(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> BulkDeleteWorkOrder(List<Guid> workOrderIds, bool trackChanges);
        Task<ApiBaseResponse> UpdateWorkOrder(Guid workOrderId, WorkOrderForCreationDto workOrder, bool trackChanges);
        Task<ApiBaseResponse> UpdateStatWorkOrder(Guid workOrderId, string workOrderStat, bool trackChanges);
    }
}
