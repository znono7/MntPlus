using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IInventoryService
    {
        Task<ApiBaseResponse> GetAllInventoriesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetInventoryAsync(Guid inventoryId, bool track);
        Task<ApiBaseResponse> CreateInventory(InventoryForCreationDto inventory);
        Task<ApiBaseResponse> DeleteInventory(Guid inventoryId, bool trackChanges);
        Task<ApiBaseResponse> UpdateInventory(Guid inventoryId, InventoryForCreationDto inventory, bool trackChanges);

    }
}
