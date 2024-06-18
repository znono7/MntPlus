using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;
using System.Linq;

namespace Service
{
    public class TaskListService : ITaskListService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TaskListService(IRepositoryManager repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiBaseResponse> BulkDeleteCheckLists(List<Guid> idsCheckList, bool trackChanges)
        {
            try
            {
                var checkLists = await _repository.CheckList.GetCheckListsByIdsAsync(idsCheckList, trackChanges);
                if (checkLists is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.CheckList.BulkDeleteCheckLists(checkLists);
                await _repository.SaveAsync();
                return new ApiOkResponse<IEnumerable<CheckListDto>>(checkLists.Select(checkList => new CheckListDto(checkList.Id, checkList.Description, checkList.Name)));
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> CreateCheckListItems(CreateCheckListDto createCheckListDto, List<CreateCheckListItemDto>? createCheckListItems)
        {
            await _unitOfWork.BeginTransactionAsync();
            try  
            {
                var CheckListEntity = new CheckList { Description = createCheckListDto.Description,Name = createCheckListDto.Name};
                _repository.CheckList.CreateCheckList(CheckListEntity);
                await _unitOfWork.SaveChangesAsync();
                var createListEntity = new List<CheckListItem>();
                if (createCheckListItems != null)
                {
                    createListEntity = createCheckListItems.Select(item => new CheckListItem
                    {
                         Order = item.Order, Description = item.Description, CheckListId = CheckListEntity.Id 
                    }).ToList();

                }
                _repository.CheckListItem.CreateCheckListItemsAsync(createListEntity);
                await _unitOfWork.SaveChangesAsync();
                var checkListToReturn = new CheckListDto(CheckListEntity.Id, CheckListEntity.Description, CheckListEntity.Name);
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<CheckListDto>(checkListToReturn);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBaseResponse> CreateIndividualTasksItems(CreateIndividualTaskDto createIndividualTaskDto, List<CreateCheckListItemDto>? createCheckListItems)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var IndividualTaskEntity = new IndividualTask();
                _repository.IndividualTask.CreateIndividualTask(IndividualTaskEntity);
                await _unitOfWork.SaveChangesAsync();
                var createListEntity = new List<CheckListItem>();
                if (createCheckListItems != null)
                {
                    createListEntity = createCheckListItems.Select(item =>
                    new CheckListItem
                    {
                        Description = item.Description,
                        IndividualTaskId = IndividualTaskEntity.Id
                    }
                    ).ToList();

                }
                _repository.CheckListItem.CreateCheckListItemsAsync(createListEntity);
                await _unitOfWork.SaveChangesAsync();
                var individualTaskToReturn = new IndividualTaskDto(IndividualTaskEntity.Id);
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<IndividualTaskDto>(individualTaskToReturn);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public async Task<ApiBaseResponse> DeleteCheckListItems(Guid idCheckList, bool trackChanges)
        { 
            try
            {
                var checkList = await _repository.CheckList.GetCheckListAsync(idCheckList, trackChanges);
                if (checkList is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.CheckList.DeleteCheckList(checkList);
                await _repository.SaveAsync();
                return new ApiOkResponse<CheckListDto>(new CheckListDto(checkList.Id, checkList.Description, checkList.Name));
            }
            catch (Exception)
            {
                return new ApiBadRequestResponse("");

            }

           
        }

        public async Task<ApiBaseResponse> DeleteIndividualTasksItems(Guid idIndividualTasks, bool trackChanges)
        {
            try
            {
                var individualTask = await _repository.IndividualTask.GetIndividualTaskById(idIndividualTasks, trackChanges);
                if (individualTask is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.IndividualTask.DeleteIndividualTask(individualTask);
                await _repository.SaveAsync();
                return new ApiOkResponse<IndividualTaskDto>(new IndividualTaskDto(individualTask.Id));

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> DeleteTaskItem(Guid idTask, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var itemToDelete = await _repository.CheckListItem.GetCheckListItemByIdAsync(idTask, trackChanges);
                if (itemToDelete is null) 
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.CheckListItem.DeleteCheckListItemAsync(itemToDelete);
                await _unitOfWork.SaveChangesAsync();
                var items = await _repository.CheckListItem.GetCheckListItemsByCheckListIdAsync(itemToDelete.CheckListId ?? Guid.Empty, trackChanges);
                //update order
                if (items != null && items.Any())
                {
                    var order = 1;
                    foreach (var item in items)
                    {
                        item.Order = order;
                        order++;
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
               
                return new ApiOkResponse<CheckListItemDto>(new CheckListItemDto(itemToDelete.Id, itemToDelete.Order, itemToDelete.Description));

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetCheckListItems(Guid idCheckList, bool trackChanges)
        {
            try
            {
                var checkListItems = await _repository.CheckListItem.GetCheckListItemsByCheckListIdAsync(idCheckList, trackChanges);
                if (checkListItems is null)
                {
                    return new ApiNotFoundResponse(""); 
                }
                List<CheckListItemDto> checkListItemsDto = new();
                checkListItemsDto = checkListItems.Select(checkListItem => new CheckListItemDto(checkListItem.Id, checkListItem.Order, checkListItem.Description)).ToList();
                return new ApiOkResponse<IEnumerable<CheckListItemDto>>(checkListItemsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetCheckLists(bool trackChanges)
        {
            try
            {
                var checkLists = await _repository.CheckList.GetAllCheckListsAsync(trackChanges);
                if(checkLists is null)
                {
                    return new ApiNotFoundResponse("");
                }
                List<CheckListDto> checkListsDto = new();
                checkListsDto = checkLists.Select(checkList => new CheckListDto(checkList.Id, checkList.Description, checkList.Name)).ToList();
                return new ApiOkResponse<IEnumerable<CheckListDto>>(checkListsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateCheckList(Guid idCheckList, CreateCheckListDto updateCheckListDto, bool trackChanges)
        {
            try
            {
                var checkListEntity = await _repository.CheckList.GetCheckListAsync(idCheckList, trackChanges);
                if (checkListEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                checkListEntity.Name = updateCheckListDto.Name;
                checkListEntity.Description = updateCheckListDto.Description;
                await _repository.SaveAsync();
                return new ApiOkResponse<CheckListDto>(new CheckListDto(checkListEntity.Id, checkListEntity.Description, checkListEntity.Name));


            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> UpdateCheckListItems(Guid idCheckList, CreateCheckListDto updateCheckListDto,
        List<CheckListItemDto>? createCheckListItems, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var checkListEntity = await _repository.CheckList.GetCheckListAsync(idCheckList, trackChanges);
                if (checkListEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }

                checkListEntity.Name = updateCheckListDto.Name;
                checkListEntity.Description = updateCheckListDto.Description;
                await _unitOfWork.SaveChangesAsync();

                var checkListItems = await _repository.CheckListItem.GetCheckListItemsByCheckListIdAsync(idCheckList, trackChanges);
                if (checkListItems != null && checkListItems.Any())
                {
                    // Log existing checkListItems
                    Console.WriteLine("Existing CheckListItems:");
                    foreach (var item in checkListItems)
                    {
                        Console.WriteLine($"Item: {item.Id}, CheckListId: {item.CheckListId}");
                    }

                    // Delete existing CheckListItems
                    foreach (var item in checkListItems)
                    {
                        _repository.CheckListItem.DeleteCheckListItemAsync(item);
                    }
                    //_repository.CheckListItem.DeleteCheckListItemsAsync(checkListItems);

                    await _unitOfWork.SaveChangesAsync();
                }

                if (createCheckListItems != null && createCheckListItems.Any())
                {
                    // Create new CheckListItems
                    var createListEntity = createCheckListItems.Select(item => new CheckListItem
                    {
                        Order = item.Order,
                        Description = item.Description,
                        CheckListId = checkListEntity.Id
                    }).ToList();

                    //foreach (var newItem in createListEntity)
                    //{
                    //    _repository.CheckListItem.CreateCheckListItemAsync(newItem);
                    //}
                    _repository.CheckListItem.CreateCheckListItemsAsync(createListEntity);

                    await _unitOfWork.SaveChangesAsync();
                }

                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<CheckListDto>(new CheckListDto(checkListEntity.Id, checkListEntity.Description, checkListEntity.Name));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                return new ApiBadRequestResponse(ex.Message);
            }
        }




        public async Task<ApiBaseResponse> UpdateTaskItem(Guid idTask, CreateCheckListItemDto updateItemDto, bool trackChanges)
        {
            try
            {
                var item = await _repository.CheckListItem.GetCheckListItemByIdAsync(idTask, trackChanges);
                if (item is null)
                {
                    return new ApiNotFoundResponse("");
                }
                item.Description = updateItemDto.Description;
                await _repository.SaveAsync();
                return new ApiOkResponse<CheckListItemDto>(new CheckListItemDto(item.Id,item.Order, item.Description));

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        private void UpdateCheckListItems(IEnumerable<CheckListItem> checkListItems, List<CheckListItemDto>? createCheckListItems)
        {
            if (createCheckListItems == null) return;

            foreach (var paramEntity in createCheckListItems)
            {
                var dbEntity = checkListItems.FirstOrDefault(e => e.Id == paramEntity.Id);
                if (dbEntity != null)
                {
                    dbEntity.Description = paramEntity.Description;
                }
            }
        }
    }
}
