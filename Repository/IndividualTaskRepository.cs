using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{ 
    public class IndividualTaskRepository : RepositoryBase<IndividualTask>, IIndividualTaskRepository
    {
        public IndividualTaskRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateIndividualTask(IndividualTask individualTask) => Create(individualTask);

        public void DeleteIndividualTask(IndividualTask individualTask) => Delete(individualTask);

        public async Task<IEnumerable<IndividualTask>?> GetAllIndividualTasks(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();
       

      

        public async Task<IndividualTask?> GetIndividualTaskById(Guid individualTaskId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(individualTaskId), trackChanges)
            .SingleOrDefaultAsync();

       
    }
    
    
}
