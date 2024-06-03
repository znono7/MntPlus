using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record InventoryDto(Guid Id, string? Status, double? Cost, int AvailableQuantity, int? MinimumQuantity, int? MaxQuantity, DateTime DateReceived, Guid? PartId, PartDto? Part);

    public record InventoryForCreationDto(string? Status, double? Cost, int AvailableQuantity, int? MinimumQuantity, int? MaxQuantity, DateTime DateReceived, Guid PartId);
       
     
} 
