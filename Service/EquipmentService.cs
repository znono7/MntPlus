using Contracts;
using Entities;
using Entities.Responses.Equipment;
using Service.Contracts;
using Shared;

namespace Service
{
    internal sealed class EquipmentService : IEquipmentService
    {
        private readonly IRepositoryManager _repository;
       

        public EquipmentService(IRepositoryManager repository)
        {
            _repository = repository;
            
        }

        public async Task<ApiBaseResponse> CreateEquipmentAsync(EquipmentForCreationDto equipment)
        {
            var equipmentEntity = MapToEquipmentEntity(equipment);
            _repository.Equipment.CreateEquipment(equipmentEntity);
            await _repository.SaveAsync();
            var equipmentToReturn = MapToEquipmentDTO(equipmentEntity);
            return new ApiOkResponse<EquipmentDto>(equipmentToReturn);  
        }

        public async Task<ApiBaseResponse> GetAllEquipmentsAsync(bool trackChanges)
        {
            var equipments = await _repository.Equipment.GetAllEquipmentsAsync(trackChanges);
            if (equipments is null)
            {

                return new EquipmentListNotFoundResponse();
            }
            var equipmentsDto = equipments.Select(MapToEquipmentDTO).ToList();
           
            return new ApiOkResponse <IEnumerable<EquipmentDto>>(equipmentsDto);
        }

        //public async Task<EquipmentDto?> CreateEquipmentAsync(EquipmentForCreationDto equipment)
        //{
        //    var equipmentEntity = MapToEquipmentEntity(equipment);
        //     _repository.Equipment.CreateEquipment(equipmentEntity);
        //    await _repository.SaveAsync();
        //    var equipmentToReturn = MapToEquipmentDTO(equipmentEntity);
        //    return equipmentToReturn;
            
        //}

        //public async Task< IEnumerable<EquipmentDto?>?> GetAllEquipmentsAsync(bool trackChanges)
        //{
            
            
        //        var equipments = await _repository.Equipment.GetAllEquipmentsAsync(trackChanges);
        //        var equipmentsDto = equipments.Select(MapToEquipmentDTO).ToList();
        //        return equipmentsDto;
            
           
        //}

        public async Task<EquipmentDto?> GetEquipmentAsync(Guid equipmentId, bool trackChanges)
        {
            var equipment = await _repository.Equipment.GetEquipmentAsync(equipmentId, trackChanges);
            var equipmentDto = MapToEquipmentDTO(equipment);
            return equipmentDto;
            
        }

        #region Mapping
        //from Equipment to EquipmentDto
        private EquipmentDto? MapToEquipmentDTO(Equipment? equipment)
        {
            if (equipment == null)
            {
                throw new ArgumentNullException(nameof(equipment));
                
            }
            return new EquipmentDto
            (
                Id : equipment.Id,
                EquipmentParent : equipment.EquipmentParent,
                EquipmentName : equipment.EquipmentName,
                EquipmentType : equipment.EquipmentType is null ? null : new EquipmentTypeDto( Id : equipment.EquipmentType.Id, EquipmentTypeName : equipment.EquipmentType.EquipmentTypeName ),
                EquipmentDescription : equipment.EquipmentDescription,
                EquipmentOrganization : equipment.EquipmentOrganization is null ? null : new EquipmentOrganizationDto ( Id : equipment.EquipmentOrganization.Id, OrganizationName : equipment.EquipmentOrganization.OrganizationName ),
                EquipmentDepartment : equipment.EquipmentDepartment is null ? null : new EquipmentDepartmentDto ( Id : equipment.EquipmentDepartment.Id, DepartmentName : equipment.EquipmentDepartment.DepartmentName ),
                EquipmentClass : equipment.EquipmentClass is null ? null : new EquipmentClassDto ( Id : equipment.EquipmentClass.Id, ClassName: equipment.EquipmentClass.EquipmentClassName ),
                EquipmentSite : equipment.EquipmentSite,
                EquipmentStatus : equipment.EquipmentStatus is null ? null : new EquipmentStatusDto ( Id : equipment.EquipmentStatus.Id, StatusName: equipment.EquipmentStatus.EquipmentStatusName ),
                EquipmentMake : equipment.EquipmentMake,
                EquipmentSerialNumber : equipment.EquipmentSerialNumber,
                EquipmentModel : equipment.EquipmentModel,
                EquipmentCost : equipment.EquipmentCost,
                EquipmentCommissionDate : equipment.EquipmentCommissionDate,
                EquipmentAssignedTo : equipment.EquipmentAssignedTo is null ? null : new EquipmentAssignedToDto ( Id : equipment.EquipmentAssignedTo.Id, AssignedToName: equipment.EquipmentAssignedTo.Name ),
                EquipmentNameImage : equipment.EquipmentNameImage,
                EquipmentImage : equipment.EquipmentImage
            );
        }
        //from EquipmentForCreationDto to Equipment
        private Equipment MapToEquipmentEntity(EquipmentForCreationDto equipment)
        {
            return new Equipment
            {
                EquipmentParent = equipment.EquipmentParent,
                EquipmentName = equipment.EquipmentName,
                EquipmentType = equipment.EquipmentType is null ? null : new EquipmentType { Id = equipment.EquipmentType.Id, EquipmentTypeName = equipment.EquipmentType.EquipmentTypeName } ,
                EquipmentDescription = equipment.EquipmentDescription,
                EquipmentOrganization = equipment.EquipmentOrganization is null ? null : new Organization { Id = equipment.EquipmentOrganization.Id, OrganizationName = equipment.EquipmentOrganization.OrganizationName},
                EquipmentDepartment = equipment.EquipmentDepartment is null ? null : new EquipmentDepartment { Id = equipment.EquipmentDepartment.Id, DepartmentName = equipment.EquipmentDepartment.DepartmentName},
                EquipmentClass = equipment.EquipmentClass is null ? null : new EquipmentClass { Id = equipment.EquipmentClass.Id, EquipmentClassName = equipment.EquipmentClass.ClassName},
                EquipmentSite = equipment.EquipmentSite,
                EquipmentStatus = equipment.EquipmentStatus is null ? null : new EquipmentStatus { Id = equipment.EquipmentStatus.Id, EquipmentStatusName = equipment.EquipmentStatus.StatusName},
                EquipmentMake = equipment.EquipmentMake,
                EquipmentSerialNumber = equipment.EquipmentSerialNumber,
                EquipmentModel = equipment.EquipmentModel,
                EquipmentCost = equipment.EquipmentCost,
                EquipmentCommissionDate = equipment.EquipmentCommissionDate,
                EquipmentAssignedTo = equipment.EquipmentAssignedTo is null ? null : new Assignee { Id = equipment.EquipmentAssignedTo.Id, Name = equipment.EquipmentAssignedTo.AssignedToName},
                EquipmentNameImage = equipment.EquipmentNameImage,
                EquipmentImage = equipment.EquipmentImage
            };
        }
        private EquipmentForCreationDto MapToEquipmentForCreationDTO(Equipment equipmentEntity)
        {
            return new EquipmentForCreationDto
            (
                
                EquipmentParent : equipmentEntity.EquipmentParent,
                EquipmentName : equipmentEntity.EquipmentName,
                EquipmentType : MapToTypeDTO(equipmentEntity.EquipmentType),
                EquipmentDescription : equipmentEntity.EquipmentDescription,
                EquipmentOrganization : MapToOrganizationDTO(equipmentEntity.EquipmentOrganization),
                EquipmentDepartment : MapToEquipmentDepartmentDTO(equipmentEntity.EquipmentDepartment),
                EquipmentClass : MapToEquipmentClassDTO(equipmentEntity.EquipmentClass),
                EquipmentSite : equipmentEntity.EquipmentSite,
                EquipmentStatus : MapToStatusDTO(equipmentEntity.EquipmentStatus),
                EquipmentMake : equipmentEntity.EquipmentMake,
                EquipmentSerialNumber : equipmentEntity.EquipmentSerialNumber,
                EquipmentModel : equipmentEntity.EquipmentModel,
                EquipmentCost : equipmentEntity.EquipmentCost,
                EquipmentCommissionDate : equipmentEntity.EquipmentCommissionDate,
                EquipmentAssignedTo : MapToAssigneeDTO(equipmentEntity.EquipmentAssignedTo),
                EquipmentNameImage : equipmentEntity.EquipmentNameImage,
                EquipmentImage : equipmentEntity.EquipmentImage
            );
        }

        private EquipmentStatusDto? MapToStatusDTO(EquipmentStatus? equipmentStatus)
        {
            if (equipmentStatus != null)
            {
                return new EquipmentStatusDto
                (
                                       Id: equipmentStatus.Id,
                                                          StatusName: equipmentStatus.EquipmentStatusName
                                                                         );
            }
            return null;
            
        }

        private EquipmentTypeDto? MapToTypeDTO(EquipmentType? equipmentType)
        {
            if (equipmentType != null)
            {
                return new EquipmentTypeDto
                (
                                       Id: equipmentType.Id,
                                                          EquipmentTypeName: equipmentType.EquipmentTypeName
                                                                         );
            }
            return null;
            
        }

        private EquipmentOrganizationDto? MapToOrganizationDTO(Organization? organizationEntity) 
        {
            if (organizationEntity != null)
            {
                return new EquipmentOrganizationDto
                (
                     Id: organizationEntity.Id,
                     OrganizationName: organizationEntity.OrganizationName
                );
            }
            return null;

        }
            

        private EquipmentDepartmentDto? MapToEquipmentDepartmentDTO(EquipmentDepartment? equipmentDepartmentEntity)
        {
            if(equipmentDepartmentEntity != null)
            {
                return new EquipmentDepartmentDto
                (
                                       Id: equipmentDepartmentEntity.Id,
                                                          DepartmentName: equipmentDepartmentEntity.DepartmentName
                                                                         );
            }
            return null;
        }

        private EquipmentClassDto? MapToEquipmentClassDTO(EquipmentClass? equipmentClassEntity)
        {
            if(equipmentClassEntity != null)
            {
                return new EquipmentClassDto
                (
                                       Id: equipmentClassEntity.Id,
                                                          ClassName: equipmentClassEntity.EquipmentClassName
                                                                         );
            }
            return null;
        }

        private EquipmentAssignedToDto? MapToAssigneeDTO(Assignee? assigneeEntity)
        {
            if(assigneeEntity != null)
            {
                return new EquipmentAssignedToDto
                (
                                     Id: assigneeEntity.Id,
                                             AssignedToName: assigneeEntity.Name
                                                                                                                                                                                           );
            }
            return null;
        }

      


        #endregion
    }
}
