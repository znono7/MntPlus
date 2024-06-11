using Entities;

namespace Contracts
{
    public interface ICheckListRepository
    {
        Task<IEnumerable<CheckList>?> GetAllCheckListsAsync(bool trackChanges);
        Task<CheckList?> GetCheckListAsync(Guid checkListId, bool trackChanges);
        void CreateCheckList(CheckList checkList );
        void DeleteCheckList(CheckList checkList);
    }
}
