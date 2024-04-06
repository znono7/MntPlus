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

        public IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllEquipments)} service method {ex}");

                throw;
            }
        }
    }
}
