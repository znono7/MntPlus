using Entities;

namespace Contracts
{
    public interface IMeterRepository
    {
        Task<IEnumerable<Meter>?> GetAllMetersAsync(bool trackChanges);
        Task<IEnumerable<Meter>?> GetAllMerersByEquipment(Guid equipmentId, bool trackChanges);
       Task<IEnumerable<Meter>?> GetAllMetersWithReadingsAsync(bool trackChanges);
        Task<Meter?> GetMeterAsync(Guid meterId, bool trackChanges);
        Task<Meter?> GetMeterWithReadingAsync(Guid meterId, bool trackChanges);
        void CreateMeter(Meter meter);
        void DeleteMeter(Meter meter);
    }
}
