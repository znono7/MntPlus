using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public InventoryService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateInventory(InventoryForCreationDto inventory)
        {
            try
            {
                var inventoryEntity = _mapper.Map<Inventory>(inventory);
                _repository.Inventory.CreateInventory(inventoryEntity);
                await _repository.SaveAsync();
                var inventoryToReturn = _mapper.Map<InventoryDto>(inventoryEntity);
                return new ApiOkResponse<InventoryDto>(inventoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteInventory(Guid inventoryId, bool track)
        {
            try
            {
                var inventory = await _repository.Inventory.GetInventoryAsync(inventoryId, track);
                if (inventory is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Inventory.DeleteInventory(inventory);
                await _repository.SaveAsync();
                var inventoryToReturn = _mapper.Map<InventoryDto>(inventory);

                return new ApiOkResponse<InventoryDto>(inventoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

        }

        public async Task<ApiBaseResponse> GetAllInventoriesAsync(bool trackChanges)
        {
            try
            {

                var inventories = await _repository.Inventory.GetAllInventoriesAsync(trackChanges);
                if (inventories is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var inventoriesToReturn = _mapper.Map<IEnumerable<InventoryDto>>(inventories);
                return new ApiOkResponse<IEnumerable<InventoryDto>>(inventoriesToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
           
            
        }

        public async Task<ApiBaseResponse> GetInventoryAsync(Guid inventoryId, bool track)
        {
            try
            {
                var inventory = await _repository.Inventory.GetInventoryAsync(inventoryId, track);
                if (inventory is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var inventoryToReturn = _mapper.Map<InventoryDto>(inventory);
                return new ApiOkResponse<InventoryDto>(inventoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateInventory(Guid inventoryId, InventoryForCreationDto inventory, bool trackChanges)
        {
            try
            {
                var inventoryEntity = await _repository.Inventory.GetInventoryAsync(inventoryId, trackChanges);
                if (inventoryEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(inventory, inventoryEntity);
                await _repository.SaveAsync();
                var inventoryToReturn = _mapper.Map<InventoryDto>(inventoryEntity);
                return new ApiOkResponse<InventoryDto>(inventoryToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
          
        }
    }
}
