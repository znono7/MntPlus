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

       
        public async Task<IEnumerable<Asset>?> GetAllAssetsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

       

        public async Task<Asset?> GetAssetAsync(Guid assetId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(assetId), trackChanges)
            .SingleOrDefaultAsync();

      
       
   
    }
}
