using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class InstructionService : IInstructionService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public InstructionService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> CreateInstruction(InstructionDtoForCreation instruction)
        {
            try
            { 
                var instructionEntity = _mapper.Map<Instruction>(instruction);
                _repository.Instruction.CreateInstruction(instructionEntity);
                await _repository.SaveAsync();
                var instructionToReturn = _mapper.Map<InstructionDto>(instructionEntity);
                return new ApiOkResponse<InstructionDto>(instructionToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteInstruction(Guid instructionId, bool trackChanges)
        {
            try
            {
                var instruction = await _repository.Instruction.GetInstructionAsync(instructionId, trackChanges);
                if (instruction is null)
                {
                    return new ApiNotFoundResponse(""); 
                }
                _repository.Instruction.DeleteInstruction(instruction);
                await _repository.SaveAsync();
                var instructionToReturn = _mapper.Map<InstructionDto>(instruction);

                return new ApiOkResponse<InstructionDto>(instructionToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetInstructionsByWorkOrderAsync(Guid workOrderId, bool trackChanges)
        {
            try
            {
                var instructions = await _repository.Instruction.GetInstructionsByWorkOrderAsync(workOrderId, trackChanges);
                if (instructions is null)
                {
                    return new ApiNotFoundResponse(""); 
                }
                var instructionsToReturn = _mapper.Map<IEnumerable<InstructionDto>>(instructions);
                return new ApiOkResponse<IEnumerable<InstructionDto>>(instructionsToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
