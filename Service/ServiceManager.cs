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
        private readonly Lazy<IInstructionService> instructionService;
        private readonly Lazy<IRoleService> roleService;
        private readonly Lazy<IUserRoleService> userRoleService;



        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper) 
          
        {
            _locationService = new Lazy<ILocationService>(() => new LocationService(repositoryManager, mapper));
            assetService = new Lazy<IAssetService>(() => new AssetService(repositoryManager, mapper));
            userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper));
            teamService = new Lazy<ITeamService>(() => new TeamService(repositoryManager, mapper));
            workOrderService = new Lazy<IWorkOrderService>(() => new WorkOrderService(repositoryManager, mapper));
            workOrderHistoryService = new Lazy<IWorkOrderHistoryService>(() => new WorkOrderHistoryService(repositoryManager, mapper));
            instructionService = new Lazy<IInstructionService>(() => new InstructionService(repositoryManager, mapper));
            roleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager, mapper));
            userRoleService = new Lazy<IUserRoleService>(() => new UserRoleService(repositoryManager, mapper));
        }
      

        public ILocationService LocationService => _locationService.Value;

        public IAssetService AssetService => assetService.Value;

        public IUserService UserService => userService.Value;

        public ITeamService TeamService => teamService.Value;

        public IWorkOrderService WorkOrderService => workOrderService.Value;

        public IWorkOrderHistoryService WorkOrderHistoryService => workOrderHistoryService.Value;

        public IInstructionService InstructionService => instructionService.Value;

        public IRoleService RoleService => roleService.Value;

        public IUserRoleService UserRoleService => userRoleService.Value;
    }
}
