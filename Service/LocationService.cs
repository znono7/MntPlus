using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class LocationService : ILocationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public LocationService(IRepositoryManager repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> CreateLocation(LocationForCreationDto location)
        {
            try 
            {
                var locationEntity = _mapper.Map<Location>(location);
                _repository.Location.CreateLocation(locationEntity);
                await _repository.SaveAsync();
                var locationToReturn = _mapper.Map<LocationDto>(locationEntity);
                return new ApiOkResponse<LocationDto>(locationToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

        }
       
        public async Task<ApiBaseResponse> DeleteLocation(Guid locationId, bool trackChanges)
        {
            try
            {
                var location = await _repository.Location.GetLocationAsync(locationId, trackChanges);
                if (location is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Location.DeleteLocation(location);
                await _repository.SaveAsync();
                var locationToReturn = _mapper.Map<LocationDto>(location);

                return new ApiOkResponse<LocationDto>(locationToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> GetAllLocationsAsync(bool trackChanges)
        {
            try
            { 
                var locations = await _repository.Location.GetAllLocationsAsync(trackChanges);
                if(locations is null)
                { 
                    return new ApiNotFoundResponse("");
                }  
                var locationsDto = _mapper.Map<IEnumerable<LocationDto>>(locations);
                return new ApiOkResponse<IEnumerable<LocationDto>>(locationsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> GetLocationAsync(Guid locationId, bool trackChanges)
        {
            try
            {
                var location = await _repository.Location.GetLocationAsync(locationId, trackChanges);
                if (location is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var locationDto = _mapper.Map<LocationDto>(location);
                return new ApiOkResponse<LocationDto>(locationDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
            
        }

        public async Task<ApiBaseResponse> UpdateLocation(Guid locationId, LocationForCreationDto location, bool trackChanges)
        {
            try
            {
                var locationEntity = await _repository.Location.GetLocationAsync(locationId, trackChanges);
                if (locationEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(location, locationEntity);
                await _repository.SaveAsync();
                var locationToReturn = _mapper.Map<LocationDto>(locationEntity);
                return new ApiOkResponse<LocationDto>(locationToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
        }
    }
}
