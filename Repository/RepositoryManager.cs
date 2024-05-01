using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
      
        private readonly Lazy<ILocationRepository> locationRepository;
        private readonly Lazy<IAssetRepository> assetRepository;
        private readonly Lazy<IUserRepository> userRepository;
        private readonly Lazy<ITeamRepository> teamRepository;
        private readonly Lazy<IWorkOrderRepository> workOrderRepository;
        private readonly Lazy<IWorkOrderHistoryRepository> workOrderHistoryRepository;









        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            locationRepository = new Lazy<ILocationRepository>(() => new LocationRepository(repositoryContext));
            assetRepository = new Lazy<IAssetRepository>(() => new AssetRepository(repositoryContext));
            userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            teamRepository = new Lazy<ITeamRepository>(() => new TeamRepository(repositoryContext));
            workOrderRepository = new Lazy<IWorkOrderRepository>(() => new WorkOrderRepository(repositoryContext));
            workOrderHistoryRepository = new Lazy<IWorkOrderHistoryRepository>(() => new WorkOrderHistoryRepository(repositoryContext));
              
        }
      

        public ILocationRepository Location => locationRepository.Value;

        public IAssetRepository Asset => assetRepository.Value;

        public IUserRepository User => userRepository.Value;

        public ITeamRepository Team => teamRepository.Value;

        public IWorkOrderRepository WorkOrder => workOrderRepository.Value;

        public IWorkOrderHistoryRepository WorkOrderHistory => workOrderHistoryRepository.Value;
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
        
    }

}
