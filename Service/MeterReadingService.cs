using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;
using System.Xml.Linq;

namespace Service
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MeterReadingService(IRepositoryManager repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiBaseResponse> CreateMeterReading(MeterReadingDtoForCreation meterReading)
        {
            await _unitOfWork.BeginTransactionAsync();
            try  
            {
                var meterReadingEntity = _mapper.Map<MeterReading>(meterReading);
                _repository.MeterReading.CreateMeterReading(meterReadingEntity);
                await _unitOfWork.SaveChangesAsync(); 
                var meter = await _repository.Meter.GetMeterAsync(meterReading.MeterId, true);
                if (meter == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiNotFoundResponse("Meter not found.");
                }
                meter.CurrentReading = meterReading.Reading;
                meter.LastUpdated = meterReading.Timestamp;
                await _unitOfWork.SaveChangesAsync();
                var meterReadingToReturn = new MeterReadingDto
                (
                   Id: meterReadingEntity.Id,
                   MeterId: meterReadingEntity.MeterId,
                   Reading: meterReadingEntity.Reading,
                   Timestamp: meterReadingEntity.Timestamp,
                   UserId: meterReadingEntity.User != null ? meterReadingEntity.UserId : null,
                   UserFullName: meterReadingEntity.User != null ? $"{meterReadingEntity.User.FirstName} {meterReadingEntity.User.LastName}" : string.Empty
                                                                                                                                                     );
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<MeterReadingDto>(meterReadingToReturn);

              
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteMeterReading(Guid meterReadingId, bool trackChanges)
        {

            try 
            {
                var meterReading = await _repository.MeterReading.GetMeterReadingAsync(meterReadingId, trackChanges);
                if (meterReading is null)
                { 
                    return new ApiNotFoundResponse("");
                }
                _repository.MeterReading.DeleteMeterReading(meterReading);
                await _repository.SaveAsync(); 
                
                var meterReadingToReturn = new MeterReadingDto
                (
                  Id: meterReading.Id,
                  MeterId: meterReading.MeterId,
                  Reading: meterReading.Reading,
                  Timestamp: meterReading.Timestamp,
                  UserId: meterReading.User != null ? meterReading.UserId : Guid.Empty,
                  UserFullName: meterReading.User != null ? $"{meterReading.User.FirstName} {meterReading.User.LastName}" : string.Empty
                 );

                return new ApiOkResponse<MeterReadingDto>(meterReadingToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> DeleteUpdateMeterReading(Guid meterReadingId, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try  
            {
                var meterReading = await _repository.MeterReading.GetMeterReadingAsync(meterReadingId, trackChanges);
                if (meterReading == null) 
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.MeterReading.DeleteMeterReading(meterReading);
                await _unitOfWork.SaveChangesAsync();

                var meter = await _repository.Meter.GetMeterAsync(meterReading.MeterId, true);
                if (meter == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiNotFoundResponse("Meter not found.");
                }
                meter.CurrentReading = 0;
                meter.LastUpdated = meterReading.Timestamp;
              
                await _unitOfWork.SaveChangesAsync();

                var meterReadingToReturn = new MeterReadingDto
                (
                   Id: meterReading.Id,
                   MeterId: meterReading.MeterId,
                   Reading: meterReading.Reading,
                   Timestamp: meterReading.Timestamp,
                   UserId: meterReading.User != null ? meterReading.UserId : Guid.Empty,
                   UserFullName: meterReading.User != null ? $"{meterReading.User.FirstName} {meterReading.User.LastName}" : string.Empty
                 );

                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<MeterReadingDto>(meterReadingToReturn);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAllMeterReadingsAsync(Guid meterId, bool trackChanges)
        {
            try
            {
                var meterReadings = await _repository.MeterReading.GetAllMeterReadingsAsync(meterId, trackChanges);
                if (meterReadings == null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<MeterReadingDto> meterReadingDtos = new(); 
                foreach (var meterReading in meterReadings)
                {
                    var meterReadingDto = new MeterReadingDto
                    (
                      Id: meterReading.Id,
                      MeterId: meterReading.MeterId,
                      Reading: meterReading.Reading,
                      Timestamp: meterReading.Timestamp,
                      UserId: meterReading.UserId,
                      UserFullName: meterReading.User != null ? $"{meterReading.User.FirstName} {meterReading.User.LastName}" : string.Empty
                    );
                    meterReadingDtos.Add(meterReadingDto);
                }
                return new ApiOkResponse<IEnumerable<MeterReadingDto>>(meterReadingDtos);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetMeterReadingAsync(Guid meterReadingId, bool trackChanges)
        {
            try
            {
                var meterReading = await _repository.MeterReading.GetMeterReadingAsync(meterReadingId, trackChanges);
                if (meterReading is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var meterReadingDto = _mapper.Map<MeterReadingDto>(meterReading);
                return new ApiOkResponse<MeterReadingDto>(meterReadingDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateMeterReading(Guid meterReadingId, MeterReadingDtoForCreation meterReading, bool trackChanges)
        {
            try
            {
                var meterReadingEntity = await _repository.MeterReading.GetMeterReadingAsync(meterReadingId, trackChanges);
                if (meterReadingEntity is null)
                { 
                    return new ApiNotFoundResponse("");
                }
                meterReadingEntity.Reading = meterReading.Reading;
                await _repository.SaveAsync();
                var meterReadingToReturn = new MeterReadingDto
                    (
                      Id: meterReadingEntity.Id,
                      MeterId: meterReadingEntity.MeterId,
                      Reading: meterReadingEntity.Reading,
                      Timestamp: meterReadingEntity.Timestamp,
                      UserId: meterReadingEntity.UserId,
                      UserFullName: meterReadingEntity.User != null ? $"{meterReadingEntity.User.FirstName} {meterReadingEntity.User.LastName}" : string.Empty
                    );
                return new ApiOkResponse<MeterReadingDto>(meterReadingToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
