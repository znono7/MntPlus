using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        ILocationRepository Location { get; }
        IAssetRepository Asset { get; }
        IUserRepository User { get; }
        ITeamRepository Team { get; }
        IWorkOrderRepository WorkOrder { get; }
        IWorkOrderHistoryRepository WorkOrderHistory { get; }
        IInstructionRepository Instruction { get; }
        IRoleRepository Role { get; }
        IUserRoleRepository UserRole { get; }
        IPartRepository Part { get; }
        IInventoryRepository Inventory { get; }
        ILinkPartRepository LinkPart { get; }
        IMeterRepository Meter { get; }
        IMeterReadingRepository MeterReading { get; }


        Task SaveAsync(); 
    }
}
