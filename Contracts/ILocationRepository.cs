using Entities;


namespace Contracts
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>?> GetAllLocationsAsync(bool trackChanges);
        Task<Location?> GetLocationAsync(Guid locationId, bool trackChanges);
        void CreateLocation(Location location);
        void DeleteLocation(Location location);
    }
}
