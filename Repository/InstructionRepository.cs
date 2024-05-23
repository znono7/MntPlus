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
    public class InstructionRepository : RepositoryBase<Entities.WorkTask>, IInstructionRepository
    {
        public InstructionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateInstruction(Entities.WorkTask instruction) => Create(instruction);
       

        public void DeleteInstruction(Entities.WorkTask instruction) => Delete(instruction);

        public async Task<WorkTask?> GetInstructionAsync(Guid instructionId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(instructionId), trackChanges)
                .SingleOrDefaultAsync();
       

        public async Task<IEnumerable<WorkTask>?> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges) =>
           await FindByCondition(c => c.PreventiveID.Equals(workOrderId), trackChanges)
                 .OrderBy(c => c.Description)
                 .ToListAsync();

      
    }
}
