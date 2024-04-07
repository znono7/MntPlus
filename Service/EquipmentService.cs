using Contracts;
using Entities;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EquipmentService : IEquipmentService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public EquipmentService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public EquipmentDto CreateEquipment(EquipmentForCreationDto equipment)
        {
            var equipmentEntity = new Equipment
            {
                EquipmentParent = equipment.EquipmentParent,
                EquipmentName = equipment.EquipmentName,
                EquipmentCategory = equipment.EquipmentCategory,
                EquipmentModel = equipment.EquipmentModel,
                EquipmentMake = equipment.EquipmentMake,
                EquipmentNameImage = equipment.EquipmentNameImage,
                EquipmentImage = equipment.EquipmentImage
            };
            _repository.Equipment.CreateEquipment(equipmentEntity);
            _repository.Save();
            var equipmentToReturn = new EquipmentDto
                (equipmentEntity.Id,
                 equipmentEntity.EquipmentParent,
                 equipmentEntity.EquipmentName,
                 equipmentEntity.EquipmentCategory,
                 equipmentEntity.EquipmentModel,
                 equipmentEntity.EquipmentMake,
                 equipmentEntity.EquipmentNameImage,
                 equipmentEntity.EquipmentImage
                 );
            return equipmentToReturn;
            
        }

        public IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges)
        {
            
            
                var equipments = _repository.Equipment.GetAllEquipments(trackChanges);
                var equipmentsDto = equipments.Select(equipment => new EquipmentDto
                               (equipment.Id,
                                equipment.EquipmentParent,
                                equipment.EquipmentName,
                                equipment.EquipmentCategory,
                                equipment.EquipmentModel,
                                equipment.EquipmentMake,
                                equipment.EquipmentNameImage,
                                equipment.EquipmentImage
                                )).ToList();
                return equipmentsDto;
            
           
        }

        public EquipmentDto GetEquipment(Guid equipmentId, bool trackChanges)
        {
            var equipment = _repository.Equipment.GetEquipment(equipmentId, trackChanges);
            var equipmentDto = new EquipmentDto
                (equipment.Id,
                 equipment.EquipmentParent,
                 equipment.EquipmentName,
                 equipment.EquipmentCategory,
                 equipment.EquipmentModel,
                 equipment.EquipmentMake,
                 equipment.EquipmentNameImage,
                 equipment.EquipmentImage
                 );
            return equipmentDto;
            
        }
    }
}
