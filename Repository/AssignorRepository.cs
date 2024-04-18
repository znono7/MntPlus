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
    public class AssignorRepository : RepositoryBase<Assignee> , IAssignorRepository
    {
        public AssignorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateAssignor(Assignee assignor) => Create(assignor);
      

        public void DeleteAssignor(Assignee assignor) => Delete(assignor);
       

        public async Task<IEnumerable<Assignee>?> GetAllAssignorsAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();


        public async Task<Assignee?> GetAssignorAsync(Guid assignorId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(assignorId), trackChanges)
            .SingleOrDefaultAsync();
        
    }
   
}
