using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class MeterScheduleRepository : RepositoryBase<MeterSchedule>, IMeterScheduleRepository
    {
        public MeterScheduleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateMeterSchedule(MeterSchedule meterSchedule) => Create(meterSchedule);

        public void DeleteMeterSchedule(MeterSchedule meterSchedule) => Delete(meterSchedule);

        public void DeleteMeterSchedules(List<MeterSchedule> meterSchedules) => DeleteBulk(meterSchedules);
       

        public async Task<IEnumerable<MeterSchedule>?> GetAllMeterSchedulesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();

        public async Task<MeterSchedule?> GetMeterScheduleAsync(Guid meterScheduleId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(meterScheduleId), trackChanges)
            .Include(c => c.Meter)
            .SingleOrDefaultAsync();
    }
}
