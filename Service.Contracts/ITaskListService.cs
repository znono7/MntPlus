using Entities;
using Shared;

namespace Service.Contracts
{
    public interface ITaskListService
    { 
        Task<ApiBaseResponse> GetCheckLists(bool trackChanges);
        Task<ApiBaseResponse> GetCheckListItems(Guid idCheckList, bool trackChanges);
        Task<ApiBaseResponse> CreateCheckListItems(CreateCheckListDto createCheckListDto, List<CreateCheckListItemDto>? createCheckListItems);
        Task<ApiBaseResponse> CreateIndividualTasksItems(CreateIndividualTaskDto createIndividualTaskDto, List<CreateCheckListItemDto>? createCheckListItems);
         
           
        Task<ApiBaseResponse> DeleteCheckListItems(Guid idCheckList, bool trackChanges);
        Task<ApiBaseResponse> BulkDeleteCheckLists(List<Guid> idsCheckList, bool trackChanges);
       Task<ApiBaseResponse> DeleteIndividualTasksItems(Guid idIndividualTasks, bool trackChanges);
        Task<ApiBaseResponse> DeleteTaskItem(Guid idTask, bool trackChanges);

        Task<ApiBaseResponse> UpdateCheckList(Guid idCheckList , CreateCheckListDto updateCheckListDto, bool trackChanges);
        Task<ApiBaseResponse> UpdateTaskItem(Guid idTask, CreateCheckListItemDto updateItemDto, bool trackChanges);
        Task<ApiBaseResponse> UpdateCheckListItems(Guid idCheckList, CreateCheckListDto updateCheckListDto, 
             List<CheckListItemDto>? createCheckListItems, bool trackChanges);



    }
}
