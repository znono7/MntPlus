using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record EquipmentDepartmentDto
    (
        Guid Id,
        string DepartmentName
        );

    public record EquipmentDepartmentCreateDto
        (
               string DepartmentName
           );
}
