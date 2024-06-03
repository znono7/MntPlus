using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Entities.Inventory>?> GetAllInventoriesAsync(bool trackChanges);
        Task<Entities.Inventory?> GetInventoryAsync(Guid inventoryId, bool trackChanges);
        void CreateInventory(Entities.Inventory inventory);
        void DeleteInventory(Entities.Inventory inventory);
    }
}
