using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class AssetService : IAssetService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AssetService(IRepositoryManager repository, IMapper mapper , IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiBaseResponse> CreateAsset(AssetForCreationDto asset)
        {
            try
            {
                var assetEntity = _mapper.Map<Asset>(asset);  
                _repository.Asset.CreateAsset(assetEntity); 
                await _repository.SaveAsync();
                var returnEquipment = _mapper.Map < AssetDto >( await _repository.Asset.GetAssetAsync(assetEntity.Id, false));
               // var assetToReturn = _mapper.Map<AssetDto>(assetEntity);

                return new ApiOkResponse<AssetDto>(returnEquipment);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        } 

        public async Task<ApiBaseResponse> DeleteAsset(Guid assetId, bool trackChanges)
        {
           
            try
            {
                var asset = await _repository.Asset.GetAssetForDeleteAsync(assetId, trackChanges); 
                if (asset is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Asset.DeleteAsset(asset);
                await _repository.SaveAsync();
                var assetToReturn = _mapper.Map<AssetDto>(asset);

                return new ApiOkResponse<AssetDto>(assetToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
           
            
        }

        public async Task<ApiBaseResponse> DeleteAssets(IEnumerable<Guid> assetIds, bool trackChanges)
        {
            await _unitOfWork
                .BeginTransactionAsync();
            try
            {
                var assets = await _repository.Asset.GetByIdsAsync(assetIds, trackChanges);
                if (assets is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Asset.DeleteAssets(assets);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<IEnumerable<Guid>>(assetIds);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAllAssetsAsync(bool trackChanges)
        {
            try
            {
                var assets = await _repository.Asset.GetAllAssetsAsync(trackChanges);
                if (assets is null)
                { 
                    return new ApiNotFoundResponse("");  
                }
                var assetsDto = _mapper.Map<IEnumerable<AssetDto>>(assets);
                return new ApiOkResponse<IEnumerable<AssetDto>>(assetsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllPartsAsync(Guid assetId, bool trackChanges)
        {
            try
            {
                var asset = await _repository.Asset.GetAllPartsAsync(assetId, trackChanges);
                if (asset is null) 
                {
                    return new ApiNotFoundResponse("");
                }
                List<PartDto>? assetDto = new List<PartDto>(); 
                if (asset.LinkParts!= null)
                {
                    foreach (var item in asset.LinkParts)
                    {
                        List<InventoryDto>? inventories = null;
                        if (item.Part?.Inventories != null)
                        {
                            
                            inventories = item.Part.Inventories.Select(inventory =>
                            new InventoryDto(
                                inventory.Id,
                                inventory.Status,
                                inventory.Cost,
                                inventory.AvailableQuantity ?? 0,
                                inventory.MinimumQuantity,
                                inventory.MaxQuantity,
                                inventory.DateReceived ?? DateTime.Now,
                                inventory.PartID,
                                null
                            )
                        ).ToList();
                        }
                        PartDto partAssetDto = new PartDto(item.Part!.Id, item.Part.PartNumber, item.Part.Name, item.Part.Description, item.Part.Category,null, inventories,null);
                        assetDto.Add(partAssetDto);
                    }
                }
                return new ApiOkResponse<IEnumerable<PartDto>>(assetDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAssetAsync(Guid assetId, bool trackChanges)
        {
            try
            {
                var asset = await _repository.Asset.GetAssetAsync(assetId, trackChanges);
                if (asset is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var assetDto = _mapper.Map<AssetDto>(asset);
                return new ApiOkResponse<AssetDto>(assetDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> UpdateAsset(Guid assetId, AssetForCreationDto asset, bool trackChanges)
        {
            try
            {
                var assetEntity = await _repository.Asset.GetAssetAsync(assetId, trackChanges);
                if (assetEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(asset, assetEntity);
                await _repository.SaveAsync();
                var assetToReturn = _mapper.Map<AssetDto>(assetEntity);
                return new ApiOkResponse<AssetDto>(assetToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> UpdateAssetImage(Guid assetId, AssetForUpdateImage asset, bool trackChanges)
        {
            try
            {
                var assetEntity = await _repository.Asset.GetAssetAsync(assetId, trackChanges);
                if (assetEntity is null)
                { 
                    return new ApiNotFoundResponse("");
                }
                assetEntity.ImagePath = asset.ImagePath;
                assetEntity.AssetImage = asset.AssetImage;
                await _repository.SaveAsync();
                var assetToReturn = _mapper.Map<AssetDto>(assetEntity);
                return new ApiOkResponse<AssetDto>(assetToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }
    }
}
