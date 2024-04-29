using Entities;
using Shared;

namespace Service.Contracts
{
    public interface ILocationService
    {
        Task<ApiBaseResponse> GetAllLocationsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetLocationAsync(Guid locationId, bool trackChanges);
        Task<ApiBaseResponse> CreateLocation(LocationForCreationDto location);
        Task<ApiBaseResponse> DeleteLocation(Guid locationId, bool trackChanges);
        Task<ApiBaseResponse> UpdateLocation(Guid locationId, LocationForCreationDto location, bool trackChanges);
    }
}
