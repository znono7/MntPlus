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
        private readonly IUnitOfWork _unitOfWork;
        public WorkOrderService(IRepositoryManager repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiBaseResponse> BulkDeleteWorkOrder(List<Guid> workOrderIds, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync(); 
            try
            {
                var workOrders = await _repository.WorkOrder.GetWorkOrdersByIdsAsync(workOrderIds, trackChanges);
                if (workOrders is null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<WorkOrderDto> workOrdersDto = new();
                workOrdersDto = workOrders.Select (x => new WorkOrderDto(
                        Id: x.Id,
                        Name: x.Name,
                        Number: x.Number,
                        Description: x.Description,
                        Priority: x.Priority,
                        StartDate: x.StartDate,
                        DueDate: x.DueDate,
                        Type: x.Type,
                        Status: x.Status,
                        Requester: x.Requester,
                        CreatedOn: x.CreatedOn,
                        UserCreatedId: x.UserCreatedId,
                        UserCreatedBy: x.UserCreatedBy != null ? new UserByDto(x.UserCreatedBy.Id, $"{x.UserCreatedBy.FirstName} {x.UserCreatedBy.LastName}") : null,
                        UserAssignedToId: x.UserAssignedToId,
                        UserAssignedTo: x.UserAssignedTo != null ? new UserByDto(x.UserAssignedTo.Id, $"{x.UserAssignedTo.FirstName} {x.UserAssignedTo.LastName}") : null,
                        TeamAssignedToId: x.TeamAssignedToId,
                        TeamAssignedTo: x.TeamAssignedTo != null ? new TeamDto(x.TeamAssignedTo.Id, x.TeamAssignedTo.Name!) : null,
                        AssetId: x.AssetId,
                        Asset: x.Asset != null ? new AssetWorkOrderDto(x.Asset.Id, x.Asset.Name,
                        x.Asset.Location != null ? new LocatioWODto(x.Asset.Location.Id, x.Asset.Location.Name!) : null) : null)
                                                                            ).ToList();
                _repository.WorkOrder.BulkDeleteWorkOrder(workOrders);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<IEnumerable<WorkOrderDto>>(workOrdersDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
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
            await _unitOfWork.BeginTransactionAsync();
            try  
            {  
                var workOrderEntity = _mapper.Map<WorkOrder>(workOrder);
                _repository.WorkOrder.CreateWorkOrder(workOrderEntity);
                await _unitOfWork.SaveChangesAsync();
                WorkOrderHistoryCreateDto initialHistoryCreateDto = new
                   (
                       Notes: "Création de l'ordre de travail",
                       Status: "Non spécifique",
                       DateChanged: DateTime.Now,
                       ChangedById: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                       WorkOrderId: workOrderEntity.Id
                   );
                var initialHistory = _mapper.Map<WorkOrderHistory>(initialHistoryCreateDto);
                _repository.WorkOrderHistory.CreateWorkOrderHistory(initialHistory);
                await _unitOfWork.SaveChangesAsync();

                if(!string.IsNullOrEmpty(workOrderEntity.Description))
                {
                    WorkOrderHistoryCreateDto historyCreateDto2 = new
                            (
                    Notes: workOrderEntity.Description,
                    Status: "Non spécifique",
                    DateChanged: DateTime.Now,
                    ChangedById: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                    WorkOrderId: workOrderEntity.Id
                        );
                    var history2 = _mapper.Map<WorkOrderHistory>(historyCreateDto2);
                    _repository.WorkOrderHistory.CreateWorkOrderHistory(history2);
                    await _unitOfWork.SaveChangesAsync();

                }
                var savedWO = await _repository.WorkOrder.GetWorkOrderAsync(workOrderEntity.Id, false);
                if(savedWO is null)
                {
                    await _unitOfWork.RollbackTransactionAsync();

                    return new ApiNotFoundResponse("");
                }
                var workOrderToReturn = new WorkOrderDto(Id: savedWO.Id,
                                                         Name: savedWO.Name,
                                                         Number: savedWO.Number,
                                                         Description: savedWO.Description,
                                                         Priority: savedWO.Priority,
                                                         StartDate: savedWO.StartDate,
                                                         DueDate: savedWO.DueDate,
                                                         Type: savedWO.Type,
                                                         Status: savedWO.Status,
                                                         Requester: savedWO.Requester,
                                                         CreatedOn: savedWO.CreatedOn,
                                                         UserCreatedId: savedWO.UserCreatedId,
                                                         UserCreatedBy: savedWO.UserCreatedBy != null ? new UserByDto(savedWO.UserCreatedBy.Id, $"{savedWO.UserCreatedBy.FirstName} {savedWO.UserCreatedBy.LastName}") : null,
                                                         UserAssignedToId: savedWO.UserAssignedToId,
                                                         UserAssignedTo: savedWO.UserAssignedTo != null ? new UserByDto(savedWO.UserAssignedTo.Id, $"{savedWO.UserAssignedTo.FirstName} {savedWO.UserAssignedTo.LastName}") : null,
                                                         TeamAssignedToId: savedWO.TeamAssignedToId,
                                                         TeamAssignedTo: savedWO.TeamAssignedTo != null ? new TeamDto(savedWO.TeamAssignedTo.Id, savedWO.TeamAssignedTo.Name!) : null,
                                                         AssetId: savedWO.AssetId,
                                                         Asset: savedWO.Asset != null ? new AssetWorkOrderDto(savedWO.Asset.Id, savedWO.Asset.Name,
                                                         savedWO.Asset.Location != null ?
                                                        new LocatioWODto(savedWO.Asset.Location.Id, savedWO.Asset.Location.Name) : null) : null);

                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteWorkOrder(Guid workOrderId, bool trackChanges)
        {
            try 
            { 
                var workOrder = await _repository.WorkOrder.GetWorkOrderForDeleteAsync(workOrderId, trackChanges);
                if (workOrder is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var workOrderToReturn = new WorkOrderDto(Id: workOrder.Id,
                               Name: workOrder.Name,
                               Number: workOrder.Number,
                               Description: workOrder.Description,
                               Priority: workOrder.Priority,
                               StartDate: workOrder.StartDate,
                               DueDate: workOrder.DueDate,
                               Type: workOrder.Type,
                               Status: workOrder.Status,
                               Requester: workOrder.Requester,
                               CreatedOn: workOrder.CreatedOn,
                               UserCreatedId: workOrder.UserCreatedId,
                               UserCreatedBy: workOrder.UserCreatedBy != null ? new UserByDto(workOrder.UserCreatedBy.Id, $"{workOrder.UserCreatedBy.FirstName} {workOrder.UserCreatedBy.LastName}") : null,
                               UserAssignedToId: workOrder.UserAssignedToId,
                               UserAssignedTo: workOrder.UserAssignedTo != null ? new UserByDto(workOrder.UserAssignedTo.Id, $"{workOrder.UserAssignedTo.FirstName} {workOrder.UserAssignedTo.LastName}") : null,
                               TeamAssignedToId: workOrder.TeamAssignedToId,
                               TeamAssignedTo: workOrder.TeamAssignedTo != null ? new TeamDto(workOrder.TeamAssignedTo.Id, workOrder.TeamAssignedTo.Name!) : null,
                               AssetId: workOrder.AssetId,
                               Asset: workOrder.Asset != null ? new AssetWorkOrderDto(workOrder.Asset.Id, workOrder.Asset.Name,
                               workOrder.Asset.Location != null ? new LocatioWODto(workOrder.Asset.Location.Id, workOrder.Asset.Location.Name!) : null) : null);

                _repository.WorkOrder.DeleteWorkOrder(workOrder);
                await _repository.SaveAsync();
                  

                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                // Log or inspect the inner exception details
                Console.WriteLine(innerException?.Message);
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllWorkOrdersAsync(bool trackChanges)
        {
            try
            {
                var workOrders = (await _repository.WorkOrder.GetAllWorkOrdersAsync(trackChanges))!.ToList();
                if (workOrders is null)
                { 
                    return new ApiNotFoundResponse(""); 
                }
                List<WorkOrderDto> workOrdersDto = new();
                workOrdersDto = workOrders.Select(x => new WorkOrderDto
                               (Id: x.Id,
                                Name: x.Name,
                                Number: x.Number,
                                Description: x.Description,
                                Priority: x.Priority,
                                StartDate: x.StartDate,
                                DueDate: x.DueDate,
                                Type: x.Type,
                                Status: x.Status,
                                Requester: x.Requester,
                                CreatedOn: x.CreatedOn,
                                UserCreatedId: x.UserCreatedId,
                                UserCreatedBy: x.UserCreatedBy != null ? new UserByDto(x.UserCreatedBy.Id, $"{x.UserCreatedBy.FirstName} {x.UserCreatedBy.LastName}") : null,
                                UserAssignedToId: x.UserAssignedToId,
                                UserAssignedTo: x.UserAssignedTo != null ? new UserByDto(x.UserAssignedTo.Id, $"{x.UserAssignedTo.FirstName} {x.UserAssignedTo.LastName}") : null,
                                TeamAssignedToId: x.TeamAssignedToId,
                                TeamAssignedTo: x.TeamAssignedTo != null ? new TeamDto(x.TeamAssignedTo.Id, x.TeamAssignedTo.Name!) : null,
                                AssetId: x.AssetId,
                                Asset: x.Asset != null ? new AssetWorkOrderDto(x.Asset.Id, x.Asset.Name, x.Asset.Location!= null ? 
                                new LocatioWODto(x.Asset.Location.Id,x.Asset.Location.Name!) : null) : null)
                               ).ToList();
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
                var workOrderToReturn = new WorkOrderDto(Id: workOrder.Id,
                                                         Name: workOrder.Name,
                                                         Number: workOrder.Number,
                                                         Description: workOrder.Description,
                                                         Priority: workOrder.Priority,
                                                         StartDate: workOrder.StartDate,
                                                         DueDate: workOrder.DueDate,
                                                         Type: workOrder.Type,
                                                         Status: workOrder.Status,
                                                         Requester: workOrder.Requester,
                                                         CreatedOn: workOrder.CreatedOn,
                                                         UserCreatedId: workOrder.UserCreatedId,
                                                         UserCreatedBy: workOrder.UserCreatedBy != null ? new UserByDto(workOrder.UserCreatedBy.Id, $"{workOrder.UserCreatedBy.FirstName} {workOrder.UserCreatedBy.LastName}") : null,
                                                         UserAssignedToId: workOrder.UserAssignedToId,
                                                         UserAssignedTo: workOrder.UserAssignedTo != null ? new UserByDto(workOrder.UserAssignedTo.Id, $"{workOrder.UserAssignedTo.FirstName} {workOrder.UserAssignedTo.LastName}") : null,
                                                         TeamAssignedToId: workOrder.TeamAssignedToId,
                                                         TeamAssignedTo: workOrder.TeamAssignedTo != null ? new TeamDto(workOrder.TeamAssignedTo.Id, workOrder.TeamAssignedTo.Name!) : null,
                                                         AssetId: workOrder.AssetId,
                                                         Asset: workOrder.Asset != null ? new AssetWorkOrderDto(workOrder.Asset.Id, workOrder.Asset.Name,
                                                         workOrder.Asset.Location != null ?
                                                        new LocatioWODto(workOrder.Asset.Location.Id, workOrder.Asset.Location.Name) : null) : null);
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateStatWorkOrder(Guid workOrderId, string workOrderStat, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            { 
                var workOrderEntity = await _repository.WorkOrder.GetWorkOrderAsync(workOrderId, trackChanges);
                if (workOrderEntity is null)
                {
                    return new ApiNotFoundResponse("");
                } 
                workOrderEntity.Status = workOrderStat;
                await _unitOfWork.SaveChangesAsync();
                //add history
                WorkOrderHistory workOrderHistory = new WorkOrderHistory
                {
                    Notes = $"Modifier le statut de l'order de travail : {workOrderStat}",
                    Status = workOrderStat,
                    DateChanged = DateTime.Now,
                    ChangedById = workOrderEntity.UserCreatedId,
                    WorkOrderId = workOrderEntity.Id

                };
                
                _repository.WorkOrderHistory.CreateWorkOrderHistory(workOrderHistory);
                await _unitOfWork.SaveChangesAsync();
                var workOrderToReturn = new WorkOrderDto(Id: workOrderEntity.Id,
                                                         Name: workOrderEntity.Name,
                                                         Number: workOrderEntity.Number,
                                                         Description: workOrderEntity.Description,
                                                         Priority: workOrderEntity.Priority,
                                                         StartDate: workOrderEntity.StartDate,
                                                         DueDate: workOrderEntity.DueDate,
                                                         Type: workOrderEntity.Type,
                                                         Status: workOrderStat,
                                                         Requester: workOrderEntity.Requester,
                                                         CreatedOn: workOrderEntity.CreatedOn,
                                                         UserCreatedId: workOrderEntity.UserCreatedId,
                                                         UserCreatedBy: workOrderEntity.UserCreatedBy != null ? new UserByDto(workOrderEntity.UserCreatedBy.Id, $"{workOrderEntity.UserCreatedBy.FirstName} {workOrderEntity.UserCreatedBy.LastName}") : null,
                                                         UserAssignedToId: workOrderEntity.UserAssignedToId,
                                                         UserAssignedTo: workOrderEntity.UserAssignedTo != null ? new UserByDto(workOrderEntity.UserAssignedTo.Id, $"{workOrderEntity.UserAssignedTo.FirstName} {workOrderEntity.UserAssignedTo.LastName}") : null,
                                                         TeamAssignedToId: workOrderEntity.TeamAssignedToId,
                                                         TeamAssignedTo: workOrderEntity.TeamAssignedTo != null ? new TeamDto(workOrderEntity.TeamAssignedTo.Id, workOrderEntity.TeamAssignedTo.Name!) : null,
                                                         AssetId: workOrderEntity.AssetId,
                                                         Asset: workOrderEntity.Asset != null ? new AssetWorkOrderDto(workOrderEntity.Asset.Id, workOrderEntity.Asset.Name,
                                                         workOrderEntity.Asset.Location != null ? new LocatioWODto(workOrderEntity.Asset.Location.Id, workOrderEntity.Asset.Location.Name): null) : null);
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
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
                workOrderEntity.Number = workOrderEntity.Number;
                workOrderEntity.Name = workOrder.Name;
                workOrderEntity.Description = workOrder.Description;
                workOrderEntity.Priority = workOrder.Priority;
                workOrderEntity.StartDate = workOrder.StartDate;
                workOrderEntity.DueDate = workOrder.DueDate;
                workOrderEntity.Type = workOrder.Type;
                workOrderEntity.Status = workOrder.Status;
                workOrderEntity.Requester = workOrder.Requester;
                workOrderEntity.UserCreatedId = workOrder.UserCreatedId;
                workOrderEntity.UserAssignedToId = workOrder.UserAssignedToId;
                workOrderEntity.TeamAssignedToId = workOrder.TeamAssignedToId;
                workOrderEntity.AssetId = workOrder.AssetId;

               

                await _repository.SaveAsync();
                var workOrderToReturn = new WorkOrderDto(Id: workOrderEntity.Id,
                                                         Name: workOrderEntity.Name,
                                                         Number: workOrderEntity.Number,
                                                         Description: workOrderEntity.Description,
                                                         Priority: workOrderEntity.Priority,
                                                         StartDate: workOrderEntity.StartDate,
                                                         DueDate: workOrderEntity.DueDate,
                                                         Type: workOrderEntity.Type,
                                                         Status: workOrderEntity.Status,
                                                         Requester: workOrderEntity.Requester,
                                                         CreatedOn: workOrderEntity.CreatedOn,
                                                         UserCreatedId: workOrderEntity.UserCreatedId,
                                                         UserCreatedBy: workOrderEntity.UserCreatedBy != null ? new UserByDto(workOrderEntity.UserCreatedBy.Id, $"{workOrderEntity.UserCreatedBy.FirstName} {workOrderEntity.UserCreatedBy.LastName}") : null,
                                                         UserAssignedToId: workOrderEntity.UserAssignedToId,
                                                         UserAssignedTo: workOrderEntity.UserAssignedTo != null ? new UserByDto(workOrderEntity.UserAssignedTo.Id, $"{workOrderEntity.UserAssignedTo.FirstName} {workOrderEntity.UserAssignedTo.LastName}") : null,
                                                         TeamAssignedToId: workOrderEntity.TeamAssignedToId,
                                                         TeamAssignedTo: workOrderEntity.TeamAssignedTo != null ? new TeamDto(workOrderEntity.TeamAssignedTo.Id, workOrderEntity.TeamAssignedTo.Name!) : null,
                                                         AssetId: workOrderEntity.AssetId,
                                                         Asset: workOrderEntity.Asset != null ? new AssetWorkOrderDto(workOrderEntity.Asset.Id, workOrderEntity.Asset.Name, workOrderEntity.Asset.Location != null ?
                                                         new LocatioWODto(workOrderEntity.Asset.Location!.Id, workOrderEntity.Asset.Location.Name!):null) : null);
                return new ApiOkResponse<WorkOrderDto>(workOrderToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
