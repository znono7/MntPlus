using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        ILocationService LocationService { get; }
        IAssetService AssetService { get; }
        IUserService UserService { get; }
        ITeamService TeamService { get; }
        IWorkOrderService WorkOrderService { get;}
        IWorkOrderHistoryService WorkOrderHistoryService { get; }
        IInstructionService InstructionService { get; }
        IRoleService RoleService { get; }
        IUserRoleService UserRoleService { get; }
        IPartService PartService { get; }
        IInventoryService InventoryService { get; }
        ILinkPartService LinkPartService { get; }
      

    }
}
