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
    public class AssetRepository : RepositoryBase<Asset>, IAssetRepository
    {
        public AssetRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateAsset(Asset asset) => Create(asset);

      
       

        public void DeleteAsset(Asset asset) => Delete(asset);

        public void DeleteAssets(IEnumerable<Asset> assets) => DeleteBulk(assets);
        

        public async Task<IEnumerable<Asset>?> GetAllAssetsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(l => l.Location)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Asset?> GetAllPartsAsync(Guid assetId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(assetId), trackChanges)
            .Include(l => l.LinkParts!)
            .ThenInclude(p => p.Part)
            .ThenInclude(p => p.Inventories)
            .SingleOrDefaultAsync();
       

        public async Task<Asset?> GetAssetAsync(Guid assetId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(assetId), trackChanges)
            .Include(l => l.Location)
            .SingleOrDefaultAsync();

        public async Task<Asset?> GetAssetForDeleteAsync(Guid assetId, bool trackChanges) =>
           await FindByCondition(c => c.Id.Equals(assetId), trackChanges)
           .Include(l => l.LinkParts)
            .Include(l => l.Meters)
           
           .SingleOrDefaultAsync();

        public async Task<IEnumerable<Asset>?> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => ids.Contains(c.Id), trackChanges)
            
            .ToListAsync();
       
    }
}
