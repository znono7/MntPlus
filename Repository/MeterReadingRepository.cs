using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class MeterReadingRepository : RepositoryBase<MeterReading>, IMeterReadingRepository
    {
        public MeterReadingRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateMeterReading(MeterReading meterReading) => Create(meterReading);

        public void DeleteMeterReading(MeterReading meterReading) => Delete(meterReading);

       

        public async Task<IEnumerable<MeterReading>?> GetAllMeterReadingsAsync(Guid meterId, bool trackChanges) =>
           await FindByCondition(c => c.MeterId.Equals(meterId), trackChanges)
            .Include(c => c.User) 
            .OrderByDescending(c => c.Timestamp) 
            .ToListAsync();
       

        public async Task<MeterReading?> GetMeterReadingAsync(Guid meterReadingId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(meterReadingId), trackChanges)
            .SingleOrDefaultAsync(); 
    }
    
    
}
