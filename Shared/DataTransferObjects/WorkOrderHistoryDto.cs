using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record WorkOrderHistoryDto 
        (
        Guid Id,
            string? Notes,
            string? Status,
            DateTime? DateChanged,
            UserDto? ChangedBy,
            Guid? WorkOrderId
        );
        

    public record WorkOrderHistoryCreateDto
     (
        
            string? Notes,
            string? Status,
            DateTime? DateChanged,
            Guid? ChangedById,
            Guid? WorkOrderId
        );
}
