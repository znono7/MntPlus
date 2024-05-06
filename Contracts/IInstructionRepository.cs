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
        Task<Instruction?> GetInstructionAsync(Guid instructionId, bool trackChanges);
        Task<IEnumerable<Instruction>?> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges);
        void CreateInstruction(Instruction instruction);
        void DeleteInstruction(Instruction instruction);
    }
}
