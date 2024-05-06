using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record InstructionDto (Guid Id, string? Description);

    public record InstructionDtoForCreation (string? Description, Guid? WorkOrderID);
    
    
}
