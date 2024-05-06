using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record WorkOrderDto (
        Guid Id, 
        string? Name, 
        string? Instructions,  
        string? Priority, 
        DateTime? DueDate, 
        string? Type, 
        string? Status, 
        Guid? UserAssignedToId,
        UserDto? UserAssignedTo, 
        Guid? TeamAssignedToId,
        TeamDto? TeamAssignedTo, 
        Guid? AssetId,
        AssetDto? Asset); 

    public record WorkOrderForCreationDto(
               string? Name, 
                      string? Instructions, 
                             string? Priority, 
                                    DateTime? DueDate, 
                                           string? Type, 
                                                  string? Status, 
                                                         Guid? UserAssignedToId, 
                                                                Guid? TeamAssignedToId, 
                                                                       Guid? AssetId);
    
}
