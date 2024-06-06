using Entities;
using Shared;


namespace Service.Contracts
{
    public interface IMeterReadingService
    {
        Task<ApiBaseResponse> GetAllMeterReadingsAsync(Guid meterId,bool trackChanges);
        Task<ApiBaseResponse> GetMeterReadingAsync(Guid meterReadingId, bool trackChanges);
        Task<ApiBaseResponse> CreateMeterReading(MeterReadingDtoForCreation meterReading);
        Task<ApiBaseResponse> DeleteMeterReading(Guid meterReadingId, bool trackChanges);
        Task<ApiBaseResponse> DeleteUpdateMeterReading(Guid meterReadingId, bool trackChanges);
        Task<ApiBaseResponse> UpdateMeterReading(Guid meterReadingId, MeterReadingDtoForCreation meterReading, bool trackChanges);
    }
}
