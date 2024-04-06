using Entities;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEquipmentService
    {
        IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges);

    }
}
