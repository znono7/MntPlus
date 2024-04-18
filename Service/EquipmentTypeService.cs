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
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly IRepositoryManager _repository;

        public EquipmentTypeService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiBaseResponse> CreateEquipmentType(EquipmentTypeDtoCreate equipmentType)
        {
            try
            {
                var equipmentTypeEntity = new EquipmentType
                {
                    EquipmentTypeName = equipmentType.EquipmentTypeName,
                };
                _repository.EquipmentType.CreateEquipmentType(equipmentTypeEntity);
                await _repository.SaveAsync();
                var equipmentTypeToReturn = new EquipmentTypeDto
                (
                                       Id: equipmentTypeEntity.Id,
                                                          EquipmentTypeName: equipmentTypeEntity.EquipmentTypeName
                                                                                            );
                return new ApiOkResponse<EquipmentTypeDto>(equipmentTypeToReturn);
            }
            catch (Exception ex)
            {
                return new AssignorCreateErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteEquipmentType(Guid typeId, bool trackChanges)
        {
            try
            {
                var equipmentType = await _repository.EquipmentType.GetEquipmentTypeAsync(typeId, trackChanges);
                if (equipmentType is null)
                {
                    return new AssignorNotFoundResponse("equipment type");
                }
                _repository.EquipmentType.DeleteEquipmentType(equipmentType);
                await _repository.SaveAsync();
                return new ApiOkResponse<EquipmentType>(equipmentType);
            }
            catch (Exception ex)
            {
                return new AssignorCreateErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllEquipmentTypesAsync(bool trackChanges)
        {
            try
            {
                var equipmentTypes = await _repository.EquipmentType.GetAllEquipmentTypesAsync(trackChanges);
                if(equipmentTypes is null)
                {
                    return new AssignorListNotFoundResponse();
                }
                var equipmentTypesDto = equipmentTypes.Select(equipmentType => new EquipmentTypeDto
                               (
                                       Id: equipmentType.Id,
                                                          EquipmentTypeName: equipmentType.EquipmentTypeName
                                                                         )).ToList();
                return new ApiOkResponse<IEnumerable<EquipmentTypeDto>>(equipmentTypesDto);
            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetEquipmentTypeAsync(Guid equipmentTypeId, bool trackChanges)
        {
            try
            {
                var equipmentType = await _repository.EquipmentType.GetEquipmentTypeAsync(equipmentTypeId, trackChanges);
                if (equipmentType is null)
                {
                    return new AssignorNotFoundResponse("equipment type");
                }
                var equipmentTypeDto = new EquipmentTypeDto
                               (
                                                          Id: equipmentType.Id,
                                                                                                                   EquipmentTypeName: equipmentType.EquipmentTypeName
                                                                                                                                                                                           );
                return new ApiOkResponse<EquipmentTypeDto>(equipmentTypeDto);
            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);
            }
            
        }
    }
}
