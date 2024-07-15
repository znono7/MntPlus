using Entities;


namespace Contracts
{
    public interface IMeterScheduleRepository
    {
        Task<MeterSchedule?> GetMeterScheduleAsync(Guid meterScheduleId, bool trackChanges);
        void CreateMeterSchedule(MeterSchedule meterSchedule);
        void DeleteMeterSchedule(MeterSchedule meterSchedule);
        void DeleteMeterSchedules(List<MeterSchedule> meterSchedules);
    }
}
