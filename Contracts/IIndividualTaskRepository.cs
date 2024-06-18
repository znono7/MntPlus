using Entities;

namespace Contracts
{
    public interface IIndividualTaskRepository
    {
        Task<IEnumerable<IndividualTask>?> GetAllIndividualTasks(bool trackChanges);
        Task<IndividualTask?> GetIndividualTaskById(Guid id,bool trackChanges);
        void CreateIndividualTask(IndividualTask individualTask);
        void DeleteIndividualTask(IndividualTask individualTask);
      


    }
}
