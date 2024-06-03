using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record PartDto(Guid Id, string? PartNumber, string? Name, string? Description, 
        string? Category, byte[]? Image, List<InventoryDto>? Inventories, List<LinkPartDto>? LinkParts);
     
    public record PartForCreationDto(string? PartNumber, string? Name, string? Description, 
               string? Category, byte[]? Image);
    
}
  