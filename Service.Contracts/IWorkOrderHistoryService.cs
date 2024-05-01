using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IWorkOrderHistoryService
    {
        Task<ApiBaseResponse> GetAllWorkOrderHistoriesAsync(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> GetWorkOrderHistoryAsync(Guid id, bool trackChanges);
        Task<ApiBaseResponse> CreateWorkOrderHistory(WorkOrderHistoryCreateDto workOrderHistory);
        Task<ApiBaseResponse> DeleteWorkOrderHistory(Guid workOrderHistoryId, bool trackChanges);
        Task<ApiBaseResponse> UpdateWorkOrderHistory(Guid workOrderHistoryId, WorkOrderHistoryCreateDto workOrderHistory, bool trackChanges);
       
    }
}
