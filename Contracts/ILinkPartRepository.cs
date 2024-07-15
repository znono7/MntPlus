using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ILinkPartRepository
    {
        Task<IEnumerable<LinkPart>?> GetLinkPartsAsync(Guid AssetId, bool trackChanges);
        Task<LinkPart?> GetLinkPartAsync( Guid idLinkPart,Guid idAsset, bool trackChanges);
        void CreateLinkPart(LinkPart linkPart);
        void DeleteLinkPart(LinkPart linkPart);
    }
}
