using Entities;

namespace Contracts
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>?> GetAllSchedulesAsync(bool trackChanges);
        Task<Schedule?> GetScheduleAsync(Guid scheduleId, bool trackChanges);
        void CreateSchedule(Schedule schedule);
        void DeleteSchedule(Schedule schedule);
        void DeleteSchedules(IEnumerable<Schedule> schedules);
    }
} 
