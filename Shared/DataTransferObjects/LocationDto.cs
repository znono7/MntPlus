using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record LocationDto(
               Guid Id,
               string Name, 
               string? Address,
               bool IsPrimaryLocation,
               Guid? IdParent,
               DateTime CreatedAt
               );

    public record LocationForCreationDto
        (
               string Name,
               string? Address,
               bool IsPrimaryLocation,
               Guid? IdParent,
               DateTime CreatedAt
        );

   
    
}
