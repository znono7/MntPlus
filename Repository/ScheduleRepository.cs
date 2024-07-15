using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateSchedule(Schedule schedule) => Create(schedule);

        public void DeleteSchedule(Schedule schedule) => Delete(schedule);

        public void DeleteSchedules(IEnumerable<Schedule> schedules) => DeleteBulk(schedules);

        public async Task<IEnumerable<Schedule>?> GetAllSchedulesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();

        public async Task<Schedule?> GetScheduleAsync(Guid scheduleId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(scheduleId), trackChanges)
            .SingleOrDefaultAsync();

       
    }
   
}
