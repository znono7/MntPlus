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
    public class InstructionRepository : RepositoryBase<Instruction>, IInstructionRepository
    {
        public InstructionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateInstruction(Instruction instruction) => Create(instruction);
       

        public void DeleteInstruction(Instruction instruction) => Delete(instruction);

        public async Task<Instruction?> GetInstructionAsync(Guid instructionId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(instructionId), trackChanges)
                .SingleOrDefaultAsync();
       

        public async Task<IEnumerable<Instruction>?> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges) =>
           await FindByCondition(c => c.WorkOrderID.Equals(workOrderId), trackChanges)
                 .OrderBy(c => c.Description)
                 .ToListAsync();
        
    }
}
