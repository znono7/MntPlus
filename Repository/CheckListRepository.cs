using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CheckListRepository : RepositoryBase<CheckList>, ICheckListRepository
    {
        public CheckListRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void BulkDeleteCheckLists(IEnumerable<CheckList> checkLists) => DeleteBulk(checkLists);
       

        public void CreateCheckList(CheckList checkList) => Create(checkList);
       

        public void DeleteCheckList(CheckList checkList) => Delete(checkList);
        

        public async Task<IEnumerable<CheckList>?> GetAllCheckListsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            //.Include(c => c.CheckListItems)
            .OrderBy(c => c.Name)
            .ToListAsync();
       

        public async Task<CheckList?> GetCheckListAsync(Guid checkListId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(checkListId), trackChanges)
            .Include(c => c.CheckListItems)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<CheckList>?> GetCheckListsByIdsAsync(List<Guid> idsCheckList, bool trackChanges) =>
            await FindByCondition(c => idsCheckList.Contains(c.Id), trackChanges)
            .ToListAsync();
       
    }

}
