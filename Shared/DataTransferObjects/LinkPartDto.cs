using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record LinkPartDto(Guid Id, Guid PartId, PartDto Part, Guid AssetId, AssetDto Asset);
    public record LinkPartForCreationDto(Guid PartId, Guid AssetId);
   
}
