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
        public AssetService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
                var asset = await _repository.Asset.GetAssetAsync(assetId, trackChanges);
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
