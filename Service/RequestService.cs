using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class RequestService : IRequestService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RequestService(IRepositoryManager repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiBaseResponse> BulkDeleteRequest(List<Guid> requestIds, bool trackChanges)
        {
            try 
            {
                var requests = await _repository.Request.GetRequestsByIdsAsync(requestIds, trackChanges);
                if (requests == null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Request.BulkDeleteRequest(requests);
                await _repository.SaveAsync();
                return new ApiOkResponse<string>("Requests Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> CreateLastNumberRequest()
        {
            try
            {
                var lastRequest = await _repository.Request.GetNextRequestNumberAsync();
                
                return new ApiOkResponse<int>(lastRequest);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> CreateRequest(RequestForCreationDto request)
        {
            try
            { 
                //create request with WorkOrderHistory
                var requestEntity = _mapper.Map<Request>(request);
                _repository.Request.CreateRequest(requestEntity);
                await _repository.SaveAsync();
               
                var requestToReturn = new RequestDto(Id: requestEntity.Id,
                Name: requestEntity.Name,
                Number: requestEntity.Number,
                Description: requestEntity.Description,
                Priority: requestEntity.Priority,
                StartDate: requestEntity.StartDate,
                DueDate: requestEntity.DueDate,
                Type: requestEntity.Type,
                Status: requestEntity.Status,
                Requester: requestEntity.Requester,
                CreatedOn: requestEntity.CreatedOn,
                UserCreatedId: requestEntity.UserCreatedId,
                UserCreatedBy: requestEntity.UserCreatedBy != null ? new UserByDto(requestEntity.UserCreatedBy.Id, $"{requestEntity.UserCreatedBy.FirstName} {requestEntity.UserCreatedBy.LastName}") : null,
                UserAssignedToId: requestEntity.UserAssignedToId,
                UserAssignedTo: requestEntity.UserAssignedTo != null ? new UserByDto(requestEntity.UserAssignedTo.Id, $"{requestEntity.UserAssignedTo.FirstName} {requestEntity.UserAssignedTo.LastName}") : null,
                TeamAssignedToId: requestEntity.TeamAssignedToId,
                TeamAssignedTo: requestEntity.TeamAssignedTo != null ? new TeamDto(requestEntity.TeamAssignedTo.Id, requestEntity.TeamAssignedTo.Name!) : null,
                AssetId: requestEntity.AssetId,
                Asset: requestEntity.Asset != null ? new AssetWorkOrderDto(requestEntity.Asset.Id, requestEntity.Asset.Name,
                requestEntity.Asset.Location != null ? new LocatioWODto(requestEntity.Asset.Location.Id, requestEntity.Asset.Location.Name!) : null) : null,
                requestEntity.LastUpdated);


                return new ApiOkResponse<RequestDto>(requestToReturn);


            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> DeleteRequest(Guid requestId, bool trackChanges)
        {
            try
            {
                var request = await _repository.Request.GetRequestForDeleteAsync(requestId, trackChanges);
                if (request == null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Request.DeleteRequest(request);
                await _repository.SaveAsync();
              
                return new ApiOkResponse<string>("Request Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetAllRequestsAsync(bool trackChanges)
        {
            try
            {
                var requests = await _repository.Request.GetAllRequestsAsync(trackChanges);
                if (requests == null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<RequestDto> requestDtos = new();
                requestDtos = requests.Select(requestEntity => new RequestDto(Id: requestEntity.Id,
                Name: requestEntity.Name,
                Number: requestEntity.Number,
                Description: requestEntity.Description,
                Priority: requestEntity.Priority,
                StartDate: requestEntity.StartDate,
                DueDate: requestEntity.DueDate,
                Type: requestEntity.Type,
                Status: requestEntity.Status,
                Requester: requestEntity.Requester,
                CreatedOn: requestEntity.CreatedOn,
                UserCreatedId: requestEntity.UserCreatedId,
                UserCreatedBy: requestEntity.UserCreatedBy != null ? new UserByDto(requestEntity.UserCreatedBy.Id, $"{requestEntity.UserCreatedBy.FirstName} {requestEntity.UserCreatedBy.LastName}") : null,
                UserAssignedToId: requestEntity.UserAssignedToId,
                UserAssignedTo: requestEntity.UserAssignedTo != null ? new UserByDto(requestEntity.UserAssignedTo.Id, $"{requestEntity.UserAssignedTo.FirstName} {requestEntity.UserAssignedTo.LastName}") : null,
                TeamAssignedToId: requestEntity.TeamAssignedToId,
                TeamAssignedTo: requestEntity.TeamAssignedTo != null ? new TeamDto(requestEntity.TeamAssignedTo.Id, requestEntity.TeamAssignedTo.Name!) : null,
                AssetId: requestEntity.AssetId,
                Asset: requestEntity.Asset != null ? new AssetWorkOrderDto(requestEntity.Asset.Id, requestEntity.Asset.Name,
                requestEntity.Asset.Location != null ? new LocatioWODto(requestEntity.Asset.Location.Id, requestEntity.Asset.Location.Name!) : null) : null,
                requestEntity.LastUpdated)
                ).ToList();

                return new ApiOkResponse<IEnumerable<RequestDto>>(requestDtos);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetRequestAsync(Guid requestId, bool trackChanges)
        {
            try
            {
                var request = await _repository.Request.GetRequestAsync(requestId, trackChanges);
                if (request == null)
                {
                    return new ApiNotFoundResponse("");
                }
                var requestDto = new RequestDto(
                    Id: request.Id,
                    Name: request.Name,
                    Number: request.Number,
                    Description: request.Description,
                    Priority: request.Priority,
                    StartDate: request.StartDate,
                    DueDate: request.DueDate,
                    Type: request.Type,
                    Status: request.Status,
                    Requester: request.Requester,
                    CreatedOn: request.CreatedOn,
                    UserCreatedId: request.UserCreatedId,
                    UserCreatedBy: request.UserCreatedBy != null ? new UserByDto(request.UserCreatedBy.Id, $"{request.UserCreatedBy.FirstName} {request.UserCreatedBy.LastName}") : null,
                    UserAssignedToId: request.UserAssignedToId,
                    UserAssignedTo: request.UserAssignedTo != null ? new UserByDto(request.UserAssignedTo.Id, $"{request.UserAssignedTo.FirstName} {request.UserAssignedTo.LastName}") : null,
                    TeamAssignedToId: request.TeamAssignedToId,
                    TeamAssignedTo: request.TeamAssignedTo != null ? new TeamDto(request.TeamAssignedTo.Id, request.TeamAssignedTo.Name!) : null,
                    AssetId: request.AssetId,
                    Asset: request.Asset != null ? new AssetWorkOrderDto(request.Asset.Id, request.Asset.Name,
                    request.Asset.Location
                    != null ? new LocatioWODto(request.Asset.Location.Id, request.Asset.Location.Name!) : null) : null,request.LastUpdated);

                return new ApiOkResponse<RequestDto>(requestDto);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateRequest(Guid requestId, RequestForCreationDto request, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var requestEntity = await _repository.Request.GetRequestAsync(requestId, trackChanges);
                if (requestEntity == null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(request, requestEntity);
                await _unitOfWork.SaveChangesAsync();
                var requestToReturn = new RequestDto(
                    Id: requestEntity.Id,           
                    Name: requestEntity.Name,                            
                    Number: requestEntity.Number,                                            
                    Description: requestEntity.Description,
                    Priority: requestEntity.Priority,                                                                   
                    StartDate: requestEntity.StartDate,
                    DueDate: requestEntity.DueDate,
                    Type: requestEntity.Type,
                    Status: requestEntity.Status,
                    Requester: requestEntity.Requester,
                    CreatedOn: requestEntity.CreatedOn,
                    UserCreatedId: requestEntity.UserCreatedId,
                    UserCreatedBy: requestEntity.UserCreatedBy != null ? new UserByDto(requestEntity.UserCreatedBy.Id, $"{requestEntity.UserCreatedBy.FirstName} {requestEntity.UserCreatedBy.LastName}") : null,
                    UserAssignedToId: requestEntity.UserAssignedToId,
                    UserAssignedTo: requestEntity.UserAssignedTo != null ? new UserByDto(requestEntity.UserAssignedTo.Id, $"{requestEntity.UserAssignedTo.FirstName} {requestEntity.UserAssignedTo.LastName}") : null,
                    TeamAssignedToId: requestEntity.TeamAssignedToId,
                    TeamAssignedTo: requestEntity.TeamAssignedTo != null ? new TeamDto(requestEntity.TeamAssignedTo.Id, requestEntity.TeamAssignedTo.Name!) : null,
                    AssetId: requestEntity.AssetId,
                    Asset: requestEntity.Asset != null ? new AssetWorkOrderDto(requestEntity.Asset.Id, requestEntity.Asset.Name,
                    requestEntity.Asset.Location != null ? new LocatioWODto(requestEntity.Asset.Location.Id, requestEntity.Asset.Location.Name!) : null) : null,requestEntity.LastUpdated);

                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<RequestDto>(requestToReturn);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateStatRequest(Guid requestId, string requestStat, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var requestEntity = await _repository.Request.GetRequestAsync(requestId, trackChanges);
                if (requestEntity == null)
                {
                    return new ApiNotFoundResponse(""); 
                }
                requestEntity.Status = requestStat;
                requestEntity.LastUpdated = DateTime.Now;
                await _unitOfWork.SaveChangesAsync();
                if(requestStat == "Approuvé")
                {
                    var workOrder = new WorkOrder
                    {
                        Name = requestEntity.Name,
                        Number = await _repository.WorkOrder.GetNextWorkOrderNumberAsync(),
                        Description = requestEntity.Description,
                        Priority = requestEntity.Priority,
                        StartDate = requestEntity.StartDate,
                        DueDate = requestEntity.DueDate,
                        Type = requestEntity.Type,
                        Status = requestStat,
                        Requester = requestEntity.Requester,
                        CreatedOn = DateTime.Now,
                        UserCreatedId = requestEntity.UserCreatedId,
                        UserAssignedToId = requestEntity.UserAssignedToId,
                        TeamAssignedToId = requestEntity.TeamAssignedToId,
                        AssetId = requestEntity.AssetId,
                    };
               
                    _repository.WorkOrder.CreateWorkOrder(workOrder);
                    await _unitOfWork.SaveChangesAsync();

                    WorkOrderHistoryCreateDto initialHistoryCreateDto = new
                  (
                      Notes: "Création de l'ordre de travail",
                      Status: requestStat,
                      DateChanged: DateTime.Now,
                      ChangedById: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                      WorkOrderId: workOrder.Id
                  );
                    var initialHistory = _mapper.Map<WorkOrderHistory>(initialHistoryCreateDto);
                    _repository.WorkOrderHistory.CreateWorkOrderHistory(initialHistory);
                    await _unitOfWork.SaveChangesAsync();
                    if (!string.IsNullOrEmpty(requestEntity.Description))
                    {
                        WorkOrderHistoryCreateDto historyCreateDto2 = new
                                (
                        Notes: requestEntity.Description,
                        Status: requestStat,
                        DateChanged: DateTime.Now,
                        ChangedById: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                        WorkOrderId: requestEntity.Id
                            );
                        var history2 = _mapper.Map<WorkOrderHistory>(historyCreateDto2);
                        _repository.WorkOrderHistory.CreateWorkOrderHistory(history2);
                        await _unitOfWork.SaveChangesAsync();

                    }
                }
                var requestToReturn = new RequestDto(
                                       Id: requestEntity.Id,
                                       Name: requestEntity.Name,
                                       Number: requestEntity.Number,
                                       Description: requestEntity.Description,
                                       Priority: requestEntity.Priority,
                                       StartDate: requestEntity.StartDate,
                                       DueDate: requestEntity.DueDate,
                                       Type: requestEntity.Type,
                                       Status: requestEntity.Status,
                                        Requester: requestEntity.Requester,
                                        CreatedOn: requestEntity.CreatedOn,
                                        UserCreatedId: requestEntity.UserCreatedId,
                                        UserCreatedBy: requestEntity.UserCreatedBy != null ? new UserByDto(requestEntity.UserCreatedBy.Id, $"{requestEntity.UserCreatedBy.FirstName} {requestEntity.UserCreatedBy.LastName}") : null,
                                        UserAssignedToId: requestEntity.UserAssignedToId,
                                        UserAssignedTo: requestEntity.UserAssignedTo != null ? new UserByDto(requestEntity.UserAssignedTo.Id, $"{requestEntity.UserAssignedTo.FirstName} {requestEntity.UserAssignedTo.LastName}") : null,
                                        TeamAssignedToId: requestEntity.TeamAssignedToId,
                                        TeamAssignedTo: requestEntity.TeamAssignedTo != null ? new TeamDto(requestEntity.TeamAssignedTo.Id, requestEntity.TeamAssignedTo.Name!) : null,
                                        AssetId: requestEntity.AssetId,
                                        Asset: requestEntity.Asset != null ? new AssetWorkOrderDto(requestEntity.Asset.Id, requestEntity.Asset.Name,
                                    requestEntity.Asset.Location != null ? new LocatioWODto(requestEntity.Asset.Location.Id, requestEntity.Asset.Location.Name!) : null) : null,
                                        requestEntity.LastUpdated);
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<RequestDto>(requestToReturn);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }
    }
}
