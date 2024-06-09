using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class MeterRepository : RepositoryBase<Meter>, IMeterRepository
    {
        public MeterRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateMeter(Meter meter) => Create(meter);

        public void DeleteMeter(Meter meter) => Delete(meter);

        public async Task<IEnumerable<Meter>?> GetAllMerersByEquipment(Guid equipmentId, bool trackChanges) =>
            await FindByCondition(c => c.AssetId.Equals(equipmentId), trackChanges)
            .Include( c =>  c.MeterReadings!)
            .ThenInclude(c => c.User)
            .Include(c => c.Asset)
            .OrderBy(c => c.Name)
            .ToListAsync();
       

        public async Task<IEnumerable<Meter>?> GetAllMetersAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .Include(c => c.Asset)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<IEnumerable<Meter>?> GetAllMetersWithReadingsAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .Include(c => c.MeterReadings!)
            .ThenInclude(c => c.User)
            .Include(c => c.Asset)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Meter?> GetMeterAsync(Guid meterId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(meterId), trackChanges)
            .Include(c => c.Asset)
            .SingleOrDefaultAsync(); 

        public async Task<Meter?> GetMeterWithReadingAsync(Guid meterId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(meterId), trackChanges)
            .Include(c => c.MeterReadings!)
            .ThenInclude(c => c.User)
            .Include(c => c.Asset)
            .SingleOrDefaultAsync();
    }
    
    
}
