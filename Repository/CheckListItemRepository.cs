using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CheckListItemRepository : RepositoryBase<CheckListItem>, ICheckListItemRepository
    {
        public CheckListItemRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateCheckListItemAsync(CheckListItem checkListItem) => Create(checkListItem);
       

        public void CreateCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems) => CreateBulk(checkListItems);
       

        public void DeleteCheckListItemAsync(CheckListItem checkListItem) => Delete(checkListItem);
       

        public void DeleteCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems) => DeleteBulk(checkListItems);
      

        public async Task<IEnumerable<CheckListItem>?> GetAllCheckListItemsAsync(bool trackChanges) => await 
            FindAll(trackChanges)
       
            .ToListAsync();
           
       

        public async Task<CheckListItem?> GetCheckListItemByIdAsync(Guid checkListItemId, bool trackChanges) => await 
            FindByCondition(c => c.Id.Equals(checkListItemId), trackChanges)
            .SingleOrDefaultAsync();
       

        public async Task<IEnumerable<CheckListItem>?> GetCheckListItemsByCheckListIdAsync(Guid checkListId , bool trackChanges) => await 
            FindByCondition(c => c.CheckListId.Equals(checkListId), trackChanges)
            .OrderBy(c => c.Order)
            .ToListAsync();
        

        public async Task<IEnumerable<CheckListItem>?> GetCheckListItemsByIndividualTaskIdAsync(Guid individualTaskId , bool trackChanges) => await 
            FindByCondition(c => c.IndividualTaskId.Equals(individualTaskId), trackChanges)
            .ToListAsync();
       

       

        public void UpdateCheckListItemsAsync(IEnumerable<CheckListItem> checkListItems) => UpdateBulk(checkListItems);
        
    }
   
}
