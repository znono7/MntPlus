using Contracts;
using Entities;
using Entities.Responses.Equipment;
using Service.Contracts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EquipmentStatusService : IEquipmentStatusService
    {
        private readonly IRepositoryManager _repositoryManager;

        public EquipmentStatusService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ApiBaseResponse> CreateEquipmentStatus(EquipmentStatusDtoCreate equipmentStatus)
        {
            try
            {
                var equipmentStatusEntity = new EquipmentStatus
                {
                    EquipmentStatusName = equipmentStatus.StatusName,
                };
                _repositoryManager.EquipmentStatus.CreateEquipmentStatus(equipmentStatusEntity);
                await _repositoryManager.SaveAsync();
                var equipmentStatusToReturn = new EquipmentStatusDto
                (
                    Id : equipmentStatusEntity.Id,
                    StatusName: equipmentStatusEntity.EquipmentStatusName
                );
                return new ApiOkResponse<EquipmentStatusDto>(equipmentStatusToReturn);
            }
            catch (Exception ex)
            {
                return new AssignorCreateErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteEquipmentStatus(Guid equipmentStatusId, bool trackChanges)
        {
            try
            {
                var equipmentStatus = await _repositoryManager.EquipmentStatus.GetEquipmentStatusByIdAsync(equipmentStatusId, trackChanges);
                if (equipmentStatus is null)
                {
                    return new AssignorNotFoundResponse("equipment status");
                }
                _repositoryManager.EquipmentStatus.DeleteEquipmentStatus(equipmentStatus);
                await _repositoryManager.SaveAsync();
                return new ApiOkResponse<EquipmentStatus>(equipmentStatus);
            }
            catch (Exception ex)
            {
                return new AssignorDeleteErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllEquipmentStatusesAsync(bool trackChanges)
        {
            try
            {
                var equipmentStatuses = await _repositoryManager.EquipmentStatus.GetAllEquipmentStatusAsync(trackChanges);
                if (equipmentStatuses is null)
                {

                    return new AssignorListNotFoundResponse();
                }
                var equipmentStatusesDto = equipmentStatuses.Select(e => new EquipmentStatusDto
                               (
                                       Id : e.Id,
                                                          StatusName : e.EquipmentStatusName
                                                                         ));
                return new ApiOkResponse<IEnumerable<EquipmentStatusDto>>(equipmentStatusesDto);
            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);
            }
           
        }

        public async Task<ApiBaseResponse> GetEquipmentStatusAsync(Guid equipmentStatusId, bool trackChanges)
        {
            try
            {
                var equipmentStatus = await _repositoryManager.EquipmentStatus.GetEquipmentStatusByIdAsync(equipmentStatusId, trackChanges);
                if (equipmentStatus is null)
                {
                    return new AssignorNotFoundResponse("equipment status");
                }
                var equipmentStatusDto = new EquipmentStatusDto
                (
                                       Id : equipmentStatus.Id,
                                                          StatusName : equipmentStatus.EquipmentStatusName
                                                                         );
                return new ApiOkResponse<EquipmentStatusDto>(equipmentStatusDto);
            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);
            }
            
        }
    }
}
