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
        int? Number, 
        string? Description,
        string? Priority, 
        DateTime? StartDate,  
        DateTime? DueDate,
        string? Type, 
        string? Status,
        string? Requester,  
        DateTime? CreatedOn,
        Guid? UserCreatedId,
        UserByDto? UserCreatedBy,
        Guid? UserAssignedToId,
        UserByDto? UserAssignedTo, 
        Guid? TeamAssignedToId, 
        TeamDto? TeamAssignedTo, 
        Guid? AssetId,
        AssetWorkOrderDto? Asset); 

    public record WorkOrderForCreationDto
        (
          string? Name, 
        int? Number,
        string? Description,
        string? Priority, 
        DateTime? StartDate,
        DateTime? DueDate,
        string? Type,
        string? Status,
        string? Requester,
        DateTime? CreatedOn,
        Guid? UserCreatedId,
        Guid? UserAssignedToId,
        Guid? TeamAssignedToId,
        Guid? AssetId
             );
    
}
