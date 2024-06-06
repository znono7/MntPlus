using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record MeterReadingDto
    (
        Guid Id,
        Guid MeterId,   
        double Reading,
        DateTime Timestamp,
        Guid? UserId,
        string? UserFullName 
        );

    public record MeterReadingDtoForCreation
        (
               Guid MeterId,   
               double Reading,
               DateTime Timestamp,
               Guid? UserId 
               );
}
