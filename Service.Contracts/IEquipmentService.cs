using Entities;

namespace Service.Contracts
{
    public interface IEquipmentService
    {
        IEnumerable<Equipment> GetAllEquipments(bool trackChanges);

    }
}
