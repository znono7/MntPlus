using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>?> GetAllAssetsAsync(bool trackChanges);
        Task<Asset?> GetAllPartsAsync(Guid assetId ,bool trackChanges);
        Task<Asset?> GetAssetAsync(Guid assetId , bool trackChanges);
        Task<Asset?> GetAssetForDeleteAsync(Guid assetId , bool trackChanges);
        void CreateAsset(Asset asset);
        void DeleteAsset(Asset asset);
        void DeleteAssets(IEnumerable<Asset> assets);
        Task<IEnumerable<Asset>?> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
