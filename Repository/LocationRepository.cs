using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LocationRepository : RepositoryBase<Location> , ILocationRepository
    {
        public LocationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateLocation(Location location) => Create(location);
       

        public void DeleteLocation(Location location) => Delete(location);
       

        public async Task<IEnumerable<Location>?> GetAllLocationsAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Location?> GetLocationAsync(Guid locationId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(locationId), trackChanges)
            .SingleOrDefaultAsync();
        
    }
   
}
