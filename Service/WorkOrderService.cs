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
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public WorkOrderService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateLastNumberWorkOrder()
        {
            try
            {
                int? Num = await _repository.WorkOrder.GetNextWorkOrderNumberAsync();
                return new ApiOkResponse<int?>(Num);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> CreateWorkOrder(WorkOrderForCreationDto workOrder)
        {
            try  
            {
                int? Num = await _repository.WorkOrder.GetNextWorkOrderNumberAsync();
                var workOrderCreation = new WorkOrderForCreationDto
                (
                    Name : workOrder.Name,
                    Number : Num,
                    Description : workOrder.Description,
                    Priority : workOrder.Priority,
                    StartDate : workOrder.StartDate,
                    DueDate : workOrder.DueDate,
                    Type : workOrder.Type,
                    Status : workOrder.Status,
                    Requester : workOrder.Requester,
                    CreatedOn : workOrder.CreatedOn,
                    UserCreatedId : workOrder.UserCreatedId,
                    UserAssignedToId : workOrder.UserAssignedToId,
                    TeamAssignedToId : workOrder.TeamAssignedToId,
                    AssetId: workOrder.AssetId
                );
                
                var workOrderEntity = _mapper.Map<WorkOrder>(workOrderCreation);
                _repository.WorkOrder.CreateWorkOrder(workOrderEntity);
                await _repository.SaveAsync();
                var workOrderToReturn = _mapper.Map<WorkOrderDto>(workOrderEntity);
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteWorkOrder(Guid workOrderId, bool trackChanges)
        {
            try
            {
                var workOrder = await _repository.WorkOrder.GetWorkOrderAsync(workOrderId, trackChanges);
                if (workOrder is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.WorkOrder.DeleteWorkOrder(workOrder);
                await _repository.SaveAsync();
                var workOrderToReturn = _mapper.Map<WorkOrderDto>(workOrder);

                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllWorkOrdersAsync(bool trackChanges)
        {
            try
            {
                var workOrders = await _repository.WorkOrder.GetAllWorkOrdersAsync(trackChanges);
                if (workOrders is null)
                { 
                    return new ApiNotFoundResponse("");
                }
                var workOrdersDto = _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
                return new ApiOkResponse<IEnumerable<WorkOrderDto>>(workOrdersDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetWorkOrderAsync(Guid workOrderId, bool trackChanges)
        {
            try
            {
                var workOrder = await _repository.WorkOrder.GetWorkOrderAsync(workOrderId, trackChanges);
                if (workOrder is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var workOrderToReturn = _mapper.Map<WorkOrderDto>(workOrder);
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateWorkOrder(Guid workOrderId, WorkOrderForCreationDto workOrder, bool trackChanges)
        {
            try
            {
                var workOrderEntity = await _repository.WorkOrder.GetWorkOrderAsync(workOrderId, trackChanges);
                if (workOrderEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(workOrder, workOrderEntity);
                await _repository.SaveAsync();
                var workOrderToReturn = _mapper.Map<WorkOrderDto>(workOrderEntity);
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
