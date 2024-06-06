using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class MeterService : IMeterService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public MeterService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateMeter(MeterDtoForCreation meter)
        {
            try
            { 
                var meterEntity = _mapper.Map<Meter>(meter);
                _repository.Meter.CreateMeter(meterEntity);
                
                await _repository.SaveAsync();
                var meterToReturn = _mapper.Map<MeterDto>(meterEntity);
                return new ApiOkResponse<MeterDto>(meterToReturn);


            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
            
        }

        public async Task<ApiBaseResponse> DeleteMeter(Guid meterId, bool trackChanges)
        {
            try
            {
                var meter = await _repository.Meter.GetMeterAsync(meterId, trackChanges);
                if (meter is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Meter.DeleteMeter(meter);
                await _repository.SaveAsync();
                var meterToReturn = _mapper.Map<MeterDto>(meter);

                return new ApiOkResponse<MeterDto>(meterToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBaseResponse> GetAllMetersAsync(bool trackChanges)
        {
            try
            {
                var meters = await _repository.Meter.GetAllMetersAsync(trackChanges);
                if (meters is null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<MeterDto> metersToReturn = new List<MeterDto>();
                foreach (var meter in meters)
                {
                    List<MeterReadingDto>? MeterReadings = null;
                    if(meter.MeterReadings is not null)
                    {
                        MeterReadings = meter.MeterReadings.Select(m => 
                        new MeterReadingDto(m.Id, m.MeterId, m.Reading, m.Timestamp, m.UserId,m.User!= null ? $"{m.User!.FirstName} {m.User!.LastName}" : string.Empty)).ToList();

                    }
                    metersToReturn.Add(new MeterDto(meter.Id, meter.Name, meter.CurrentReading, meter.LastUpdated, meter.Unit, meter.Frequency, meter.FrequencyUnit,meter.AssetId, meter.Asset!.Name, MeterReadings));

                }
                return new ApiOkResponse<IEnumerable<MeterDto>>(metersToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBaseResponse> GetMeterAsync(Guid meterId, bool trackChanges)
        {
            try
            {
                var meter = await _repository.Meter.GetMeterAsync(meterId, trackChanges);
                if (meter is null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<MeterReadingDto>? MeterReadings = null;

                
                    if (meter.MeterReadings is not null)
                    {
                        MeterReadings = meter.MeterReadings.Select(m =>
                        new MeterReadingDto(m.Id, m.MeterId, m.Reading, m.Timestamp, m.UserId, $"{m.User!.FirstName} {m.User!.LastName}")).ToList();

                    }
                   
                
                
                    
                var meterToReturn = new MeterDto(meter.Id, meter.Name, meter.CurrentReading, meter.LastUpdated, meter.Unit, meter.Frequency, meter.FrequencyUnit, meter.AssetId, meter.Asset!.Name, MeterReadings);
                    
                return new ApiOkResponse<MeterDto>(meterToReturn);
                

                

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBadRequestResponse> RemoveBulkMetersAsync(List<Guid> guids)
        {
            try
            {
                foreach (var meterId in guids)
                {
                    var meter = await _repository.Meter.GetMeterAsync(meterId, false);
                    if (meter is null)
                    {
                        continue;
                    }else
                    {
                        _repository.Meter.DeleteMeter(meter);
                    }
                }

                await _repository.SaveAsync();
                return new ApiBadRequestResponse("");

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBaseResponse> UpdateMeter(Guid meterId, MeterDtoForCreation meter, bool trackChanges)
        {
            try
            {
                var meterEntity = await _repository.Meter.GetMeterAsync(meterId, trackChanges);
                if (meterEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(meter, meterEntity); 
                await _repository.SaveAsync();
                var meterToReturn = _mapper.Map<MeterDto>(meterEntity);
                return new ApiOkResponse<MeterDto>(meterToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
