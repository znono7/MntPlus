using Entities;

namespace Contracts
{
    public interface IMeterRepository
    {
        Task<IEnumerable<Meter>?> GetAllMetersAsync(bool trackChanges);
        Task<Meter?> GetMeterAsync(Guid meterId, bool trackChanges);
        void CreateMeter(Meter meter);
        void DeleteMeter(Meter meter);
    }
}
