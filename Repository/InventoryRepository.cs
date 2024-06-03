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
    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        public InventoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateInventory(Inventory inventory) => Create(inventory);

        public void DeleteInventory(Inventory inventory) => Delete(inventory);

        public async Task<IEnumerable<Inventory>?> GetAllInventoriesAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.DateReceived)
            .ToListAsync();

        public async Task<Inventory?> GetInventoryAsync(Guid inventoryId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(inventoryId), trackChanges)
            .SingleOrDefaultAsync();

    }
}
