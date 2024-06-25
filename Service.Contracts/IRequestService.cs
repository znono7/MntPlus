using Entities;
using Shared;

namespace Service.Contracts
{
    public interface IRequestService
    {
        Task<ApiBaseResponse> GetAllRequestsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetRequestAsync(Guid requestId, bool trackChanges);
        Task<ApiBaseResponse> CreateRequest(RequestForCreationDto request); 
        Task<ApiBaseResponse> CreateLastNumberRequest();
        Task<ApiBaseResponse> DeleteRequest(Guid requestId, bool trackChanges);
        Task<ApiBaseResponse> BulkDeleteRequest(List<Guid> requestIds, bool trackChanges);
        Task<ApiBaseResponse> UpdateRequest(Guid requestId, RequestForCreationDto request, bool trackChanges);
        Task<ApiBaseResponse> UpdateStatRequest(Guid requestId, string requestStat, bool trackChanges);
    }
}
