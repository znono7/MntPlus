using Entities;

namespace Contracts
{
    public interface ICheckListRepository
    {
        Task<IEnumerable<CheckList>?> GetAllCheckListsAsync(bool trackChanges);
        Task<CheckList?> GetCheckListAsync(Guid checkListId, bool trackChanges);
        void CreateCheckList(CheckList checkList );
        void DeleteCheckList(CheckList checkList);
        void BulkDeleteCheckLists(IEnumerable<CheckList> checkLists);
        Task<IEnumerable<CheckList>?> GetCheckListsByIdsAsync(List<Guid> idsCheckList, bool trackChanges); 
    }
}
