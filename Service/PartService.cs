using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class PartService : IPartService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public PartService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreatePart(PartForCreationDto part)
        {
            try
            {
                var partEntity = _mapper.Map<Part>(part);
                _repository.Part.CreatePart(partEntity); 
                await _repository.SaveAsync();
                var partToReturn = _mapper.Map<PartDto>(partEntity);
                return new ApiOkResponse<PartDto>(partToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeletePart(Guid partId, bool trackChanges)
        {
            try
            {
                var part = await _repository.Part.GetPartAsync(partId, trackChanges);
                if (part is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Part.DeletePart(part);
                await _repository.SaveAsync();
                var partToReturn = _mapper.Map<PartDto>(part);

                return new ApiOkResponse<PartDto>(partToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllPartsAsync(bool trackChanges)
        {
            try
            {
                var parts = await _repository.Part.GetAllPartsAsync(trackChanges);
                if (parts is null)
                {
                    return new ApiNotFoundResponse("");
                }

                List<PartDto> partsDto = new List<PartDto>();

                foreach (var part in parts)
                {
                    List<InventoryDto>? inventories = null;
                    if (part.Inventories is not null)
                    {
                        inventories = part.Inventories.Select(inventory =>
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

                    List<LinkPartDto>? linkPartDtos = null;
                    if (part.LinkParts is not null)
                    {
                        linkPartDtos = part.LinkParts.Select(item =>
                            new LinkPartDto(
                                item.Id,
                                item.PartId,
                                _mapper.Map<PartDto>(item.Part),
                                item.AssetId,
                                _mapper.Map<AssetDto>(item.Asset)
                            )
                        ).ToList();
                    }

                    partsDto.Add(new PartDto(
                        part.Id,
                        part.PartNumber,
                        part.Name,
                        part.Description,
                        part.Category,
                        part.Image,
                        inventories,
                        linkPartDtos
                    ));
                }

                return new ApiOkResponse<IEnumerable<PartDto>>(partsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }



        public async Task<ApiBaseResponse> GetPartAsync(Guid partId, bool trackChanges)
        {
            try
            {
                var part = await _repository.Part.GetPartAsync(partId, trackChanges);
                if (part is null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<InventoryDto>? inventories = null;
                if (part.Inventories is not null)
                {
                    inventories = part.Inventories.Select(inventory =>
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
                List<LinkPartDto>? linkPartDtos = null;
                if (part.LinkParts is not null)
                {
                    linkPartDtos = part.LinkParts.Select(item =>
                        new LinkPartDto(
                            item.Id,
                            item.PartId,
                            _mapper.Map<PartDto>(item.Part),
                            item.AssetId,
                            _mapper.Map<AssetDto>(item.Asset)
                        )
                    ).ToList();
                }
                var partDto = new PartDto(
                                       part.Id,
                                       part.PartNumber,
                                       part.Name,
                                       part.Description,
                                       part.Category,
                                       part.Image,
                                       inventories,
                                       linkPartDtos
                                   );
                return new ApiOkResponse<PartDto>(partDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdatePart(Guid partId, PartForCreationDto part, bool trackChanges)
        {
            try
            {
                var partEntity = await _repository.Part.GetPartAsync(partId, trackChanges);
                if (partEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(part, partEntity);
                await _repository.SaveAsync();
                var partToReturn = _mapper.Map<PartDto>(partEntity);
                return new ApiOkResponse<PartDto>(partToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
                
            }

            
        }
    }
}
