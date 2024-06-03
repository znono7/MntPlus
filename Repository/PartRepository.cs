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
    public class PartRepository : RepositoryBase<Part>, IPartRepository
    {
        public PartRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreatePart(Part part) => Create(part);

        public void DeletePart(Part part) => Delete(part);

        public async Task<IEnumerable<Part>?> GetAllPartsAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .Include(c => c.Inventories)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Part?> GetPartAsync(Guid partId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(partId), trackChanges)
            .Include(c => c.Inventories)
            .SingleOrDefaultAsync();
    }
   
}
