using Entities;
using Shared;

namespace Service.Contracts
{
    public interface IMeterService
    {
        Task<ApiBaseResponse> GetAllMetersAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAllMetersWthReadingsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetMeterAsync(Guid meterId, bool trackChanges);
        Task<ApiBaseResponse> GetMeterWithReadingAsync(Guid meterId, bool trackChanges);
        Task<ApiBaseResponse> CreateMeter(MeterDtoForCreation meter);
        Task<ApiBaseResponse> DeleteMeter(Guid meterId, bool trackChanges);
        Task<ApiBaseResponse> UpdateMeter(Guid meterId, MeterDtoForCreation meter, bool trackChanges);
    }
}
