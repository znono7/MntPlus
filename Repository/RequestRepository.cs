using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void BulkDeleteRequest(IEnumerable<Request> requests) => DeleteBulk(requests);
       

        public void CreateRequest(Request request) => Create(request);
       

        public void DeleteRequest(Request request) => Delete(request);
       

        public async Task<IEnumerable<Request>?> GetAllRequestsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
             .Include(w => w.UserAssignedTo)
            .Include(w => w.TeamAssignedTo)
            .Include(w => w.Asset)
            .ThenInclude(a => a!.Location)
            .Include(w => w.UserCreatedBy)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<int> GetNextRequestNumberAsync()
        {
            var maxNumber = await FindByCondition(r => true, false)
                            .MaxAsync(r => (int?)r.Number) ?? 0; 
            return maxNumber + 1;
        }

        public async Task<Request?> GetRequestAsync(Guid requestId, bool trackChanges) =>
            await FindByCondition(r => r.Id.Equals(requestId), trackChanges)
            .Include(w => w.UserAssignedTo)
            .Include(w => w.TeamAssignedTo)
            .Include(w => w.Asset)
            .ThenInclude(a => a!.Location)
            .Include(w => w.UserCreatedBy)
            .Include(w => w.Asset)
            .SingleOrDefaultAsync();

        public async Task<Request?> GetRequestForDeleteAsync(Guid requestId, bool trackChanges) =>
            await FindByCondition(r => r.Id.Equals(requestId), trackChanges)
            
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Request>> GetRequestsByIdsAsync(List<Guid> requestIds, bool trackChanges) =>
            await FindByCondition(r => requestIds.Contains(r.Id), trackChanges)
            
            .ToListAsync();
       
    }
   
}
