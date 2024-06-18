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
        private readonly Lazy<IRoleRepository> roleRepository;
        private readonly Lazy<IUserRoleRepository> userRoleRepository;
        private readonly Lazy<IPartRepository> partRepository;
        private readonly Lazy<IInventoryRepository> inventoryRepository;
        private readonly Lazy<ILinkPartRepository> linkPartRepository;
        private readonly Lazy<IMeterRepository> meterRepository;
        private readonly Lazy<IMeterReadingRepository> meterReadingRepository;
        private readonly Lazy<ICheckListRepository> checkListRepository;
        private readonly Lazy<IIndividualTaskRepository> individualTaskRepository;
        private readonly Lazy<ICheckListItemRepository> checkListItemRepository;




        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            locationRepository = new Lazy<ILocationRepository>(() => new LocationRepository(repositoryContext));
            assetRepository = new Lazy<IAssetRepository>(() => new AssetRepository(repositoryContext));
            userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            teamRepository = new Lazy<ITeamRepository>(() => new TeamRepository(repositoryContext));
            workOrderRepository = new Lazy<IWorkOrderRepository>(() => new WorkOrderRepository(repositoryContext));
            workOrderHistoryRepository = new Lazy<IWorkOrderHistoryRepository>(() => new WorkOrderHistoryRepository(repositoryContext));
            roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(repositoryContext));
            userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(repositoryContext));
              partRepository = new Lazy<IPartRepository>(() => new PartRepository(repositoryContext));
            inventoryRepository = new Lazy<IInventoryRepository>(() => new InventoryRepository(repositoryContext));
            linkPartRepository = new Lazy<ILinkPartRepository>(() => new LinkPartRepository(repositoryContext));
            meterRepository = new Lazy<IMeterRepository>(() => new MeterRepository(repositoryContext));
            meterReadingRepository = new Lazy<IMeterReadingRepository>(() => new MeterReadingRepository(repositoryContext));
            checkListRepository = new Lazy<ICheckListRepository>(() => new CheckListRepository(repositoryContext));
            individualTaskRepository = new Lazy<IIndividualTaskRepository>(() => new IndividualTaskRepository(repositoryContext));
            checkListItemRepository = new Lazy<ICheckListItemRepository>(() => new CheckListItemRepository(repositoryContext));
        }
      

        public ILocationRepository Location => locationRepository.Value;

        public IAssetRepository Asset => assetRepository.Value;

        public IUserRepository User => userRepository.Value;

        public ITeamRepository Team => teamRepository.Value;

        public IWorkOrderRepository WorkOrder => workOrderRepository.Value;

        public IWorkOrderHistoryRepository WorkOrderHistory => workOrderHistoryRepository.Value;


        public IRoleRepository Role => roleRepository.Value;

        public IUserRoleRepository UserRole => userRoleRepository.Value;

        public IPartRepository Part => partRepository.Value;

        public IInventoryRepository Inventory => inventoryRepository.Value;

        public ILinkPartRepository LinkPart => linkPartRepository.Value;

        public IMeterRepository Meter => meterRepository.Value;

        public IMeterReadingRepository MeterReading => meterReadingRepository.Value;

        public ICheckListRepository CheckList => checkListRepository.Value;

        public IIndividualTaskRepository IndividualTask => individualTaskRepository.Value;

        public ICheckListItemRepository CheckListItem => checkListItemRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
        
    }

}
