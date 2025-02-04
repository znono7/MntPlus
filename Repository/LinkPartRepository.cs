﻿using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class LinkPartRepository : RepositoryBase<LinkPart>, ILinkPartRepository
    {
        public LinkPartRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateLinkPart( LinkPart linkPart)
        {
            Create(linkPart);
        } 

        public void DeleteLinkPart(LinkPart linkPart) => Delete(linkPart);

        public async Task<LinkPart?> GetLinkPartAsync( Guid idLinkPart, Guid idAsset, bool trackChanges) =>
            await FindByCondition(c =>  c.PartId.Equals(idLinkPart) && c.AssetId.Equals(idAsset), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<LinkPart>?> GetLinkPartsAsync(Guid AssetId, bool trackChanges) =>
            await FindByCondition(c => c.AssetId.Equals(AssetId), trackChanges)
            .ToListAsync();
    }
    
    
}
