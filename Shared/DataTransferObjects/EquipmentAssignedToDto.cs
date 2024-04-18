﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record EquipmentAssignedToDto
    (
        Guid Id,
        string AssignedToName
        );

    public record AssignedToForCreationDto
        (
               string AssignedToName
               );
        
}
