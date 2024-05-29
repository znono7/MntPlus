using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WorkOrderHistoryService : IWorkOrderHistoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public WorkOrderHistoryService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> CreateWorkOrderHistory(WorkOrderHistoryCreateDto workOrderHistory)
        {
            try
            {
                var workOrderHistoryEntity = _mapper.Map<WorkOrderHistory>(workOrderHistory);
                _repository.WorkOrderHistory.CreateWorkOrderHistory(workOrderHistoryEntity);
                await _repository.SaveAsync();
                var workOrderHistoryToReturn = _mapper.Map<WorkOrderHistoryDto>(workOrderHistoryEntity);
                return new ApiOkResponse<WorkOrderHistoryDto>(workOrderHistoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> DeleteWorkOrderHistory(Guid workOrderHistoryId, bool trackChanges)
        {
            try
            {
                var workOrderHistory = await _repository.WorkOrderHistory.GetWorkOrderHistoryAsync(workOrderHistoryId, trackChanges);
                if (workOrderHistory is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.WorkOrderHistory.DeleteWorkOrderHistory(workOrderHistory);
                await _repository.SaveAsync();
                var workOrderHistoryToReturn = _mapper.Map<WorkOrderHistoryDto>(workOrderHistory);

                return new ApiOkResponse<WorkOrderHistoryDto>(workOrderHistoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAllWorkOrderHistoriesAsync(Guid workOrderId, bool trackChanges)
        {
            try
            {
                var workOrderHistories = await _repository.WorkOrderHistory.GetWorkOrderHistoriesAsync(workOrderId, trackChanges);
                if (workOrderHistories is null)
                {
                    return new ApiNotFoundResponse(""); 
                }
                List<WorkOrderHistoryDto> workOrderHistoriesDto = new();
                foreach (var item in workOrderHistories)
                {
                    workOrderHistoriesDto.Add(_mapper.Map<WorkOrderHistoryDto>(item));
                }
                //var workOrderHistoriesDto = _mapper.Map<IEnumerable<WorkOrderHistoryDto>>(workOrderHistories);
                return new ApiOkResponse<IEnumerable<WorkOrderHistoryDto>>(workOrderHistoriesDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetWorkOrderHistoryAsync(Guid id, bool trackChanges)
        {
            try
            {
                var workOrderHistory = await _repository.WorkOrderHistory.GetWorkOrderHistoryAsync(id, trackChanges);
                if (workOrderHistory is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var workOrderHistoryToReturn = _mapper.Map<WorkOrderHistoryDto>(workOrderHistory);
                return new ApiOkResponse<WorkOrderHistoryDto>(workOrderHistoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateWorkOrderHistory(Guid workOrderHistoryId, WorkOrderHistoryCreateDto workOrderHistory, bool trackChanges)
        {
            try
            {
                var workOrderHistoryEntity = await _repository.WorkOrderHistory.GetWorkOrderHistoryAsync(workOrderHistoryId, trackChanges);
                if (workOrderHistoryEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(workOrderHistory, workOrderHistoryEntity);
                await _repository.SaveAsync();
                var workOrderHistoryToReturn = _mapper.Map<WorkOrderHistoryDto>(workOrderHistoryEntity);
                return new ApiOkResponse<WorkOrderHistoryDto>(workOrderHistoryToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
