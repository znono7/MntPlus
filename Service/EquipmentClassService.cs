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
    public class EquipmentClassService : IEquipmentClassService
    {
        private readonly IRepositoryManager _repository;

        public EquipmentClassService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiBaseResponse> CreateEquipmentClass(EquipmentClassForCreationDto equipmentClass)
        {
            try
            {
                var equipmentClassEntity = new EquipmentClass
                {
                    EquipmentClassName = equipmentClass.ClassName,
                };
                _repository.EquipmentClass.CreateEquipmentClass(equipmentClassEntity);
                await _repository.SaveAsync();
                var equipmentClassToReturn = new EquipmentClassDto
                (
                                       Id: equipmentClassEntity.Id,
                                                          ClassName: equipmentClassEntity.EquipmentClassName
                                                                           
                                                                                            );
                return new ApiOkResponse<EquipmentClassDto>(equipmentClassToReturn);

            }
            catch (Exception ex)
            {

                return new AssignorCreateErrorResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> DeleteEquipmentClass(Guid equipmentClassId, bool trackChanges)
        {
            try
            {
                var equipmentClass = await _repository.EquipmentClass.GetEquipmentClassAsync(equipmentClassId, trackChanges);
                if (equipmentClass is null)
                {
                    return new AssignorNotFoundResponse("equipment class");
                }
                _repository.EquipmentClass.DeleteEquipmentClass(equipmentClass);
                await _repository.SaveAsync();
                return new ApiOkResponse<EquipmentClass>(equipmentClass);

            }
            catch (Exception ex)
            {
                return new AssignorDeleteErrorResponse(ex.Message);

                
            }
        }

        public async Task<ApiBaseResponse> GetAllEquipmentClassesAsync(bool trackChanges)
        {
            try
            {
                var equipmentClasses = await _repository.EquipmentClass.GetAllEquipmentClassesAsync(trackChanges);
                if(equipmentClasses is null)
                {
                    return new AssignorListNotFoundResponse();
                }
                var equipmentClassesDto = equipmentClasses.Select(e => new EquipmentClassDto
                               (
                                       Id: e.Id,
                                                          ClassName: e.EquipmentClassName
                                                                         )).ToList();
                return new ApiOkResponse<IEnumerable<EquipmentClassDto>>(equipmentClassesDto);
               
            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetEquipmentClassAsync(Guid equipmentClassId, bool trackChanges)
        {
            try
            {
                var equipmentClass = await _repository.EquipmentClass.GetEquipmentClassAsync(equipmentClassId, trackChanges);
                if (equipmentClass is null)
                {
                    return new AssignorNotFoundResponse("equipment class");
                }
                var equipmentClassDto = new EquipmentClassDto
                (
                                       Id: equipmentClass.Id,
                                                          ClassName: equipmentClass.EquipmentClassName
                                                                         );
                return new ApiOkResponse<EquipmentClassDto>(equipmentClassDto);

            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);


            }
        }
    }
}
