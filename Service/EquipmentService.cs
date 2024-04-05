using Contracts;
using Entities;
using Service.Contracts;

namespace Service
{
    internal sealed class EquipmentService : IEquipmentService
    {
        private readonly IRepositoryManager _repository;
        public EquipmentService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public IEnumerable<Equipment> GetAllEquipments(bool trackChanges)
        {
            try
            {
                var equipments = _repository.Equipment.GetAllEquipments(trackChanges);
                return equipments;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
