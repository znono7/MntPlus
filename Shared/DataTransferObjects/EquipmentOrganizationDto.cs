using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record EquipmentOrganizationDto
    (
        Guid Id,
        string OrganizationName
        );

    public record EquipmentOrganizationCreateDto
        (
               string OrganizationName
               );
}
