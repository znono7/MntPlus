using Entities;
using Shared;

namespace Service.Contracts
{
    public interface IPartService
    {
        Task<ApiBaseResponse> GetAllPartsAsync( bool trackChanges);
        Task<ApiBaseResponse> GetPartAsync(Guid partId, bool trackChanges);
        Task<ApiBaseResponse> CreatePart(PartForCreationDto part);
        Task<ApiBaseResponse> DeletePart(Guid partId, bool trackChanges);
        Task<ApiBaseResponse> UpdatePart(Guid partId, PartForCreationDto part, bool trackChanges);
    }
}
