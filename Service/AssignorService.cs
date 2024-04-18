using Contracts;
using Entities;
using Entities.Responses.Equipment;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AssignorService : IAssignorService
    {
        private readonly IRepositoryManager _repository;


        public AssignorService(IRepositoryManager repository)
        {
            _repository = repository;

        }
        public async Task<ApiBaseResponse> CreateAssignor(AssignedToForCreationDto assignor)
        {
            try
            {
                var assignorEntity = new Assignee
                {
                    Name = assignor.AssignedToName,
                };
                _repository.Assignor.CreateAssignor(assignorEntity);
                await _repository.SaveAsync();
                var assignorToReturn = new EquipmentAssignedToDto
                (
                    Id : assignorEntity.Id,
                    AssignedToName : assignorEntity.Name
                );
                return new ApiOkResponse<EquipmentAssignedToDto>(assignorToReturn);
            }
            catch (Exception ex)
            {

                return new AssignorCreateErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteAssignor(Guid assignorId, bool trackChanges)
        {
            try
            {
                var assignor = await _repository.Assignor.GetAssignorAsync(assignorId, trackChanges);
                if (assignor is null)
                {
                    return new AssignorNotFoundResponse("assignor");
                }
                _repository.Assignor.DeleteAssignor(assignor);
                await _repository.SaveAsync();
                return new ApiOkResponse<Assignee>(assignor);
            }
            catch (Exception)
            {
                return new AssignorDeleteErrorResponse("");


            }
        }

        public async Task<ApiBaseResponse> GetAllAssignorsAsync(bool trackChanges)
        {
            try
            {
                var assignors = await _repository.Assignor.GetAllAssignorsAsync(trackChanges);
                if (assignors is null)
                {

                    return new AssignorListNotFoundResponse();
                }
                var assignorsDto = assignors.Select(item => new EquipmentAssignedToDto(item.Id,item.Name)).ToList();

                return new ApiOkResponse<IEnumerable<EquipmentAssignedToDto>>(assignorsDto);
            }
            catch (Exception ex)
            {

                return new AssignorGetListErrorResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAssignorAsync(Guid assignorId, bool trackChanges)
        {
            try
            {
                var assignor = await _repository.Assignor.GetAssignorAsync(assignorId, trackChanges);
                if (assignor is null)
                {
                    return new AssignorNotFoundResponse("assignor");
                }
                var assignorDto = new EquipmentAssignedToDto(assignor.Id, assignor.Name);
                return new ApiOkResponse<EquipmentAssignedToDto>(assignorDto);

            }
            catch (Exception ex)
            {

                return new AssignorGetListErrorResponse(ex.Message);

            }
        }
    }
}
