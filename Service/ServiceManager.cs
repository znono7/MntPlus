using AutoMapper;
using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {

        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IAssetService> assetService;
        private readonly Lazy<IUserService> userService;
        private readonly Lazy<ITeamService> teamService;
        private readonly Lazy<IWorkOrderService> workOrderService;
        private readonly Lazy<IWorkOrderHistoryService> workOrderHistoryService;
        private readonly Lazy<IRoleService> roleService;
        private readonly Lazy<IUserRoleService> userRoleService;
        private readonly Lazy<IPartService> partService;
        private readonly Lazy<IInventoryService> inventoryService;
        private readonly Lazy<ILinkPartService> linkPartService;
        private readonly Lazy<IMeterService> meterService;
        private readonly Lazy<IMeterReadingService> meterReadingService;
        private readonly Lazy<ITaskListService> taskListService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper,IUnitOfWork unitOfWork) 
          
        {
            _locationService = new Lazy<ILocationService>(() => new LocationService(repositoryManager, mapper));
            assetService = new Lazy<IAssetService>(() => new AssetService(repositoryManager, mapper));
            userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper));
            teamService = new Lazy<ITeamService>(() => new TeamService(repositoryManager, mapper));
            workOrderService = new Lazy<IWorkOrderService>(() => new WorkOrderService(repositoryManager, mapper , unitOfWork));
            workOrderHistoryService = new Lazy<IWorkOrderHistoryService>(() => new WorkOrderHistoryService(repositoryManager, mapper));
            roleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager, mapper));
            userRoleService = new Lazy<IUserRoleService>(() => new UserRoleService(repositoryManager, mapper));
            partService = new Lazy<IPartService>(() => new PartService(repositoryManager, mapper));
            inventoryService = new Lazy<IInventoryService>(() => new InventoryService(repositoryManager, mapper));
            linkPartService = new Lazy<ILinkPartService>(() => new LinkPartService(repositoryManager, mapper));
            meterService = new Lazy<IMeterService>(() => new MeterService(repositoryManager, mapper));
            meterReadingService = new Lazy<IMeterReadingService>(() => new MeterReadingService(repositoryManager, mapper,unitOfWork));
            taskListService = new Lazy<ITaskListService>(() => new TaskListService(repositoryManager, mapper , unitOfWork));
        }
      

        public ILocationService LocationService => _locationService.Value;

        public IAssetService AssetService => assetService.Value;

        public IUserService UserService => userService.Value;

        public ITeamService TeamService => teamService.Value;

        public IWorkOrderService WorkOrderService => workOrderService.Value;

        public IWorkOrderHistoryService WorkOrderHistoryService => workOrderHistoryService.Value;


        public IRoleService RoleService => roleService.Value;

        public IUserRoleService UserRoleService => userRoleService.Value;

        public IPartService PartService => partService.Value;

        public IInventoryService InventoryService => inventoryService.Value;

        public ILinkPartService LinkPartService => linkPartService.Value;

        public IMeterService MeterService => meterService.Value;

        public IMeterReadingService MeterReadingService => meterReadingService.Value;

        public ITaskListService TaskListService => taskListService.Value;
    }
}
