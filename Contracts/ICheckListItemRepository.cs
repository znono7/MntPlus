using Entities;


namespace Contracts
{
    public interface ICheckListItemRepository
    {
        Task<IEnumerable<CheckListItem>?> GetAllCheckListItemsAsync(bool trackChanges);
        Task<CheckListItem?> GetCheckListItemByIdAsync(Guid checkListItemId , bool trackChanges);
        Task<IEnumerable<CheckListItem>?> GetCheckListItemsByCheckListIdAsync(Guid checkListId, bool trackChanges);
        Task<IEnumerable<CheckListItem>?> GetCheckListItemsByIndividualTaskIdAsync(Guid individualTaskId , bool trackChanges);
        void CreateCheckListItemAsync(CheckListItem checkListItem);
        void DeleteCheckListItemAsync(CheckListItem checkListItem);

        void CreateCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems);
        void UpdateCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems);
        void DeleteCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems);
    }
}
