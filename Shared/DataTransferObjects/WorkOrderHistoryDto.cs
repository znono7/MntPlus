using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record WorkOrderHistoryDto (
        Guid Id,
               string? Status,
                      DateTime? DateChanged,
                             UserDto? ChangedBy,
                                    string? Notes,
                                           Guid? WorkOrderId
                                                 
           );

    public record WorkOrderHistoryCreateDto(
        string? Status,
                      DateTime? DateChanged,
                                           UserDto? ChangedBy,
                                                                       string? Notes,
                                                                                                          Guid? WorkOrderId
                  );
    
}
