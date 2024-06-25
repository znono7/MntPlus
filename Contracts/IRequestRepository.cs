using Entities;


namespace Contracts
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>?> GetAllRequestsAsync(bool trackChanges);
        Task<Request?> GetRequestAsync(Guid requestId, bool trackChanges);
        Task<Request?> GetRequestForDeleteAsync(Guid requestId, bool trackChanges);
        void CreateRequest(Request request);
        void DeleteRequest(Request request);
        Task<int> GetNextRequestNumberAsync();
        void BulkDeleteRequest(IEnumerable<Request> requests);
        Task<IEnumerable<Request>> GetRequestsByIdsAsync(List<Guid> requestIds, bool trackChanges);
    }
}
