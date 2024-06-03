using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ILinkPartService 
    {
        Task<ApiBaseResponse> GetAllLinkPartsAsync(Guid idAsset, bool trackChanges);
        Task<ApiBaseResponse> GetLinkPartAsync(Guid AssetId, bool trackChanges);
        Task<ApiBaseResponse> CreateLinkPart(LinkPartForCreationDto linkPart);
        Task<ApiBaseResponse> DeleteLinkPart(Guid linkPartId, bool trackChanges);

    }
}
