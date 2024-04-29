using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAssetService
    {
        Task<ApiBaseResponse> GetAllAssetsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAssetAsync(Guid assetId, bool trackChanges);
        Task<ApiBaseResponse> CreateAsset(AssetForCreationDto asset);
        Task<ApiBaseResponse> DeleteAsset(Guid assetId, bool trackChanges);
        Task<ApiBaseResponse> UpdateAsset(Guid assetId, AssetForCreationDto asset, bool trackChanges);
        Task<ApiBaseResponse> UpdateAssetImage(Guid assetId, AssetForUpdateImage asset, bool trackChanges);
    }
}
