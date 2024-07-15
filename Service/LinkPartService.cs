using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class LinkPartService : ILinkPartService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public LinkPartService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateLinkPart(LinkPartForCreationDto linkPart)
        {
            try
            {
                var linkPartEntity = new LinkPart
                {
                    PartId = linkPart.PartId,
                    AssetId = linkPart.AssetId
                };
                _repository.LinkPart.CreateLinkPart(linkPartEntity);
                await _repository.SaveAsync();
                var linkPartToReturn = new LinkPartDto(linkPartEntity.Id, linkPartEntity.PartId,null, linkPartEntity.AssetId , null);
                return new ApiOkResponse<LinkPartDto>(linkPartToReturn);
            }
            catch (Exception ex) 
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteLinkPart(Guid linkPartId, Guid idAsset, bool trackChanges)
        {
            try
            {
                var linkPart = await _repository.LinkPart.GetLinkPartAsync(linkPartId, idAsset, trackChanges);
                if (linkPart is null)
                { 
                    return new ApiNotFoundResponse("");
                }
                _repository.LinkPart.DeleteLinkPart(linkPart);
                await _repository.SaveAsync();
                var linkPartToReturn = new LinkPartDto(linkPart.Id, linkPart.PartId, null, linkPart.AssetId, null);

                return new ApiOkResponse<LinkPartDto>(linkPartToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

       

        public async Task<ApiBaseResponse> GetAllLinkPartsAsync(Guid idAsset, bool trackChanges)
        {
            try
            {
                var linkParts = await _repository.LinkPart.GetLinkPartsAsync(idAsset,trackChanges);
                if (linkParts is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var linkPartsToReturn = _mapper.Map<IEnumerable<LinkPartDto>>(linkParts);
                return new ApiOkResponse<IEnumerable<LinkPartDto>>(linkPartsToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetLinkPartAsync(Guid linkPartId, Guid idAsset, bool trackChanges)
        {
            try
            {
                var linkPart = await _repository.LinkPart.GetLinkPartAsync(linkPartId, idAsset, trackChanges);
                if (linkPart is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var linkPartToReturn = _mapper.Map<LinkPartDto>(linkPart);
                return new ApiOkResponse<LinkPartDto>(linkPartToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

       
    }
}
