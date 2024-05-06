using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IInstructionService
    {
        //Task<ApiBaseResponse> GetAllInstructionsAsync(bool trackChanges);
        //Task<ApiBaseResponse> GetInstructionAsync(Guid instructionId, bool trackChanges);
        Task<ApiBaseResponse> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges);
        Task<ApiBaseResponse> CreateInstruction(InstructionDtoForCreation instruction);
        Task<ApiBaseResponse> DeleteInstruction(Guid instructionId, bool trackChanges);
        //Task<ApiBaseResponse> UpdateInstruction(Guid instructionId, InstructionDtoForCreation instruction, bool trackChanges);
    }
}
