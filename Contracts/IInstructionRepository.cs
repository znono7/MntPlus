using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IInstructionRepository
    {
        //Task<IEnumerable<Instruction>?> GetAllInstructionsAsync(bool trackChanges);
        Task<WorkTask?> GetInstructionAsync(Guid instructionId, bool trackChanges);
        Task<IEnumerable<WorkTask>?> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges);
        void CreateInstruction(Entities.WorkTask instruction);
        void DeleteInstruction(Entities.WorkTask instruction);
    }
}
