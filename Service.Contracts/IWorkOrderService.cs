using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IWorkOrderService
    {
        Task<ApiBaseResponse> GetAllWorkOrdersAsync(bool trackChanges);
        Task<ApiBaseResponse> GetWorkOrderAsync(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> CreateWorkOrder(WorkOrderForCreationDto workOrder); 
        Task<ApiBaseResponse> CreateLastNumberWorkOrder();
        Task<ApiBaseResponse> DeleteWorkOrder(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> UpdateWorkOrder(Guid workOrderId, WorkOrderForCreationDto workOrder, bool trackChanges);
    }
}
