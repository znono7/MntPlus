using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record EquipmentTypeDto
    (
        Guid Id,
        string EquipmentTypeName
    );

    public record EquipmentTypeDtoCreate
    (
               string EquipmentTypeName
           );
} 
